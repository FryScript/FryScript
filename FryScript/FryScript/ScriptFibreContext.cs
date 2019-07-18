using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class ScriptFibreContext : ScriptObject
    {
        public const int PendingState = -1;
        public const int CompletedState = -2;

        public static readonly object NoResult = new object();

        private readonly Stack<ScriptFibreContext> _stack = new Stack<ScriptFibreContext>();

        private object _result = NoResult;
        private int _yieldState = PendingState;

        public int YieldState
        {
            get { return _stack.Peek()._yieldState; }
            set { _stack.Peek()._yieldState = value; }
        }

        [ScriptableProperty("pending")]
        public bool Pending => _yieldState == PendingState;

        [ScriptableProperty("running")]
        public bool Running => _yieldState > PendingState;

        [ScriptableProperty("completed")]
        public bool Completed => _yieldState == CompletedState;

        [ScriptableProperty("result")]
        public object Result => HasResult ? _result : null;

        [ScriptableProperty("hasResult")]
        public bool HasResult => _result != NoResult;

        public Func<ScriptFibreContext, object> Target { get; set; }

        public ScriptFibreContext(Func<ScriptFibreContext, object> target)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            _stack.Push(this);
        }

        [ScriptableMethod("resume")]
        public object Resume()
        {
            if (Completed)
                ExceptionHelper.FibreContextCompleted();

            var context = _stack.Peek();
            context._result = context.Target(this);

            if (context != this && context.Completed)
                _stack.Pop();

            return Result;
        }

        [ScriptableMethod("execute")]
        public object Execute()
        {
            while(!Completed)
            {
                Resume();
            }

            return Result;
        }

        public object Await(ScriptFibreContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _stack.Push(context);

            return Resume();
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptObject(parameter, BindingRestrictions.Empty, this);
        }

        public static ScriptFibreContext New(Func<ScriptFibreContext, object> target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return new ScriptFibreContext(target);
        }
    }

}
