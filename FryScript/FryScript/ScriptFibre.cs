using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class ScriptFibre : ScriptFunction
    {
        new public const string DefaultScriptType = "[fibre]";

        public ScriptFibre(Delegate target)
            : base(target, DefaultScriptType)
        {
        }

        internal static ScriptFibre New(Delegate target)
        {
            return new ScriptFibre(target);
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptFibre(parameter, BindingRestrictions.Empty, this);
        }

        public ScriptFibreContext Begin(params object[] args)
        {
            return TargetDelegate.DynamicInvoke(args) as ScriptFibreContext;
        }
    }

    //public class FunkyFibre : PseudoFibre
    //{
    //    protected override IEnumerable<YieldState> Fibre(ScriptParams args)
    //    {
    //        yield return Pause("Pause 1");
    //        yield return Pause("Pause 2");
    //        yield return Pause("Pause 3");
    //        yield return Pause("Pause 4");
    //    }
    //}


    public abstract class PseudoFibre : ScriptFibre
    {
        protected enum YieldType
        { 
            Yield,
            YieldReturn
        }

        protected struct YieldState
        {
            public YieldType YieldType;

            public object Value;

            public YieldState(YieldType yieldType, object value = null)
            {
                YieldType = yieldType;
                Value = value;
            }
        }

        protected PseudoFibre()
            : base(new Func<ScriptParams, ScriptFibreContext>(s => null))
        {
            TargetDelegate = new Func<ScriptParams, ScriptFibreContext>(Invoke);
        }

        private ScriptFibreContext Invoke(ScriptParams args)
        {
            var state = Fibre(args).GetEnumerator();

            var context = new ScriptFibreContext(c =>
            {
                c.YieldState = 0;

                if (!state.MoveNext())
                    ExceptionHelper.FibreContextCompleted();

                if (state.Current.YieldType == YieldType.YieldReturn)
                    c.YieldState = ScriptFibreContext.CompletedState;

                return state.Current.Value;
            });

            return context;
        }

        protected YieldState Pause()
        {
            return new YieldState(YieldType.Yield, ScriptFibreContext.NoResult);
        }

        protected YieldState Pause(object value)
        {
            return new YieldState(YieldType.Yield, value);
        }

        protected YieldState Complete()
        {
            return new YieldState(YieldType.YieldReturn, ScriptFibreContext.NoResult);
        }

        protected YieldState Complete(object value)
        {
            return new YieldState(YieldType.YieldReturn, value);
        }

        protected abstract IEnumerable<YieldState> Fibre(ScriptParams args);
    }
}
