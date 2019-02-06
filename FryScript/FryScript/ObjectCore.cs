using FryScript.Binders;
using System;
using System.Runtime.CompilerServices;

namespace FryScript
{
    public class ObjectCore
    {
        public object[] MemberData;
        public MemberIndex MemberIndex;

        public ObjectCore()
        {
            MemberIndex = MemberIndex.Root;
        }
    }
}
