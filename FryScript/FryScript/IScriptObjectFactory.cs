using System;

namespace FryScript
{
    public interface IScriptObjectFactory
    {
        IScriptObject Create(Type type, Func<IScriptObject, object> ctor, Uri uri, IScriptObjectBuilder parent);

        IScriptObject CreatePrimitive(Type type);
    }
}
