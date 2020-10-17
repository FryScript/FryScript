using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class BinaryExpressionNodeTests : AstNodeTestBase<BinaryExpressionNode>
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

            _leftExpr = Expression.Parameter(typeof(object));
            _rightExpr = Expression.Constant(true, typeof(object));

            _left.GetIdentifier(Scope).Returns(_leftExpr);
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
        public void GetExpression_Single_Child_Gets_Child_Expression()
        {
            TestSingleChildNode();
        }

        [DataTestMethod]
        [DataRow(Operators.IncrementAssign)]
        [DataRow(Operators.DecrementAssign)]
        [DataRow(Operators.MultiplyAssign)]
        [DataRow(Operators.DivideAssign)]
        [DataRow(Operators.ModuloAssign)]
        public void GetExpression_Uses_Operator(string op)
        {
            SetOperator(op);
            var expectedExpr = Expression.Constant(true);
            _left.SetIdentifier(Scope, Arg.Any<Expression>()).Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetExpression_Invalid_Operator()
        {
            SetOperator("invalid");

            Node.GetExpression(Scope);
        }
    }
}
