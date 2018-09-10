namespace FryScript.HostInterop.Extensions
{
    public static class FloatExtensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this float value)
        {
            return value.ToString();
        }

        [ScriptableMethod("toBool")]
        public static object ToBool(this float value)
        {
            return value != 0;
        }

        [ScriptableMethod("toFloat")]
        public static object ToFloat(this float value)
        {
            return value;
        }

        [ScriptableMethod("toInt")]
        public static object ToInt(this float value)
        {
            return (int)value;
        }
    }
}
