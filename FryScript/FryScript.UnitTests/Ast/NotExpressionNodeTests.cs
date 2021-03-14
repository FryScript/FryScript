using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class NotExpressionNodeTests : AstNodeTestBase<NotExpressionNode>
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
        public void GetExpression_Creates_Not_Expression()
        {
            var expectedExpr = Expression.Constant(new object());
            var exprNode = Node<AstNode>.Empty;
            exprNode.GetExpression(expectedExpr, Scope);

            Node.SetChildren(null, exprNode);

            var result = Node.GetExpression(Scope) as UnaryExpression;

            Assert.AreEqual(ExpressionType.Convert, result.NodeType);
            Assert.AreEqual(typeof(object), result.Type);

            var notExpr = result.Operand as UnaryExpression;

            Assert.AreEqual(ExpressionType.Not, notExpr.NodeType);
            
            var booleanConvert = notExpr.Operand as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, booleanConvert.NodeType);
            Assert.AreEqual(1, booleanConvert.Arguments.Count);
            Assert.AreEqual(expectedExpr, booleanConvert.Arguments[0]);
        }
    }
}
