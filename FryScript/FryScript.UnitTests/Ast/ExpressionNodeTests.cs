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
    public class ExpressionNodeTests : AstNodeTestBase<ExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Has_Debug_Hook()
        {
            var debugHook = Substitute.For<DebugHook>();
            StubCompilerContext(debugHook);

            var expectedExpr = Expression.Empty();
            Node.Configure().WrapDebugExpression(
                DebugEvent.Expression,
                Scope,
                Arg.Any<Func<Scope, Expression>>())
                .Returns(expectedExpr);
            
            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetExpression_Detailed_Exceptions()
        {
            StubCompilerContext(detailedExceptions: true);

            var expectedChildExpr = Expression.Empty();
            Node.Configure().GetChildExpression(Scope).Returns(expectedChildExpr);

            var expectedDetailedExceptionExpr = Expression.Empty();
            Node.Configure().GetDetailedExceptionExpression(
                expectedChildExpr,
                Node,
                Scope
                ).Returns(expectedDetailedExceptionExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedDetailedExceptionExpr, result);
        }

        [TestMethod]
        public void GetExpression_No_Detailed_Exception()
        {
            StubCompilerContext(detailedExceptions: false);

            var expectedExpr = Expression.Empty();
            Node.Configure().GetChildExpression(Scope).Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }
    }
}
