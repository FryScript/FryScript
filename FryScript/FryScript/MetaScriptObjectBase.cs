using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{

    public abstract class MetaScriptObjectBase : DynamicMetaObject
    {
        protected MetaScriptObjectBase(Expression expression, BindingRestrictions restrictions, object value)
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

        public virtual DynamicMetaObject BindGetMembers(ScriptGetMembersBinder binder)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));

            return binder.FallbackGetMembers(this);
        }
    }
}
