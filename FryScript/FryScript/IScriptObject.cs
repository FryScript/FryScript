using System;
using System.Collections.Generic;

namespace FryScript
{
    public interface IScriptObject
    {
        ObjectCore ObjectCore { get; }
        object Target { get; set; }
        bool HasTarget { get; }
        Type TargetType { get; }
        object[] MemberData { get; set; }
        MemberIndex MemberIndex { get; set; }
    }
}
