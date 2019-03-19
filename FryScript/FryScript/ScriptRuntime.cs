using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.HostInterop;
using FryScript.ScriptProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FryScript
{
    public class ScriptRuntime : IScriptRuntime
    {
        private readonly IScriptProvider _scriptProvider;
        private readonly IScriptCompiler _compiler;
        private readonly IObjectRegistry _registry;
        private readonly IScriptObjectBuilderFactory _builderFactory;
        private readonly ITypeFactory _typeFactory;

        private readonly Queue<string> _compileQueue = new Queue<string>();

#if NETSTANDARD2_0
        public ScriptRuntime()
            : this(new DirectoryScriptProvider(AppContext.BaseDirectory),
                  new ScriptCompiler(),
                  new ObjectRegistry(),
                  new ScriptObjectBuilderFactory(),
                  TypeFactory.Current)
        {
        }
#else
        public ScriptRuntime()
            : this(new DirectoryScriptProvider(AppDomain.CurrentDomain.BaseDirectory),
                  new ScriptCompiler(),
                  new ObjectRegistry(),
                  new ScriptObjectBuilderFactory(),
                  TypeFactory.Current)
        {
        }
#endif

        public ScriptRuntime(
            IScriptProvider scriptProvider, 
            IScriptCompiler compiler, 
            IObjectRegistry registry, 
            IScriptObjectBuilderFactory builderFactory,
            ITypeFactory typeFactory)
        {
            _scriptProvider = scriptProvider ?? throw new ArgumentNullException(nameof(scriptProvider));
            _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _builderFactory = builderFactory ?? throw new ArgumentNullException(nameof(builderFactory));
            _typeFactory = typeFactory ?? throw new ArgumentNullException(nameof(typeFactory));
        }

        public IScriptObject Get(string name, string relativeTo = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            name = Path.ChangeExtension(name, ".fry").ToLower();

            var key = (relativeTo == null
                ? name
                : $"{name} -> {relativeTo}").ToLower();

            if (_registry.TryGetObject(key, out IScriptObject obj))
                return obj;

            if (!_scriptProvider.TryGetScriptInfo(name, out ScriptInfo scriptInfo, relativeTo))
                throw new ScriptLoadException(name, relativeTo);

            var resolvedName = scriptInfo.Uri.AbsoluteUri;

            if (_registry.TryGetObject(resolvedName, out obj))
            {
                _registry.Import(key, obj);
                return obj;
            }

            if(_compileQueue.Any(q => q == resolvedName))
                throw ExceptionHelper.CircularDependency(resolvedName, _compileQueue);

            _compileQueue.Enqueue(resolvedName);

            var context = new CompilerContext(this, resolvedName);
            var ctor = _compiler.Compile2(scriptInfo.Source, resolvedName, context);
            var scriptType = _typeFactory.CreateScriptableType(context.ScriptType);
            var builder = _builderFactory.Create(scriptType, ctor, scriptInfo.Uri);

            var instance = builder.Build();

            _registry.Import(resolvedName, instance);
            _registry.Import(key, instance);

            _compileQueue.Dequeue();

            return instance;
        }
    }
}
