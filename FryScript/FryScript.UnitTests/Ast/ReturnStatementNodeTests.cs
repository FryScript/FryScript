using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ReturnStatementNodeTests : AstNodeTestBase<ReturnStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_No_Return_Context()
        {
            Node.StubCompilerContext();
            Node.StubParseNode();

            Scope.RemoveData(ScopeData.ReturnTarget);

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Return_No_Value()
        {
            var expectedTarget = Expression.Label();
            Scope.SetData(ScopeData.ReturnTarget, expectedTarget);

            Node.SetChildren(Node<AstNode>.Empty);

            var result = Node.GetExpression(Scope) as GotoExpression;

            Assert.AreEqual(expectedTarget, result.Target);
            Assert.IsNull((result.Value as ConstantExpression).Value);
        }

        [TestMethod]
        public void GetExpression_Return_Value()
        {
            var expectedTarget = Expression.Label();
            Scope.SetData(ScopeData.ReturnTarget, expectedTarget);

            var expectedExpr = Expression.Constant(new object());
            var expressionNode = Node<AstNode>.Empty;
            expressionNode.GetExpression(expectedExpr, Scope);

            Node.SetChildren(Node<AstNode>.Empty, expressionNode);

            var result = Node.GetExpression(Scope) as GotoExpression;

            Assert.AreEqual(expectedTarget, result.Target);
            Assert.AreEqual(expectedExpr, result.Value);
        }
    }
}
