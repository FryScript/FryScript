using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ArrayExpressionNodeTests : AstNodeTestBase<ArrayExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Empty_Array()
        {
            Node.SetChildren(Node<AstNode>.WithValue(1));

            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(ExpressionType.New, result.NodeType);
            Assert.AreEqual(typeof(ScriptArray), result.Type);
            Assert.AreEqual(0, result.Arguments.Count);
        }

        [TestMethod]
        public void GetExpression_Array_With_Items()
        {
            var arrayItems = Node<AstNode>.WithChildren(
                Node<AstNode>.WithValue(new object()),
                Node<AstNode>.WithValue(new object()),
                Node<AstNode>.WithValue(new object())
            );

            Node.SetChildren(arrayItems);

            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(ExpressionType.New, result.NodeType);
            Assert.AreEqual(typeof(ScriptArray), result.Type);

            arrayItems.ChildNodes[0].Received().GetExpression(Scope);
            arrayItems.ChildNodes[1].Received().GetExpression(Scope);
            arrayItems.ChildNodes[2].Received().GetExpression(Scope);
        }
    }
}