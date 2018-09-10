using System;

namespace FryScript.ScriptProviders
{
    public interface IScriptProvider
    {
        bool TryGetUri(string path, out Uri uri, string relativeTo = null);

        string GetScript(Uri uri);
    }
}
