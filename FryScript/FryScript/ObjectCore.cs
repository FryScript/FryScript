using System;

namespace FryScript
{
    public class ObjectCore
    {
        public object[] MemberData;
        public MemberIndex MemberIndex;
        public Uri Uri;
        public Func<IScriptObject, object> Ctor;

        public ObjectCore()
        {
            MemberIndex = MemberIndex.Root;
        }
    }
}
