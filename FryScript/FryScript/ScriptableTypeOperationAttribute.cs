namespace FryScript
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptableTypeOperationAttribute : ScriptableBaseAttribute
    {
        private const string OperatorName = "Operator";

        private readonly ScriptableTypeOperator _operation;

        public ScriptableTypeOperator Operation
        {
            get { return _operation; }
        }

        public ScriptableTypeOperationAttribute(ScriptableTypeOperator operation)
            : base(OperatorName)
        {
            _operation = operation;
        }
    }
}