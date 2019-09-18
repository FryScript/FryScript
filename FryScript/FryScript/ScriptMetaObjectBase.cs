namespace FryScript
{
    using System.Dynamic;
    using System.Linq.Expressions;
    using Binders;
    using System;

    public abstract class ScriptMetaObjectBase : DynamicMetaObject,
        IBindExtendsOperationProvider,
        IBindHasOperationProvider,
        IBindIsOperationProvider
    {
        protected ScriptMetaObjectBase(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public virtual DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            value = value ?? throw new ArgumentNullException(nameof(value));

            return binder.FallbackExtends(this, value);
        }

        public virtual DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));

            return binder.FallbackHas(this);
        }

        public virtual DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));

            return binder.FallbackIs(this, value);
        }
    }
}
