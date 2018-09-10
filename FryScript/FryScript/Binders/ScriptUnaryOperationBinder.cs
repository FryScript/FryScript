using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{

    public class ScriptUnaryOperationBinder : UnaryOperationBinder
    {
        public ScriptUnaryOperationBinder(ExpressionType operationType)
            : base(operationType)
        {
        }

        public override DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
