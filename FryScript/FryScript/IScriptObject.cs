using System;
using System.Collections.Generic;

namespace FryScript
{
    public interface IScriptObject
    {
        object Target { get; set; }
        bool HasTarget { get; }
        Type TargetType { get; }
        object[] MemberData { get; set; }
        MemberIndex MemberIndex { get; set; }
        object SetMember(int index, object value);
        object SetIndex(string name, object value);
        object GetMember(int index);
        object GetIndex(string name);
        bool IsValidSetMember(MemberIndex memberIndex);
        bool IsValidGetMember(MemberIndex memberIndex);
        IEnumerable<string> GetMembers();
    }
}
