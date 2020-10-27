using FryScript.Ast;
using FryScript.Binders;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class MultiplyExpressionNodeTests : AstNodeTestBase<MultiplyExpressionNode>
    {
        private AstNode
           _left,
           _right,
           _op;

        private Expression
            _leftExpr,
            _rightExpr;

        private void SetOperator(string op)
        {
            _op.ValueString.Returns(op);
        }

        public override void OnTestInitialize()
        {
            _left = Node<IdentifierExpressionNode>.Empty;
            _right = Node<AstNode>.Empty;
            _op = Node<AstNode>.Empty;

            _leftExpr = Expression.Constant(new object());
            _rightExpr = Expression.Constant(new object());

            _left.GetExpression(Scope).Returns(_leftExpr);
            _right.GetExpression(Scope).Returns(_rightExpr);

            Node.SetChildren(_left, _op, _right);
        }

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

        [DataTestMethod]
        [DataRow(Operators.Multiply, ExpressionType.Multiply)]
        [DataRow(Operators.Divide, ExpressionType.Divide)]
        [DataRow(Operators.Modulo, ExpressionType.Modulo)]
        public void GetExpression_Uses_Operator(string op, ExpressionType expectedOp)
        {
            SetOperator(op);
            _left.GetExpression(_leftExpr, Scope);
            _right.GetExpression(_rightExpr, Scope);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(_leftExpr, result.Arguments[0]);
            Assert.AreEqual(_rightExpr, result.Arguments[1]);

            var binder = result.Binder as ScriptBinaryOperationBinder;
            Assert.AreEqual(expectedOp, binder.Operation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetExpression_Unknown_Operator()
        {
            SetOperator("unknown");

            Node.GetExpression(Scope);
        }
    }
}
