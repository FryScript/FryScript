using System.Dynamic;

namespace FryScript.Binders
{

    public interface IBindExtendsOperationProvider
    {
        DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject value);
    }
}
