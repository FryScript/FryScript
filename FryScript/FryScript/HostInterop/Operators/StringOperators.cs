namespace FryScript.HostInterop.Operators
{

    public static class StringOperators
    {
        public const string StringName = "[string]";

        [ScriptableBinaryOperation(ScriptableBinaryOperater.Add)]
        public static object Add(this string value1, string value2)
        {
            return string.Concat(value1, value2);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.TypeOf)]
        public static object GetScriptType(this string value)
        {
            return StringName;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Default)]
        public static object Default(this string value)
        {
            return string.Empty;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Ctor)]
        public static object Ctor(this string value)
        {
            return string.Empty;
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this string value)
        {
            return value != string.Empty;
        }
    }
}
