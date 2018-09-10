using System;

namespace FryScript
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq.Expressions;

    public sealed class ScriptObjectReference : IDynamicMetaObjectProvider, IScriptType
    {
        private Lazy<ScriptObject> _resolver;

        public ScriptObject Target
        {
            get { return _resolver.Value; }
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptObjectReference(parameter, BindingRestrictions.Empty, this);
        }

        public void SetResolver(Func<ScriptObject> func)
        {
            if (func == null) 
                throw new ArgumentNullException("func");

            _resolver = new Lazy<ScriptObject>(func);
        }

        public string GetScriptType()
        {
            return Target.GetScriptType();
        }

        public bool IsScriptType(string scriptType)
        {
            return Target.IsScriptType(scriptType);
        }

        public bool ExtendsScriptType(string scriptType)
        {
            return Target.ExtendsScriptType(scriptType);
        }

        public bool HasMember(string name)
        {
            return Target.HasMember(name);
        }

        public IEnumerable<string> GetMembers()
        {
            return Target.GetMembers();
        }
    }
}
