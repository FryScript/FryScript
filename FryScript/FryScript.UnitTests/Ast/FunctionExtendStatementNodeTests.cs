using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FunctionExtendStatementNodeTests : AstNodeTestBase<FunctionExtendStatementNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Exposes_Base_Member()
        {
            var identifierNode = Node<IdentifierNode>.Empty;
            var functionNode = Node<FunctionExpressionNode>.Empty;

            var expectedGetIdentifierExpr = Expression.Constant("get identifier", typeof(object));
            identifierNode
                .GetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedGetIdentifierExpr);

            var expectedFuncExpr = Expression.Constant("function", typeof(object));
            functionNode.GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedFuncExpr);

            var expectedSetIdentifierExpr = Expression.Constant("set identifier", typeof(object));
            identifierNode
                .SetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope), expectedFuncExpr)
                .Returns(expectedSetIdentifierExpr);

            Node.SetChildren(identifierNode, null, functionNode);

            var result = Node.GetExpression(Scope) as BlockExpression;
            Assert.AreEqual(2, result.Expressions.Count);

            var assignExpr = result.Expressions[0] as BinaryExpression;
            Assert.AreEqual(ExpressionType.Assign, assignExpr.NodeType);

            var expectedBaseExpr = Scope
                .Children[0]
                .GetMemberExpression(Keywords.Base);
            Assert.AreEqual(expectedBaseExpr, assignExpr.Left);

            var extendExpr = assignExpr.Right as MethodCallExpression;
            Assert.AreEqual(nameof(ScriptFunction.Extend), extendExpr.Method.Name);
            Assert.AreEqual(typeof(ScriptFunction), extendExpr.Method.DeclaringType);
            Assert.AreEqual(1, extendExpr.Arguments.Count);

            var argExpr = extendExpr.Arguments[0];
            Assert.AreEqual(expectedGetIdentifierExpr, argExpr);

            Assert.AreEqual(expectedSetIdentifierExpr, result.Expressions[1]);
        }
    }
}
