using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    [ScriptableType("fibre")]
    public class ScriptFibre : ScriptFunction
    {
        public ScriptFibre()
            : base(new Action(() => {}))
        {
            ObjectCore.Builder = Builder.ScriptFibreBuilder;
        }

        public ScriptFibre(Delegate target)
            : base(target)
        {
            ObjectCore.Builder = Builder.ScriptFibreBuilder;
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


    //public abstract class PseudoFibre : ScriptFibre
    //{
    //    protected enum YieldType
    //    { 
    //        Yield,
    //        YieldReturn
    //    }

    //    protected struct YieldState
    //    {
    //        public YieldType YieldType;

    //        public object Value;

    //        public YieldState(YieldType yieldType, object value = null)
    //        {
    //            YieldType = yieldType;
    //            Value = value;
    //        }
    //    }

    //    public PseudoFibre()
    //        : base(null)
    //    {
    //        Target = new Func<ScriptParams, ScriptFibreContext>(Invoke);
    //    }

    //    private ScriptFibreContext Invoke(ScriptParams args)
    //    {
    //        var state = Fibre(args).GetEnumerator();

    //        var context = new ScriptFibreContext(c =>
    //        {
    //            c.YieldState = 0;

    //            if (!state.MoveNext())
    //                ExceptionHelper.FibreContextCompleted();

    //            if (state.Current.YieldType == YieldType.YieldReturn)
    //                c.YieldState = ScriptFibreContext.CompletedState;

    //            return state.Current.Value;
    //        });

    //        return context;
    //    }

    //    protected YieldState Pause()
    //    {
    //        return new YieldState(YieldType.Yield, ScriptFibreContext.NoResult);
    //    }

    //    protected YieldState Pause(object value)
    //    {
    //        return new YieldState(YieldType.Yield, value);
    //    }

    //    protected YieldState Complete()
    //    {
    //        return new YieldState(YieldType.YieldReturn, ScriptFibreContext.NoResult);
    //    }

    //    protected YieldState Complete(object value)
    //    {
    //        return new YieldState(YieldType.YieldReturn, value);
    //    }

    //    protected abstract IEnumerable<YieldState> Fibre(ScriptParams args);
    //}

    //public abstract class NativeFibre : ScriptFibre
    //{
    //    protected enum YieldState
    //    {
    //        Yield,
    //        YieldReturn
    //    }

    //    protected struct Yield
    //    {
    //        public readonly YieldState YieldState;
    //        public readonly object Value;

    //        internal Yield(YieldState yieldState, object value = null)
    //        {
    //            YieldState = yieldState;
    //            Value = value ?? ScriptFibreContext.NoResult;
    //        }
    //    }

    //    public NativeFibre() : base(null)
    //    {
    //        Target = new Func<ScriptParams, ScriptFibreFrame>(Invoke);
    //    }

    //    protected Yield Empty()
    //    {
    //        return new Yield(YieldState.Yield);
    //    }

    //    protected Yield EmptyReturn()
    //    {
    //        return new Yield(YieldState.YieldReturn);
    //    }

    //    protected Yield Value(object value)
    //    {
    //        return new Yield(YieldState.Yield, value);
    //    }

    //    protected Yield ValueReturn(object value)
    //    {
    //        return new Yield(YieldState.YieldReturn, value);
    //    }

    //    private ScriptFibreFrame Invoke(ScriptParams args)
    //    {
    //        IEnumerator<Yield> enumerator = null;
            
    //        var frame = new ScriptFibreFrame();
    //        frame.Init(c =>
    //        {
    //            enumerator = enumerator ?? Fibre(c, args);

    //            frame.YieldState = 0;

    //            if (!enumerator.MoveNext())
    //            {
    //                frame.YieldState = ScriptFibreContext.CompletedState;
    //                return enumerator.Current;
    //            }

    //            var yield = enumerator.Current;

    //            if (yield.YieldState == YieldState.YieldReturn)
    //                frame.YieldState = ScriptFibreContext.CompletedState;

    //            return yield.Value;
    //        });

    //        return frame;
    //    }

    //    protected abstract IEnumerator<Yield> Fibre(ScriptFibreContext context, ScriptParams args);
    //}

    //public class AfterFibre : NativeFibre
    //{
    //    protected override IEnumerator<Yield> Fibre(ScriptFibreContext context, ScriptParams args)
    //    {
    //        var length = args.Get<int>(0);
    //        var func = args.Get<ScriptFunction>(1);

    //        var endTime = DateTime.UtcNow.AddSeconds(length);

    //        while(DateTime.UtcNow < endTime)
    //        {
    //            yield return Empty(); 
    //        }

    //        yield return ValueReturn(func.Invoke<object>());
    //    }
    //}
}
