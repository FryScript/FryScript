using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class CatchStatementNodeTests : AstNodeTestBase<CatchStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Not_Implemented()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCatchBlock_Null_Scope()
        {
            Node.GetCatchBlock(null);
        }

        [TestMethod]
        public void GetCatchBlock_Creates_Catch_Block()
        {
            var identifier = Node<IdentifierNode>.Empty;
            var block = Node<AstNode>.Empty;

            Node.SetChildren(null, identifier, block);

            var expectedAssignExpr = Expression.Constant("assign expr");
            var expectedBlockExpr = Expression.Block(Expression.Constant("block expr"));

            identifier.SetIdentifier(
                Arg.Is<Scope>(s => s.Parent == Scope),
                Arg.Is<MethodCallExpression>(m =>
                    m.Method.Name == nameof(FryScriptException.GetCatchObject) &&
                    m.Method.DeclaringType == typeof(FryScriptException)))
                .Returns(expectedAssignExpr);

            block.GetExpression(Arg.Is<Scope>(s => s.Parent.Parent == Scope))
                .Returns(expectedBlockExpr);

            var result = Node.GetCatchBlock(Scope) as CatchBlock;

            var childScope = Scope.Children.Single();
            identifier.Received().CreateIdentifier(Arg.Is<Scope>(s => s.Parent == Scope));

            Assert.IsTrue(childScope.TryGetData(ScopeData.CurrentException, out ParameterExpression catchParamExpr));
            Assert.IsNotNull(catchParamExpr);

            var actualAssignExpr = (result.Body as BlockExpression).Expressions[0];
            Assert.AreEqual(expectedAssignExpr, actualAssignExpr);

            var actualBlockExpr = (result.Body as BlockExpression).Expressions[1];
            Assert.AreEqual(expectedBlockExpr, actualBlockExpr);
        }
    }
}
