using FryScript.CallSites;

namespace FryScript.HostInterop.Extensions
{
    public static class FallbackExtensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this object value)
        {
            return value.ToString();
        }

        [ScriptableMethod("hasMember")]
        public static object HasMember(this object source, string name)
        {
            return CallSiteCache.Current.HasMember(name, source);
        }
    }
}
