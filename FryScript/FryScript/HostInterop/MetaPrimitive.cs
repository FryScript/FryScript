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
    public sealed class MetaPrimitive : ScriptMetaObjectBase
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

        // public override DynamicMetaObject BindConvert(ConvertBinder binder)
        // {
        //     //binder = binder ?? throw new ArgumentNullException(nameof(binder));

        //     // if (binder.Type == typeof(object))
        //     // {
        //     //     var expr = Expression.Convert(Expression, typeof(object));
        //     //     var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //     return new DynamicMetaObject(expr, restrictions);
        //     // }

        //     // if(LimitType == binder.Type)
        //     // {
        //     //     var expr = Expression.Convert(Expression, LimitType);
        //     //     var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //     return new DynamicMetaObject(expr, restrictions);
        //     // }

        //     // if (LimitType.TryCanConvert(binder.Type, out MethodInfo convertMethod))
        //     // {
        //     //     var expr = Expression.Convert(Expression, LimitType, convertMethod);
        //     //     var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //     return new DynamicMetaObject(expr, restrictions);
        //     // }

        //     // if (TypeProvider.Current.TryGetConvertOperator(LimitType, binder.Type, out convertMethod))
        //     // {
        //     //     var callExpr = Expression.Call(
        //     //         convertMethod,
        //     //         Expression.Convert(Expression, LimitType));

        //     //     var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //     return new DynamicMetaObject(callExpr, restrictions);
        //     // }

        //     // if(TypeProvider.Current.IsNumericType(LimitType) && TypeProvider.Current.IsNumericType(binder.Type))
        //     // {
        //     //     var convertExpr = Expression.Convert(Expression, LimitType);
        //     //     convertExpr = Expression.Convert(convertExpr, binder.Type);

        //     //     var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //     return new DynamicMetaObject(convertExpr, restrictions);
        //     // }

        //     // if(binder.Type.IsNullable())
        //     // {
        //     //     var unwrappedNullable = binder.Type.UnwrapNullable();

        //     //     if (TypeProvider.Current.TryGetConvertOperator(LimitType, unwrappedNullable, out MethodInfo convertMethod))
        //     //     {
        //     //         Expression callExpr = Expression.Call(
        //     //             convertMethod,
        //     //             Expression.Convert(Expression, LimitType));

        //     //         callExpr = Expression.Convert(callExpr, binder.Type);

        //     //         var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        //     //         return new DynamicMetaObject(callExpr, restrictions);
        //     //     }

                

        //     //     return new DynamicMetaObject(Expression.Convert(Expression, binder.Type),
        //     //         BindingRestrictions.GetTypeRestriction(Expression, LimitType)
        //     //         );
        //     // }

        //     //throw ExceptionHelper.InvalidConvert(LimitType, binder.Type);
        // }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return BindHelper.BindBinaryOperation(
                binder ?? throw new ArgumentNullException(nameof(binder)), 
                this, 
                arg ?? throw new ArgumentNullException(nameof(arg)));
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));

            if(Value == null)
                ExceptionHelper.MemberUndefined(binder.Name);

            var restrictions = RestrictionsHelper.TypeOrNullRestriction(this);
            if (ExpressionHelper.TryGetMethodExpression(Expression.Convert(Expression, LimitType), binder.Name, out Expression expression))
                return new DynamicMetaObject(expression, restrictions);

            return new DynamicMetaObject(ExpressionHelper.Null(), restrictions);

            //throw ExceptionHelper.MemberUndefined(binder.Name);
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            throw ExceptionHelper.NonSetMember(LimitType);
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            args = args ?? throw new ArgumentNullException(nameof(args));


            var getBinder = BinderCache.Current.GetMemberBinder(binder.Name);
            var metaobject = BindGetMember(getBinder);
            var invokeExpr = ExpressionHelper.DynamicInvoke(metaobject.Expression, args.Select(a => a.Expression).ToArray());

            return new DynamicMetaObject(invokeExpr, metaobject.Restrictions);
        }

        public override DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value)
        {
            return BindHelper.BindIsOperation(binder, this, value);
        }

        public override DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            return BindHelper.BindExtendsOperation(binder, this, value);
        }

        public override DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));

            var hasMember = TypeProvider.Current.HasMember(LimitType, binder.Name);

            return new DynamicMetaObject(
                    Expression.Constant(hasMember, typeof(object)),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType)
                );
        }
    }
}
