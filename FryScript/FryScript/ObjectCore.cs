namespace FryScript
{
    public class ObjectCore
    {
        public object[] MemberData;
        public MemberIndex MemberIndex;
        public IScriptObjectBuilder Builder;

        public ObjectCore()
        {
            MemberIndex = MemberIndex.Root;
        }
    }
}
