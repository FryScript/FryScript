using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public sealed class ScriptImport : IEnumerable<object>, IScriptObject
    {
        readonly private Lazy<IScriptObject> _resolver;

        public IScriptObject Target
        {
            get { return _resolver.Value; }
        }

        public ObjectCore ObjectCore => Target.ObjectCore;

        public ScriptImport(Func<IScriptObject> func)
        {
            _resolver = new Lazy<IScriptObject>(func, false);
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptImport(parameter, BindingRestrictions.Empty, this);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return Target.GetMembers().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
