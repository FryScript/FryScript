using System.Dynamic;

namespace FryScript.Binders
{
    public interface IBindHasOperationProvider
    {
        DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder);
    }
}
