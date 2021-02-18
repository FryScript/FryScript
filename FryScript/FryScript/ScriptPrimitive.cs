using System.Dynamic;
using System.Linq.Expressions;
using FryScript.Helpers;
using FryScript.Extensions;
using System;

namespace FryScript
{
    public class ScriptPrimitive<T> : ScriptObject
    {
        public T Target { get; set; }

        public Type TargetType => typeof(T);

        public string ScriptType { get; }

        public ScriptPrimitive()
        {
            Target = (T)typeof(T).GetDefaultValue();
            ScriptType = ScriptTypeHelper.GetScriptType(Target);
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptPrimitive<T>(parameter, BindingRestrictions.Empty, this);
        }

        public static implicit operator T(ScriptPrimitive<T> scriptPrimitive)
        {
            return (T)scriptPrimitive.Target;
        }
    }
}
