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
