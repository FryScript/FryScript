using System;
using System.Dynamic;

namespace FryScript
{
    public interface IScriptObject : IDynamicMetaObjectProvider
    {
        ObjectCore ObjectCore { get; }
        object Target { get; set; }
        bool HasTarget { get; }
        Type TargetType { get; }
        object[] MemberData { get; set; }
        MemberIndex MemberIndex { get; set; }
    }
}
