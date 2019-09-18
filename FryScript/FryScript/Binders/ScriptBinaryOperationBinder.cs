using FryScript.Helpers;
using FryScript.HostInterop;
using System;
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
            return BindHelper.BindBinaryOperation(
                this , 
                target ?? throw new ArgumentNullException(nameof(target)), 
                arg ?? throw new ArgumentNullException(nameof(arg)));
        }
    }
}
