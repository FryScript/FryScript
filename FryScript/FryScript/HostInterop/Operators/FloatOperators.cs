using System;

namespace FryScript.HostInterop.Operators
{
    public static class FloatOperators
    {
        public const string FloatName = "single";

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
            return Default(value);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Invoke)]
        public static object Invoke(this float value, object arg)
        {
            return Convert.ChangeType(arg, typeof(float));
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this float value)
        {
            return value != 0;
        }
    }
}
