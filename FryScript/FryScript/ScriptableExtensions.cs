namespace FryScript
{
    using System;
    using System.Dynamic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using Helpers;
    using HostInterop;

    public static class ScriptableExtensions
    {
        private class ScriptableBinder : DynamicMetaObjectBinder
        {
            public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
            {
                var name = TypeProvider.Current.GetTypeName(target.LimitType);
                var ctor = TypeProvider.Current.GetCtor(target.LimitType);

                var targetExpr = target.Expression;
                var nameExpr = Expression.Constant(name);
                var ctorExpr = Expression.Constant(ctor);
                var newScriptObjectExpr = ExpressionHelper.NewScriptObject(
                    targetExpr,
                    nameExpr,
                    ctorExpr
                    );

                var scriptableExpr = Expression.Convert(target.Expression, typeof (IScriptable));
                var scriptPropertyExpr = Expression.Property(scriptableExpr, "Script");
                var assignScriptPropertyExpr = Expression.Assign(scriptPropertyExpr, newScriptObjectExpr);

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);

                return new DynamicMetaObject(assignScriptPropertyExpr, restrictions);
            }
        }

        private static readonly CallSite<Func<CallSite, IScriptable, object>> _callSite;
        private static readonly ScriptableBinder _binder;

        static ScriptableExtensions()
        {
            _binder = new ScriptableBinder();
            _callSite = CallSite<Func<CallSite, IScriptable, object>>.Create(_binder);
        }

        public static void Bind(this IScriptable scriptable)
        {
            if (scriptable == null) 
                throw new ArgumentNullException("scriptable");

            if (scriptable.Script != null)
                return;

            _callSite.Target(_callSite, scriptable);
        }
    }
}
