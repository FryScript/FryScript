namespace FryScript.HostInterop.Extensions
{
    public static class Int32Extensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this int value)
        {
            return value.ToString();
        }

        [ScriptableMethod("toBool")]
        public static object ToBool(this int value)
        {
            return value != 0;
        }

        [ScriptableMethod("toFloat")]
        public static object ToFloat(this int value)
        {
            return (float)value;
        }

        [ScriptableMethod("toInt")]
        public static object ToInt(this int value)
        {
            return value;
        }
    }
}
