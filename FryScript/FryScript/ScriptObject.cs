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

    public class ScriptObject : IScriptType, IDynamicMetaObjectProvider, IEnumerable<object>
    {
        private readonly object _lock = new object();
        public const string ObjectName = "[object]";

        internal string ScriptType;
        internal HashSet<string> Extends;
        internal MemberIndex MemberIndex = MemberIndex.Root;
        internal object[] MemberData;
        internal Func<ScriptObject, object> Ctor;
        internal object Target;

        internal Type TargetType { get { return Target.GetType(); } }

        internal bool HasTarget { get { return Target != null; } }

        public object this[string name]
        {
            get { return GetIndex(name); }
            set { SetIndex(name, value); }
        }

        public ScriptObject()
            : this(scriptType: ObjectName)
        {
        }

        internal ScriptObject(
            object target = null, 
            string scriptType = null,
            Func<ScriptObject, object> ctor = null,
            HashSet<string> extends = null, 
            bool autoConstruct = true)
        {
            SetTarget(target);
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

        internal object SetTarget(object target)
        {
            if (Target == null && target != null)
            {
                Target = target;

                var scriptable = target as IScriptable;
                if (scriptable != null)
                    scriptable.Script = this;
            }

            return this;
        }

        internal object Extend(ScriptObject scriptObject)
        {
            if(scriptObject.Ctor != null)
                scriptObject.Ctor(this);

            return this;
        }

        internal ScriptObject CreateInstance()
        {
            return new ScriptObject(
                scriptType: ScriptType,
                ctor: Ctor,
                extends: Extends
                );
        }

        internal object SetMember(int index,  object value)
        {
            lock (_lock)
            {
                if (MemberData == null)
                    MemberData = new object[16];

                if(MemberData.Length <= index)
                    while (MemberData.Length <= index)
                        Array.Resize(ref MemberData, MemberData.Length + 16);

                return MemberData[index] = value;
            }
        }

        internal object SetIndex(string name, object value)
        {
            lock (_lock)
            {
                return CallSiteCache.Current.SetMember(name, this, value);
            }
        }

        internal object GetMember(int index)
        {
            lock (_lock)
            {
                return MemberData[index];
            }
        }

        internal object GetIndex(string name)
        {
            lock (_lock)
            {
                return CallSiteCache.Current.GetMember(name, this);
            }
        }

        internal bool IsValidSetMember(MemberIndex memberIndex)
        {
            lock(_lock)
            {
                var curIndex = MemberIndex;
                if (curIndex == memberIndex)
                    return true;

                if (curIndex.CurrentHash == memberIndex.PreviousHash)
                {
                    MemberIndex = memberIndex;
                    return true;
                }

                return false;
            }
        }

        internal bool IsValidGetMember(MemberIndex memberIndex)
        {
            lock(_lock)
            {
                return MemberIndex == memberIndex;
            }
        }

        internal bool HasMemberOfType(string name, Type type)
        {
            lock (_lock)
            {
                if (!CallSiteCache.Current.HasMember(name, this))
                    return false;

                var value = CallSiteCache.Current.GetMember(name, this);

                return value != null && value.GetType() == type;
            }
        }

        internal bool TryGetMemberOfType<T>(string name, out T value)
        {
            lock(_lock)
            {
                value = default(T);

                if (!CallSiteCache.Current.HasMember(name, this))
                    return false;

                var rawValue = CallSiteCache.Current.GetMember(name, this);

                if (!(rawValue is T))
                    return false;

                value = (T)rawValue;

                return true;
            }
        }

        public string GetScriptType()
        {
            return ScriptType;
        }

        public IEnumerable<string> GetMembers()
        {
            var members = TypeProvider.Current.GetMemberNames(GetType());

            if (HasTarget)
            {
                members = members.Union(TypeProvider.Current.GetMemberNames(TargetType));
            }

            members = members.Union(MemberIndex.Keys);

            return members;
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

        public static object[] GetMemberData(ScriptObject scriptObject)
        {
            if (scriptObject == null)
                throw new ArgumentNullException("scriptObject");

            return scriptObject.MemberData;
        }

        public static object GetTarget(ScriptObject scriptObject)
        {
            if (scriptObject == null) 
                throw new ArgumentNullException("scriptObject");

            return scriptObject.Target;
        }

        public static void SetTarget(ScriptObject scriptObject, object target)
        {
            if (scriptObject == null) 
                throw new ArgumentNullException("scriptObject");

            if (target == null) 
                throw new ArgumentNullException("target");

            scriptObject.Target = target;
        }

        public static MemberIndex GetMemberIndex(ScriptObject scriptObject)
        {
            if (scriptObject == null) 
                throw new ArgumentNullException("scriptObject");

            return scriptObject.MemberIndex;
        }
    }
}
