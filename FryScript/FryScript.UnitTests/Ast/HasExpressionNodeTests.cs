using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class HasExpressionNodeTests : AstNodeTestBase<HasExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public override void GetExpression_Single_Child_Gets_Child_Expression()
        {
            base.GetExpression_Single_Child_Gets_Child_Expression();
        }

        [TestMethod]
        public void GetExpression_Creates_Has_Expression()
        {
            var identifierNode = Node<IdentifierExpressionNode>.Empty;
            var memberNameNode = Node<AstNode>.WithValueString("method");

            Node.SetChildren(identifierNode, null, memberNameNode);

            var expectedIdentifierExpr = Expression.Constant(new object());
            identifierNode.GetExpression(Scope).Returns(expectedIdentifierExpr);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.IsInstanceOfType(result.Binder, typeof(ScriptHasOperationBinder));
            Assert.AreEqual("method", (result.Binder as ScriptHasOperationBinder).Name);
            Assert.AreEqual(expectedIdentifierExpr, result.Arguments[0]);
        }
    }
}
