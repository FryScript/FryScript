using FryScript.HostInterop;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{

    class ScriptBinaryOperationBinder : BinaryOperationBinder
    {
        public ScriptBinaryOperationBinder(ExpressionType operation)
            : base(operation)
        {
        }

        public override DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
        {
            var left = target;
            var right = arg;
           
            var metaObject = TypeProvider.Current.GetMetaObject(left.Expression, left.Value);
            return metaObject.BindBinaryOperation(this, right);
        }
    }
}
