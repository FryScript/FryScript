using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using FryScript.HostInterop;
using FryScript.ScriptProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FryScript
{
    public class ScriptRuntime : IScriptRuntime

    {
        private readonly ScriptObject _evalContext = new ScriptObject();
        private readonly IScriptProvider _scriptProvider;
        private readonly IScriptCompiler _compiler;
        private readonly IObjectRegistry _registry;
        private readonly IScriptObjectFactory _objectFactory;
        private readonly ITypeProvider _typeProvider;

        private readonly Queue<string> _compileQueue = new Queue<string>();

        public bool DetailedExceptions { get; set; }

        public DebugHook DebugHook { get; set; }

#if NETSTANDARD2_0
        public ScriptRuntime()
            : this(new DirectoryScriptProvider(Environment.CurrentDirectory),
                  new ScriptCompiler(),
                  new ObjectRegistry(),
                  new ScriptObjectFactory(),
                  TypeProvider.Current)
        {
        }
#else
        public ScriptRuntime()
            : this(new DirectoryScriptProvider(AppDomain.CurrentDomain.BaseDirectory),
                  new ScriptCompiler(),
                  new ObjectRegistry(),
                  new ScriptObjectFactory(),
                  TypeProvider.Current)
        {
        }
#endif

        public ScriptRuntime(
            IScriptProvider scriptProvider,
            IScriptCompiler compiler,
            IObjectRegistry registry,
            IScriptObjectFactory objectFactory,
            ITypeProvider typeProvider)
        {
            _scriptProvider = scriptProvider ?? throw new ArgumentNullException(nameof(scriptProvider));
            _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
            _typeProvider = typeProvider ?? throw new ArgumentNullException(nameof(typeProvider));

            _registry.Import("error", new ScriptError());
            _registry.Import("array", new ScriptArray());
            _registry.Import("function", new ScriptFunction(new Action(() => { })));
            _registry.Import("object", new ScriptObject());

            foreach (var type in _typeProvider.GetPrimitives())
            {
                var primitive = _objectFactory.CreatePrimitive(type);
                _registry.Import(type.Name, primitive);
            }
        }
        public object Eval(string script)
        {
            if (string.IsNullOrWhiteSpace(script))
                throw new ArgumentNullException(nameof(script));

            var func = _compiler.Compile2(script, "eval", new CompilerContext(this, null, true));

            return func(_evalContext);
        }

        public IScriptObject Get(string name, Uri relativeTo = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            if (_registry.TryGetObject(name, out IScriptObject obj))
                return obj;

            name = Path.ChangeExtension(name, ".fry");

            var key = relativeTo == null
                ? name
                : $"{name} -> {relativeTo.AbsoluteUri}";

            if (_registry.TryGetObject(key, out obj))
                return obj;

            if (!_scriptProvider.TryGetScriptInfo(name, out ScriptInfo scriptInfo, relativeTo))
                throw new ScriptLoadException(name, relativeTo?.AbsoluteUri);

            var resolvedName = scriptInfo.Uri.AbsoluteUri;

            if (_registry.TryGetObject(resolvedName, out obj))
            {
                _registry.Import(key, obj);
                return obj;
            }

            if (_compileQueue.Any(q => q.Equals(resolvedName, StringComparison.OrdinalIgnoreCase)))
                throw ExceptionHelper.CircularDependency(resolvedName, _compileQueue);

            _compileQueue.Enqueue(resolvedName);

            var context = new CompilerContext(this, scriptInfo.Uri);
            var ctor = _compiler.Compile2(scriptInfo.Source, resolvedName, context);
            var instance = _objectFactory.Create(context.ScriptType, ctor, scriptInfo.Uri);

            _registry.Import(resolvedName, instance);
            _registry.Import(key, instance);

            _compileQueue.Dequeue();

            return instance;
        }

        public IScriptObject Import(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            var scriptableType = type.GetTypeInfo().GetCustomAttribute<ScriptableTypeAttribute>();

            if (scriptableType == null)
                throw new ArgumentException($"Type {type.FullName} is not decorated with {typeof(ScriptableTypeAttribute).FullName}", nameof(type));

            if (_registry.TryGetObject(scriptableType.Name, out IScriptObject obj))
                return obj;

            obj = _objectFactory.Create(type, o => o, new Uri($"runtime://{scriptableType.Name}"));

            _registry.Import(scriptableType.Name, obj);

            return obj;
        }

        public IScriptObject New(string name, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var instance = Get(name).ObjectCore.Builder.Build();

            if (instance.HasMemberOfType("ctor", typeof(ScriptFunction)))
                instance.InvokeMember("ctor", args);

            return instance;
        }
    }
}
