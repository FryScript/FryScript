using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    using System.Collections;
    using System.Linq;
    using CallSites;
    using HostInterop;

    [ScriptableType("[object]")]
    public class ScriptObject : IScriptType, IDynamicMetaObjectProvider, IEnumerable<object>, IScriptObject
    {
        private readonly object _lock = new object();
        public const string ObjectName = "[object]";

        internal string ScriptType;
        internal HashSet<string> Extends;

        internal Func<ScriptObject, object> Ctor;

        public ObjectCore ObjectCore { get; } = new ObjectCore();

        public object this[string name]
        {
            get { return ScriptObjectExtensions.GetIndex(this, name); }
            set { ScriptObjectExtensions.SetIndex(this, name, value); }
        }

        public ScriptObject()
            : this(scriptType: ObjectName)
        {
        }

        public ScriptObject(
            string scriptType = null,
            Func<ScriptObject, object> ctor = null,
            HashSet<string> extends = null, 
            bool autoConstruct = true)
        {
            ScriptType = scriptType ?? ObjectName;
            Ctor = ctor;
            Extends = extends;

            if (Ctor != null && autoConstruct)
                Ctor(this);
        }
         
        public bool IsScript(ScriptObject scriptObject)
        {
            if (scriptObject.ScriptType == ObjectName)
                return true;

            if (Extends == null)
                return false;

            if (ScriptType == scriptObject.ScriptType)
                return true;

            return Extends.Contains(scriptObject.ScriptType);
        }

        public bool IsScriptType(string scriptType)
        {
            scriptType = ScriptTypeHelper.NormalizeTypeName(scriptType);
            var currentType = ScriptTypeHelper.NormalizeTypeName(ScriptType);

            return scriptType == currentType;
        }

        public bool ExtendsScriptType(string scriptType)
        {
            scriptType = ScriptTypeHelper.NormalizeTypeName(scriptType);

            if (scriptType == ObjectName)
                return true;

            if (IsScriptType(scriptType))
                return true;

            if (Extends == null)
                return false;

            return Extends.Contains(scriptType);
        }

        //internal bool TryGetMemberOfType<T>(string name, out T value)
        //{
        //    lock(_lock)
        //    {
        //        value = default(T);

        //        if (!CallSiteCache.Current.HasMember(name, this))
        //            return false;

        //        var rawValue = CallSiteCache.Current.GetMember(name, this);

        //        if (!(rawValue is T))
        //            return false;

        //        value = (T)rawValue;

        //        return true;
        //    }
        //}

        public string GetScriptType()
        {
            return ScriptType;
        }

        public IEnumerable<string> GetMembers()
        {
            return ScriptObjectExtensions.GetMembers(this);
        }

        public bool HasMember(string name)
        {
            lock(_lock)
            {
                return CallSiteCache.Current.HasMember(name, this);
            }
        }

        public virtual DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptObject(parameter, BindingRestrictions.Empty, this);
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return GetMembers().GetEnumerator();
        }

        public override string ToString()
        {
            return GetScriptType();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
