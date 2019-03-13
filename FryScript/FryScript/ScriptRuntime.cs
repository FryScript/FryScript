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

            if (!_scriptProvider.TryGetScriptInfo(key, out ScriptInfo scriptInfo))
                throw new ScriptLoadException(name, relativeTo);

            if (_registry.TryGetObject(scriptInfo.Uri.AbsoluteUri, out obj))
                return obj;

            throw new NotImplementedException();
        }
    }
}
