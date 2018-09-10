using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptableConvertOperationAttribute : ScriptableBaseAttribute
    {
        private const string OperatorName = "Operator";

        public ScriptableConvertOperationAttribute()
            : base(OperatorName)
        {
        }
    }
}
