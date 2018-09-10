using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    using Helpers;

    public partial class ScriptFunction : ScriptObject
    {
        public const string DefaultScriptType = "[function]";

        internal Delegate TargetDelegate { get { return Target as Delegate; } }

        public ScriptFunction(Delegate target)
            : base(target: target, scriptType: DefaultScriptType)
        {
        }

        internal ScriptFunction(Delegate target, string scriptType)
            : base(target: target, scriptType: scriptType)
        {
        }

        public void Invoke(params object[] args)
        {
            (this as dynamic)(new ScriptParams(args));
        }

        public T Invoke<T>(params object[] args)
        {
            return (this as dynamic)(new ScriptParams(args));
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
           return new MetaScriptFunction(parameter, BindingRestrictions.Empty, this);
        }

        internal static ScriptFunction Extend(object obj)
        {
            var func = obj as ScriptFunction;
            if (func == null)
                ExceptionHelper.InvalidExtendTarget();

            return func;
        }
    }
}
