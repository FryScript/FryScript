using System;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests.Ast
{
    public static class Dynamic
    {
        public static bool HasBinderWithArgsCount<T>(this DynamicExpression expression, int argCount) where T : DynamicMetaObjectBinder
        {
            return typeof(T) == expression.Binder.GetType()
            && argCount == expression.Arguments.Count;
        }

        public static void AsDynamic<T>(this DynamicExpression expression, Action<T> action) where T : DynamicMetaObjectBinder
        {
            Assert.IsInstanceOfType(expression.Binder, typeof(T));

            action?.Invoke((T)expression.Binder);
        }
    }
}