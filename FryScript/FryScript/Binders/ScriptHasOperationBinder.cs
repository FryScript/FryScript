using FryScript.Helpers;
using System;
using System.Dynamic;

namespace FryScript.Binders
{

    public class ScriptHasOperationBinder : DynamicMetaObjectBinder
    {
        private readonly string _name;

        public string Name { get { return _name; } }

        public ScriptHasOperationBinder(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }

        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            if (target is MetaScriptObjectBase metaScriptObjectBase)
                return metaScriptObjectBase.BindHasOperation(this);
            
            return FallbackHas(target);
        }

        public DynamicMetaObject FallbackHas(DynamicMetaObject target)
        {
            return BindHelper.BindHasOperation(this, target);
        }
    }
}
