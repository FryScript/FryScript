using FryScript.Binders;
using FryScript.Extensions;
using FryScript.Helpers;
using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.HostInterop
{
    public sealed class MetaPrimitive : MetaScriptObjectBase
    {
        public Expression TypedExpression { get { return Expression.Convert(Expression, LimitType); } }

        public MetaPrimitive(DynamicMetaObject source)
            : base(source.Expression, source.Restrictions, source.Value)
        {
        }

        public MetaPrimitive(Expression expression, object value)
            : base(expression, BindingRestrictions.Empty, value)
        {
            
        }

        public override DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            return BindHelper.BindExtendsOperation(binder, this, value);
        }
    }
}
