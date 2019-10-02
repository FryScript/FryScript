using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using FryScript.Helpers;

namespace FryScript
{
    public class MetaScriptArray : MetaScriptObject
    {
        public MetaScriptArray(Expression expression, BindingRestrictions restrictions, object value) : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            var ctorInfo = typeof(ScriptArray)
                .GetTypeInfo()
                .GetConstructor(new Type[]{typeof(int)});
            
            var capacityExpr = ExpressionHelper.DynamicConvert(args[0].Expression, typeof(int));
            var createExpr = Expression.New(ctorInfo, new[]{capacityExpr});
            var convertExpr = ExpressionHelper.DynamicConvert(createExpr, typeof(object));

            var restrictions = BindingRestrictions
                .GetTypeRestriction(Expression, LimitType)
                .Merge(BindingRestrictions.GetTypeRestriction(args[0].Expression, args[0].LimitType));

            return new DynamicMetaObject(convertExpr, restrictions);
        }
    }
}