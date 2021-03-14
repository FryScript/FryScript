namespace FryScript.HostInterop.Operators
{
    public static class ScriptFibreContextOperators
    {
        [ScriptableTypeOperation(ScriptableTypeOperator.TypeOf)]
        public static object GetScriptType(this ScriptFibreContext value)
        {
            return "[fibre-context]";
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Default)]
        public static object Default(this ScriptFibreContext value)
        {
            return null;
        }

        [ScriptableTypeOperation(ScriptableTypeOperator.Ctor)]
        public static object Ctor(this ScriptFibreContext value)
        {
            return null;
        }
    }
}
