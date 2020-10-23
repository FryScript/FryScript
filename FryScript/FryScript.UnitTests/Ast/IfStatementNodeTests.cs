using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class IfStatementNodeTests : AstNodeTestBase<IfStatementNode>
    {
        private AstNode
            _conditionNode,
            _thenNode,
            _elseNode;

        public override void OnTestInitialize()
        {
            _conditionNode = Node<AstNode>.Empty;
            _thenNode = Node<AstNode>.Empty;
            _elseNode = Node<AstNode>.Empty;

            Node.SetChildren(null, _conditionNode, _thenNode, null, _elseNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void Get_Expression_Hoisted_Scope_Is_Made_Awaitable()
        {
            Scope = Scope.New(Node, hoisted: true);

            var expectedExpr = Expression.Empty();
            Node.Configure()
                .TryMakeAwaitable(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetExpression_If_Then_Statement()
        {
            Node.SetChildren(null, _conditionNode, _thenNode);

            var expectedConditionExpr = Expression.Constant(new object());
            _conditionNode.GetExpression(expectedConditionExpr, Scope);

            var expectedThenExpr = Expression.Constant(new object());
            _thenNode.GetExpression(expectedThenExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var result = Node.GetExpression(Scope) as ConditionalExpression;

            var convertExpr = result.Test as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, convertExpr.NodeType);
            Assert.AreEqual(expectedConditionExpr, convertExpr.Arguments[0]);
            Assert.AreEqual(typeof(bool), (convertExpr.Binder as ScriptConvertBinder).Type);

            var thenBlockExpr = result.IfTrue as BlockExpression;
            Assert.AreEqual(1, thenBlockExpr.Expressions.Count);
            Assert.AreEqual(expectedThenExpr, thenBlockExpr.Expressions[0]);

            var elseBlockExpr = result.IfFalse as BlockExpression;
            Assert.AreEqual(1, elseBlockExpr.Expressions.Count);
            Assert.AreEqual(null, (elseBlockExpr.Expressions[0] as ConstantExpression).Value);
        }

        [TestMethod]
        public void GetExpression_If_Then_Else_Statement()
        {
            var expectedConditionExpr = Expression.Constant(new object());
            _conditionNode.GetExpression(expectedConditionExpr, Scope);

            var expectedThenExpr = Expression.Constant(new object());
            _thenNode.GetExpression(expectedThenExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var expectedElseExpr = Expression.Constant(new object());
            _elseNode.GetExpression(expectedElseExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var result = Node.GetExpression(Scope) as ConditionalExpression;

            var convertExpr = result.Test as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, convertExpr.NodeType);
            Assert.AreEqual(expectedConditionExpr, convertExpr.Arguments[0]);
            Assert.AreEqual(typeof(bool), (convertExpr.Binder as ScriptConvertBinder).Type);

            var thenBlockExpr = result.IfTrue as BlockExpression;
            Assert.AreEqual(1, thenBlockExpr.Expressions.Count);
            Assert.AreEqual(expectedThenExpr, thenBlockExpr.Expressions[0]);

            var elseBlockExpr = result.IfFalse as BlockExpression;
            Assert.AreEqual(1, elseBlockExpr.Expressions.Count);
            Assert.AreEqual(expectedElseExpr, elseBlockExpr.Expressions[0]);
        }
    }
}
