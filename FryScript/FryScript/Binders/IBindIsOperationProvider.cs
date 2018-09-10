using System.Dynamic;

namespace FryScript.Binders
{
    public interface IBindIsOperationProvider
    {
        DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject value);
    }
}
