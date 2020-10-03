using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public sealed class ScriptObjectReference : IDynamicMetaObjectProvider
    {
        readonly private Lazy<IScriptObject> _resolver;

        public IScriptObject Target
        {
            get { return _resolver.Value; }
        }

        public ScriptObjectReference(Func<IScriptObject> func)
        {
            _resolver = new Lazy<IScriptObject>(func, false);
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptObjectReference(parameter, BindingRestrictions.Empty, this);
        }
    }
}
