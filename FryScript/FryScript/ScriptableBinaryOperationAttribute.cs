namespace FryScript
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptableBinaryOperationAttribute : ScriptableBaseAttribute
    {
        private const string OperatorName = "Operator";

        private readonly ScriptableBinaryOperater _operation;

        public ScriptableBinaryOperater Operation
        {
            get { return _operation; }
        }

        public ScriptableBinaryOperationAttribute(ScriptableBinaryOperater operation)
            : base(OperatorName)
        {
            _operation = operation;
        }
    }
}
