using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Debugging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class StatementNodeTests : AstNodeTestBase<StatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Returns_Child_Expression()
        {
            Node.StubCompilerContext();

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GeExpression_Wraps_Debug_Expression()
        {
            Node.StubCompilerContext(debugHook: d => { });

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var expectedWrappedExpr = Expression.Constant(new object());
            Func<Scope, Expression> func = null;
            Node.Configure()
                .WrapDebugExpression(DebugEvent.Statement,
                Scope,
                Arg.Do<Func<Scope, Expression>>(a => func = a))
                .Returns(expectedWrappedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedWrappedExpr, result);

            var funcResult = func(Scope);

            Assert.AreEqual(expectedExpr, funcResult);
        }
    }
}
