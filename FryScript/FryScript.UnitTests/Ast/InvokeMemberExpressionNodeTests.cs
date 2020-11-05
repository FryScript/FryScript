using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class InvokeMemberExpressionTests : AstNodeTestBase<InvokeMemberExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Generates_Invoke()
        {
            var expectedTargetExpr = Expression.Constant(new object());
            var targetNode = Node<AstNode>.Empty;
            targetNode.GetExpression(expectedTargetExpr, Scope);

            var argsNode = Node<AstNode>.Empty;

            var memberNode = Node<AstNode>.Empty;
            memberNode.ValueString.Returns("member");

            var expectedArg1Expr = Expression.Constant(new object());
            var arg1Node = Node<AstNode>.Empty;
            arg1Node.GetExpression(expectedArg1Expr, Scope);

            var expectedArg2Expr = Expression.Constant(new object());
            var arg2Node = Node<AstNode>.Empty;
            arg2Node.GetExpression(expectedArg2Expr, Scope);

            argsNode.SetChildren(arg1Node, arg2Node);

            Node.SetChildren(targetNode, memberNode, argsNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);

            Assert.AreEqual(3, result.Arguments.Count);
            Assert.AreEqual(expectedTargetExpr, result.Arguments[0]);
            Assert.AreEqual(expectedArg1Expr, result.Arguments[1]);
            Assert.AreEqual(expectedArg2Expr, result.Arguments[2]);

            var binder = result.Binder as ScriptInvokeMemberBinder;
            Assert.AreEqual(2, binder.CallInfo.ArgumentCount);
            Assert.AreEqual("member", binder.Name);
        }
    }
}
