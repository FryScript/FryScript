using System;

namespace FryScript
{
    public interface IObjectRegistry
    {
        void Import(string name, IScriptObject obj);

        void Import(Type type);

        bool TryGetObject(string name, out IScriptObject obj);
    }
}
