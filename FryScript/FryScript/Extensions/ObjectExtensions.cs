namespace FryScript.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToDisplayString(this object value)
        {
            value = value ?? string.Empty;

            var valueStr = value is string
                ? string.Format("\"{0}\"", value)
                : value.ToString();

            return valueStr;
        }
    }
}
