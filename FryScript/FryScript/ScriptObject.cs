using FryScript.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    [ScriptableType("object")]
    public class ScriptObject : IEnumerable<object>, IScriptObject
    {
        private readonly object _lock = new object();
        
        public ObjectCore ObjectCore { get; } = new ObjectCore();

        public object this[string name]
        {
            get { return ScriptObjectExtensions.GetIndex(this, name); }
            set { ScriptObjectExtensions.SetIndex(this, name, value); }
        }

        public ScriptObject()
        {
            ObjectCore.Builder = Builder.ScriptObjectBuilder;
        }

        public virtual DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return this.GetMetaScriptObject(parameter);
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return this.GetMembers().GetEnumerator();
        }

        public override string ToString()
        {
            return ScriptTypeHelper.GetScriptType(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
