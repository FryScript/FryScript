namespace FryScript.HostInterop.Operators
{
    public static class FallbackOperators
    {
        [ScriptableBinaryOperation(ScriptableBinaryOperater.Equal)]
        public static object Equal(this object value1, object value2)
        {
            return value1 == value2;
        }

        [ScriptableBinaryOperation(ScriptableBinaryOperater.NotEqual)]
        public static object NotEqual(this object value1, object value2)
        {
            return value1 != value2;
        }

        [ScriptableBinaryOperation(ScriptableBinaryOperater.LessThan)]
        public static object LessThan(this object value1, object value2)
        {
            return false;
        }

        [ScriptableBinaryOperation(ScriptableBinaryOperater.LessThanOrEqual)]
        public static object LessThanOrEqual(this object value1, object value2)
        {
            return false;
        }

        [ScriptableBinaryOperation(ScriptableBinaryOperater.GreaterThan)]
        public static object GreaterThan(this object value1, object value2)
        {
            return false;
        }

        [ScriptableBinaryOperation(ScriptableBinaryOperater.GreaterThanOrEqual)]
        public static object GreaterThanOrEqual(this object value1, object value2)
        {
            return false;
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this object value)
        {
            return value != null;
        }
    }
}
