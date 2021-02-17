using System;

namespace FryScript.HostInterop.Operators
{
    public static class Int32Operators
    {
        public const string IntName = "int32";

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
            return Default(value);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Invoke)]
        public static object Invoke(this int value, object arg)
        {
            return Convert.ChangeType(arg, typeof(int));
        }

        [ScriptableConvertOperation]
        public static bool Boolean(this int value)
        {
            return value != 0;
        }
    }
}
