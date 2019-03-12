using System;

namespace FryScript.ScriptProviders
{
    public interface IScriptProvider
    {
        bool TryGetUri(string path, out Uri uri, string relativeTo = null);

        string GetScript(Uri uri);

        bool TryGetScriptInfo(string path, out ScriptInfo scriptInfo, string relativeTo = null);
    }
}
