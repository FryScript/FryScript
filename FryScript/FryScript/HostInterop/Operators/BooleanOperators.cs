using System;

namespace FryScript.HostInterop.Operators
{
    public static class BooleanOperators
    {
        public const string BooleanName = "[bool]";

        [ScriptableTypeOperation(ScriptableTypeOperator.TypeOf)]
        public static object GetScriptType(this bool value)
        {
            return BooleanName;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Default)]
        public static object Default(this bool value)
        {
            return default(bool);
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Ctor)]
        public static object Ctor(this bool value)
        {
            return value;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Invoke)]
        public static object Invoke(this bool value)
        {
            throw new NotImplementedException();
        }
    }
}
