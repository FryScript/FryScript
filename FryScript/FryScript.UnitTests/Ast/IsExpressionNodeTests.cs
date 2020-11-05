using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class IsExpressionNodeTests : AstNodeTestBase<IsExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public override void GetExpression_Single_Child_Gets_Child_Expression()
        {
            base.GetExpression_Single_Child_Gets_Child_Expression();
        }

        [TestMethod]
        public void GetExpression_Generates_Is_Expression()
        {
            var expectedIdentifierExpr = Expression.Constant(new object());
            var identifierNode = Node<AstNode>.Empty;
            identifierNode.GetExpression(expectedIdentifierExpr, Scope);

            var expectedTypeExpr = Expression.Constant(new object());
            var typeNode = Node<AstNode>.Empty;
            typeNode.GetExpression(expectedTypeExpr, Scope);

            Node.SetChildren(identifierNode, null, typeNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedIdentifierExpr, result.Arguments[0]);
            Assert.AreEqual(expectedTypeExpr, result.Arguments[1]);
            Assert.IsInstanceOfType(result.Binder, typeof(ScriptIsOperationBinder));
        }
    }
}
