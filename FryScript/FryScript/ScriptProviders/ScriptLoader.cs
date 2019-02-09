using System;

namespace FryScript.ScriptProviders
{
    public class ScriptLoader : IScriptLoader
    {
        private readonly IScriptProvider[] _scriptProviders;

        public ScriptLoader(IScriptProvider[] scriptProviders)
        {
            _scriptProviders = scriptProviders == null || scriptProviders.Length == 0
                ? throw new ArgumentNullException(nameof(scriptProviders))
                : scriptProviders;
        }

        public string Load(string path, string relativeTo)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            foreach(var scriptProvider in _scriptProviders)
            {
                if (!scriptProvider.TryGetUri(path, out Uri uri, relativeTo))
                    continue;

                return scriptProvider.GetScript(uri);
            }

            throw new ScriptLoadException($"Unable to load script \"{path}\" relative to \"{relativeTo}\"");
        }
    }
}
