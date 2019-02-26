using FryScript.Compilation;
using FryScript.ScriptProviders;
using System;

namespace FryScript
{
    public class ScriptRuntime : IScriptRuntime
    {
        private readonly IScriptProvider[] _scriptProviders;
        private readonly IScriptCompiler _compiler;
        private readonly IObjectRegistry _registry;

        public ScriptRuntime(IScriptProvider[] scriptProviders, IScriptCompiler compiler, IObjectRegistry registry)
        {
            _scriptProviders = scriptProviders ?? throw new ArgumentNullException(nameof(scriptProviders));
            _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public IScriptObject Get(string name)
        {
            name = string.IsNullOrWhiteSpace(name) 
                ? throw new ArgumentNullException(nameof(name))
                : name;

            if (_registry.TryGetObject(name, out IScriptObject obj))
                return obj;

            throw new NotImplementedException();
        }
    }
}
