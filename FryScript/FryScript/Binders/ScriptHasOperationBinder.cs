using FryScript.Helpers;
using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{

    public class ScriptHasOperationBinder : DynamicMetaObjectBinder
    {
        private readonly string _name;

        public string Name { get { return _name; } }

        public ScriptHasOperationBinder(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }

        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            var metaObject = DynamicMetaObjectHelper.GetDynamicMetaObject(target);

            var bindHasOperationProvider = metaObject as IBindHasOperationProvider;

            if (bindHasOperationProvider == null)
                return new DynamicMetaObject(
                    Expression.Constant(false, typeof (object)),
                    BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    );

            return bindHasOperationProvider.BindHasOperation(this);
        }

        public DynamicMetaObject FallbackHas(DynamicMetaObject target)
        {
            return BindHelper.BindHasOperation(this, target);
        }
    }
}
