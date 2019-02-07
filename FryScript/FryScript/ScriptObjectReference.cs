using System;

namespace FryScript
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq.Expressions;

    public sealed class ScriptObjectReference : IDynamicMetaObjectProvider, IScriptType
    {
        private Lazy<IScriptObject> _resolver;

        public IScriptObject Target
        {
            get { return _resolver.Value; }
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptObjectReference(parameter, BindingRestrictions.Empty, this);
        }

        public void SetResolver(Func<IScriptObject> func)
        {
            if (func == null) 
                throw new ArgumentNullException("func");

            _resolver = new Lazy<IScriptObject>(func);
        }

        public string GetScriptType()
        {
            //return Target.GetScriptType();
            throw new NotImplementedException();
        }

        public bool IsScriptType(string scriptType)
        {
            //return Target.IsScriptType(scriptType);
            throw new NotImplementedException();
        }

        public bool ExtendsScriptType(string scriptType)
        {
            //return Target.ExtendsScriptType(scriptType);
            throw new NotImplementedException();

        }

        public bool HasMember(string name)
        {
            //return Target.HasMember(name);
            throw new NotImplementedException();

        }

        public IEnumerable<string> GetMembers()
        {
            return Target.GetMembers();
        }
    }
}
