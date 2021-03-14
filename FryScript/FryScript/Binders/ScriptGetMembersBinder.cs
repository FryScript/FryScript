using FryScript.Helpers;
using System.Dynamic;

namespace FryScript.Binders
{
    public class ScriptGetMembersBinder : DynamicMetaObjectBinder
    {
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            if (target is MetaScriptObjectBase metaScriptObjectBase)
                return metaScriptObjectBase.BindGetMembers(this);

            return FallbackGetMembers(target);
        }

        public DynamicMetaObject FallbackGetMembers(DynamicMetaObject target)
        {
            return BindHelper.BindGetMembers(target);
        }
    }
}
