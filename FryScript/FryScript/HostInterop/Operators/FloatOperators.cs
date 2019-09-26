namespace FryScript.HostInterop.Operators
{
    public static class FloatOperators
    {
        public const string FloatName = "[float]";

        [ScriptableTypeOperation(ScriptableTypeOperator.TypeOf)]
        public static object GetScriptType(this float value)
        {
            return FloatName;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Default)]
        public static object Default(this float value)
        {
            return default(float);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Ctor)]
        public static object Ctor(this float value)
        {
            return value;
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this float value)
        {
            return value != 0;
        }
    }
}
