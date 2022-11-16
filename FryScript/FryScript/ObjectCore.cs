using System;

namespace FryScript
{
    public sealed class ObjectCore
    {
        public object[] MemberData;
        public MemberIndex MemberIndex;
        public IScriptObjectBuilder Builder;
        public Uri Uri => Builder?.Uri;

        public ObjectCore()
        {
            MemberIndex = MemberIndex.Root;
        }
    }
}
