using System;

namespace FryScript
{
    public class ObjectCore
    {
        public readonly object[] MemberData;
        public readonly MemberIndex MemberIndex;

        public ObjectCore()
        {
            MemberData = new object[16];
            MemberIndex = MemberIndex.Root;
        }
    }
}
