namespace FryScript.HostInterop.Operators
{
    public static class Int32Operators
    {
        public const string IntName = "[int]";

        [ScriptableTypeOperation(ScriptableTypeOperator.TypeOf)]
        public static object GetScriptType(this int value)
        {
            return IntName;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Default)]
        public static object Default(this int value)
        {
            return default(int);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Ctor)]
        public static object Ctor(this int value)
        {
            return value;
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this int value)
        {
            return value != 0;
        }
    }
}
