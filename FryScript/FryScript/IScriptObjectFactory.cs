using System;

namespace FryScript
{
    public interface IScriptObjectFactory
    {
        IScriptObject Create(Type type, Func<IScriptObject, object> ctor, Uri uri);

        IScriptObject CreatePrimitive(Type type);
    }
}
