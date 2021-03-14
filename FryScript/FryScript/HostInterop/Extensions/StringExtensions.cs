using FryScript.Helpers;

namespace FryScript.HostInterop.Extensions
{
    public static class StringExtensions
    {
        [ScriptableMethod("toString")]
        public static object ToString(this string value)
        {
            return value;
        }

        [ScriptableMethod("toBool")]
        public static object ToBool(this string value)
        {
            switch (value)
            {
                case "true":
                    return true;
                case "false":
                    return false;
            }

            throw ExceptionHelper.InvalidConvert(typeof(string), typeof(bool), value);
        }

        [ScriptableMethod("toInt")]
        public static object ToInt(this string value)
        {
            if (!int.TryParse(value, out int parsedValue))
                throw ExceptionHelper.InvalidConvert(typeof(string), typeof(int), value);

            return parsedValue;
        }

        [ScriptableMethod("toFloat")]
        public static object ToFloat(this string value)
        {
            if (!float.TryParse(value, out float parsedValue))
                throw ExceptionHelper.InvalidConvert(typeof(string), typeof(float), value);

            return parsedValue;
        }
    }
}
