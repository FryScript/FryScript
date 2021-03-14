using FryScript.Helpers;
using FryScript.HostInterop;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript
{

    public class MetaScriptPrimitive<T> : DynamicMetaObject
    {
        private ScriptPrimitive<T> ScriptObject { get { return Value as ScriptPrimitive<T>; } }

        private Expression ScriptObjectExpr
        {
            get { return Expression.Convert(Expression, typeof(ScriptPrimitive<T>)); }
        }

        private Expression ScriptObjectTargetExpr { get { return Expression.Convert(Expression.PropertyOrField(ScriptObjectExpr, "Target"), typeof(T)); } }

        public MetaScriptPrimitive(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            return GetMetaObject(ExpressionHelper.DynamicGetMember(binder.Name, ScriptObjectTargetExpr));
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            return GetMetaObject(ExpressionHelper.DynamicSetMember(binder.Name, ScriptObjectTargetExpr, value.Expression));
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            return
                GetMetaObject(ExpressionHelper.DynamicInvokeMember(ScriptObjectTargetExpr, binder.Name,
                    args.Select(a => a.Expression).ToArray()));
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return GetMetaObject(ExpressionHelper.DynamicBinaryOperation(ScriptObjectTargetExpr, binder.Operation, arg.Expression));
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            if (!TypeProvider.Current.TryGetTypeOperator(ScriptObject.TargetType, ScriptableTypeOperator.Ctor, out MethodInfo ctorInfo))
                throw new FryScriptException(string.Format("Type {0} does not have a scriptable constructor defined", typeof(T).FullName));

            var parameterTypes = ctorInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            var argExprs = parameterTypes.Select(
               (t, i) => i == 0
                   ? ScriptObjectTargetExpr
                   : i > 0 && i < args.Length
                       ? ExpressionHelper.DynamicConvert(args[i].Expression, t)
                       : Expression.Default(t)
               ).ToArray();

            var invokeExpr = Expression.Call(ctorInfo, argExprs);

            return GetMetaObject(invokeExpr);
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            return GetMetaObject(ExpressionHelper.DynamicConvert(ScriptObjectTargetExpr, binder.Type));
        }

        private DynamicMetaObject GetMetaObject(Expression expr)
        {
            return new DynamicMetaObject(expr,
                BindingRestrictions.GetTypeRestriction(Expression, LimitType));
        }
    }
}