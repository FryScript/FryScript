namespace FryScript.HostInterop.Extensions
{
    public static class FallbackExtensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this object value)
        {
            return value.ToString();
        }
    }
}
