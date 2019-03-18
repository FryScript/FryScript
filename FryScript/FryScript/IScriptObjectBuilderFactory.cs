using System;

namespace FryScript
{
    public interface IScriptObjectBuilderFactory
    {
        IScriptObjectBuilder Create(Type type, Func<IScriptObject, object> ctor, Uri uri);
    }
}
