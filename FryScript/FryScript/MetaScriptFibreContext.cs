using FryScript.Binders;
using System.Dynamic;
using System.Linq.Expressions;
using System;
using System.Reflection;
using System.Linq;

namespace FryScript
{
    public class MetaScriptFibreContext : MetaScriptObject
    {
        private static readonly MethodInfo ResumeMethod = typeof(ScriptFibreContext).GetTypeInfo().DeclaredMethods.Single(m => m.Name == nameof(ScriptFibreContext.Resume));

        public Expression ScriptFibreContextExpr => Expression.Convert(Expression, typeof(ScriptFibreContext));

        public MetaScriptFibreContext(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        //public override DynamicMetaObject BindResumeOperation(ScriptResumeOperationBinder binder)
        //{
        //    var invokeExpr = Expression.Call(ScriptFibreContextExpr, ResumeMethod);
        //    var restrictions = BindingRestrictions.GetTypeRestriction(Expression, typeof(ScriptFibreContext));

        //    return new DynamicMetaObject(invokeExpr, restrictions);
        //}
    }
}
