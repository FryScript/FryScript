namespace FryScript.HostInterop.Extensions
{
    public static class BooleanExtensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this bool value)
        {
            return value ? "true" : "false";
        }

        [ScriptableMethod("toBool")]
        public static object ToBool(this bool value)
        {
            return value;
        }

        [ScriptableMethod("toInt")]
        public static object ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        [ScriptableMethod("toFloat")]
        public static object ToFloat(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
