using System;
using System.Dynamic;
using System.Linq.Expressions;
using FryScript.Helpers;

namespace FryScript
{
    [ScriptableType("function")]
    public partial class ScriptFunction : ScriptObject
    {
        public Delegate TargetDelegate { get; set; }

        public ScriptFunction()
            : this(new Action(() => { }))
        {

        }

        public ScriptFunction(Delegate target)
        {
            TargetDelegate = target ?? throw new ArgumentNullException(nameof(target));

            ObjectCore.Builder = Builder.ScriptFunctionBuilder;
        }

        internal ScriptFunction(Delegate target, string scriptType)
            : base(scriptType: scriptType)
        {
            TargetDelegate = target ?? throw new ArgumentNullException(nameof(target));
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
