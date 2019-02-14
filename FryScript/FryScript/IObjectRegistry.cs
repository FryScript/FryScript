using System;

namespace FryScript
{
    public interface IObjectRegistry
    {
        IScriptObject Import(string name, IScriptObject obj);

        IScriptObject Import(Type type, string name = null, bool autoConstruct = true);

        bool TryGetObject(string name, out IScriptObject obj);
    }
}
