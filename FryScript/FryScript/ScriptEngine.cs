using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using FryScript.HostInterop;
using FryScript.Parsing;
using FryScript.ScriptProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FryScript
{
    public class ScriptEngine// : IScriptEngine
    {
        public const string DefaultExtension = "fry";
        private readonly IScriptCompiler _compiler;
        private readonly string _extension;
        private readonly ConcurrentDictionary<string, ScriptObject> _scripts = new ConcurrentDictionary<string, ScriptObject>(StringComparer.OrdinalIgnoreCase);
        private readonly IList<IScriptProvider> _scriptProviders;
        private readonly Queue<string> _compileQueue = new Queue<string>();

        public string Extension { get { return _extension; } }

        public IScriptCompiler Compiler { get { return _compiler; } }

        public DebugHook DebugHook { get; set; }

        public bool DetailedExceptions { get; set; }


#if NETSTANDARD2_0
        public ScriptEngine()
            : this(
            new ScriptCompiler(new ScriptParser()),
            DefaultExtension,
            new List<IScriptProvider> { new DirectoryScriptProvider(AppContext.BaseDirectory) })
        {
        }
#else
        public ScriptEngine()
            : this(
            new ScriptCompiler(new ScriptParser()),
            DefaultExtension,
            new List<IScriptProvider> { new DirectoryScriptProvider(AppDomain.CurrentDomain.BaseDirectory) })
        {
        }
#endif

        public ScriptEngine(IScriptCompiler compiler, string extension, IList<IScriptProvider> scriptProviders)
        {
            if (compiler == null)
                throw new ArgumentNullException("compiler");

            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException("extension");

            if (scriptProviders == null)
                throw new ArgumentNullException(nameof(scriptProviders));

            _compiler = compiler;
            _extension = extension;
            _scriptProviders = scriptProviders;

            DetailedExceptions = true;

            RegisterPrimitiveTypes();

            Import<ScriptError>();
        }

        public dynamic Eval(string script)
        {
            return Eval(script, script);
        }

        public dynamic Eval(string name, string script)
        {
            if (string.IsNullOrWhiteSpace(script))
                throw new ArgumentNullException("script");

            var compilerContext = new CompilerContext(this, name);

            var func = _compiler.Compile(script, name, compilerContext);

            var scriptObject = new ScriptObject(
                scriptType: name,
                extends: compilerContext.Extends,
                ctor: func,
                autoConstruct: false
                );

            CreateProto(scriptObject, compilerContext);

            return func(scriptObject);
        }

        public ScriptObject Compile(string name, string script)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrWhiteSpace(script))
                throw new ArgumentNullException("script");

            var keyName = ScriptTypeHelper.NormalizeTypeName(name);

            ScriptObject scriptObject;
            if (_scripts.TryGetValue(keyName, out scriptObject))
                throw new ArgumentException(string.Format("A script named {0} has already been compiled", name));

            if (_compileQueue.Any(c => c == keyName))
                throw ExceptionHelper.CircularDependency(keyName, _compileQueue);

            _compileQueue.Enqueue(keyName);

            var compilerContext = new CompilerContext(this, keyName);

            var func = _compiler.Compile(script, keyName, compilerContext);

            scriptObject = new ScriptObject(keyName, func, compilerContext.Extends);

            CreateProto(scriptObject, compilerContext);

            _compileQueue.Dequeue();
            return _scripts[keyName] = scriptObject;
        }

        public IScriptObject Get(string name)
        {
            return Get(name, null);
        }

        internal IScriptObject Get(string name, string relativeTo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            ScriptObject scriptObject;
            if (_scripts.TryGetValue(name, out scriptObject))
                return scriptObject;

            name = Path.ChangeExtension(name, _extension);

            TryResolveUri(name, relativeTo, out Uri uri, out IScriptProvider provider);

            if (_scripts.TryGetValue(uri.AbsoluteUri, out scriptObject))
                return scriptObject;

            var script = provider.GetScript(uri);

            DebugHook?.Invoke(new DebugContext(DebugEvent.RegisterScript, uri.LocalPath, 0, 0, 0, new[]
            {
               new DebugVariable(uri.LocalPath, script)
            }));

            return Compile(uri.AbsoluteUri, script);
        }

        //public T Get<T>(string name)
        //    where T : class
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //        throw new ArgumentNullException("name");

        //    var scriptObject = Get(name);

        //    //if (!(scriptObject.Target is T))
        //    //    throw new ArgumentException(string.Format("Script {0} cannot be converted to type {1}", name, typeof(T).FullName), "name");

        //    return scriptObject.Target as T;
        //}

        public void Import(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var name = TypeProvider.Current.GetTypeName(type);
            var keyName = ScriptTypeHelper.NormalizeTypeName(name);

            if (_scripts.ContainsKey(keyName))
                throw new ArgumentException(string.Format("A script named {0} has already been compiled", name));

            var ctor = TypeProvider.Current.GetCtor(type);

            _scripts[keyName] = new ScriptObject(
                scriptType: keyName,
                ctor: ctor,
                extends: new HashSet<string>()
                );
        }

        public void Import<T>()
            where T : class, new()
        {
            var type = typeof(T);

            var proxyType = TypeProvider.Current.GetProxy(typeof(T));
            Import(proxyType);
        }

        public void Import<T>(string name, Func<T> ctor)
            where T : class, IScriptable
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (ctor == null)
                throw new ArgumentNullException("ctor");

            var keyName = ScriptTypeHelper.NormalizeTypeName(name);

            if (_scripts.ContainsKey(keyName))
                throw new ArgumentException(string.Format("A script named {0} has already been compiled", name));

            var scriptCtor = ScriptableCtorHelper.GetCtor(ctor);

            _scripts[keyName] = new ScriptObject(
                scriptType: keyName,
                ctor: scriptCtor,
                extends: new HashSet<string>()
                );
        }

        //public ScriptObject Bind(IScriptable scriptable, string name, params object[] args)
        //{
        //    if (scriptable == null)
        //        throw new ArgumentNullException("scriptable");

        //    if (string.IsNullOrWhiteSpace(name))
        //        throw new ArgumentNullException("name");

        //    var keyName = ScriptTypeHelper.NormalizeTypeName(name);

        //    var scriptObject = Get(keyName);

        //    //if (scriptObject.HasTarget && scriptObject.TargetType != scriptable.GetType())
        //    //    throw new ArgumentException(string.Format("Scriptable type {0} does not match the native type of script {1} expected {2}", scriptable.GetType().FullName, name, scriptObject.TargetType.FullName), "scriptable");

        //    if (scriptable.Script != null)
        //        throw new ArgumentException("Scriptable type has already been bound to a script", "scriptable");

        //    var newScriptObject = new ScriptObject(scriptObject.GetScriptType(), scriptObject.Ctor, scriptObject.Extends);

        //    InvokeConstructor(newScriptObject, args);

        //    return newScriptObject;
        //}

        //public ScriptObject New(string name, params object[] args)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        throw new ArgumentNullException("name");

        //    var keyName = ScriptTypeHelper.NormalizeTypeName(name);

        //    var scriptObject = Get(keyName);

        //    var newScriptObject = scriptObject.CreateInstance();

        //    DebugHook?.Invoke(new DebugContext(DebugEvent.PushStackFrame, "[HOST]", 0, 0, 0, null));
        //    InvokeConstructor(newScriptObject, args);
        //    DebugHook?.Invoke(new DebugContext(DebugEvent.PopStackFrame, "[HOST]", 0, 0, 0, null));

        //    return newScriptObject;
        //}

        public T New<T>(string name, params object[] args)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var keyName = ScriptTypeHelper.NormalizeTypeName(name);

            var scriptObject = Get(keyName);

            //if (scriptObject.Target == null || !typeof(T).IsAssignableFrom(scriptObject.TargetType))
            //    throw new ArgumentException(string.Format("Cannot create a new instance of type {0} bound to script {1}", typeof(T).FullName, name), "name");

            //var newScriptObject = scriptObject.CreateInstance();

            //InvokeConstructor(newScriptObject, args);

            //return newScriptObject.Target as T;

            return Activator.CreateInstance(scriptObject.GetType()) as T;
        }

        private bool TryResolveUri(string name, string relativeTo, out Uri uri, out IScriptProvider provider)
        {
            provider = null;

            foreach (var currentProvider in _scriptProviders)
            {
                if (!currentProvider.TryGetUri(name, out uri, relativeTo))
                    continue;

                provider = currentProvider;
                return true;
            }

            throw new ArgumentException($"Unable to locate a script at path {name}");
        }

        private void RegisterPrimitiveTypes()
        {
            foreach (var primitiveType in TypeProvider.Current.GetPrimitives())
            {
                var wrapperType = typeof(ScriptPrimitive<>).MakeGenericType(primitiveType);
                var wrapperInstance = (ScriptObject)Activator.CreateInstance(wrapperType);

                _scripts[wrapperInstance.ScriptType] = wrapperInstance;
            }
        }

        private static void InvokeConstructor(ScriptObject obj, params object[] args)
        {
            ScriptFunction ctor;
            if (!obj.TryGetMemberOfType("ctor", out ctor))
                return;

            ctor.Invoke(args);
        }

        private static void CreateProto(ScriptObject scriptObject, CompilerContext compilerContext)
        {
            if (compilerContext.ProtoCtor == null || compilerContext.ProtoReference == null)
                return;

            compilerContext.ProtoCtor(scriptObject);
            compilerContext.ProtoReference.SetResolver(() => scriptObject);
        }
    }
}
