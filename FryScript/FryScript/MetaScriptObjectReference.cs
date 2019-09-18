using FryScript.Binders;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class MetaScriptObjectReference : MetaScriptObjectBase
    {
        public ScriptObjectReference Reference { get { return (ScriptObjectReference) Value; } }

        public Expression ReferenceExpr { get { return Expression.Convert(Expression, typeof (ScriptObjectReference)); } }

        public Expression TargetExpr { get { return Expression.Property(ReferenceExpr, "Target"); } }

        public MetaScriptObjectReference(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindGetMember(binder);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindSetMember(binder, value);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindGetIndex(binder, indexes);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindSetIndex(binder, indexes, value);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindInvokeMember(binder, args);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindBinaryOperation(binder, arg);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindConvert(binder);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr).BindCreateInstance(binder, args);
            return GetReferenceMetaObject(metaObject);
        }

        public override DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr);

            var isOperationProvider = metaObject as IBindIsOperationProvider;

            return GetReferenceMetaObject(isOperationProvider.BindIsOperation(binder, value));
        }

        public override DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr);

            var extendsOperationProvider = metaObject as IBindExtendsOperationProvider;

            return GetReferenceMetaObject(extendsOperationProvider.BindExtendsOperation(binder, value));
        }

        public override DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder)
        {
            var metaObject = Reference.Target.GetMetaObject(TargetExpr);

            var hasOperationProvider = metaObject as IBindHasOperationProvider;

            return GetReferenceMetaObject(hasOperationProvider.BindHasOperation(binder));
        }

        private DynamicMetaObject GetReferenceMetaObject(DynamicMetaObject metaObject)
        {
            var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType)
                .Merge(metaObject.Restrictions);

            return new DynamicMetaObject(metaObject.Expression, restrictions);
        }
    }
}
