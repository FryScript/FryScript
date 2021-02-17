using System;
using System.Dynamic;

namespace FryScript
{
    public interface IScriptObject : IDynamicMetaObjectProvider
    {
        ObjectCore ObjectCore { get; }
    }
}
