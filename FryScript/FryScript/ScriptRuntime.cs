using FryScript.Compilation;
using FryScript.ScriptProviders;
using System;

namespace FryScript
{
    public class ScriptRuntime : IScriptRuntime
    {
        private readonly IScriptProvider _scriptProvider;
        private readonly IScriptCompiler _compiler;
        private readonly IObjectRegistry _registry;

        public ScriptRuntime(IScriptProvider scriptProvider, IScriptCompiler compiler, IObjectRegistry registry)
        {
            _scriptProvider = scriptProvider ?? throw new ArgumentNullException(nameof(scriptProvider));
            _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public IScriptObject Get(string name, string relativeTo = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var key = relativeTo == null
                ? name
                : $"{name} : {relativeTo}";

            if (_registry.TryGetObject(key, out IScriptObject obj))
                return obj;

            if (!_scriptProvider.TryGetScriptInfo(name, out ScriptInfo scriptInfo))
                throw new ScriptLoadException(name, relativeTo);

            var resolvedName = scriptInfo.Uri.AbsoluteUri;

            if (_registry.TryGetObject(resolvedName, out obj))
            {
                _registry.Import(key, obj);
                return obj;
            }

            var context = new CompilerContext(this, resolvedName);
            var ctor = _compiler.Compile2(scriptInfo.Source, resolvedName, context);

            var instance = Activator.CreateInstance(context.ScriptType) as IScriptObject;

            ctor(instance);

            instance.ObjectCore.Uri = scriptInfo.Uri;
            instance.ObjectCore.Ctor = ctor;

            _registry.Import(resolvedName, instance);

            return instance;
        }
    }
}
