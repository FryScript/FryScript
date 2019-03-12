using System;

namespace FryScript.ScriptProviders
{
    public interface IScriptProvider
    {
        bool TryGetScriptInfo(string path, out ScriptInfo scriptInfo, string relativeTo = null);
    }
}
