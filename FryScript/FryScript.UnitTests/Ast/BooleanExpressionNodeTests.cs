using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class BooleanExpressionNodeTests : AstNodeTestBase<BooleanExpressionNode>
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
            _left = Node<AstNode>.Empty;
            _right = Node<AstNode>.Empty;
            _op = Node<AstNode>.Empty;

            _leftExpr = Expression.Constant(true, typeof(object));
            _rightExpr = Expression.Constant(true, typeof(object));

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
        [DataRow(Operators.And, ExpressionType.And)]
        [DataRow(Operators.Or, ExpressionType.Or)]
        public void GetExpression_Operators(string op, ExpressionType expectedExpressionType)
        {
            SetOperator(op);
   
            var result = Node.GetExpression(Scope) as UnaryExpression;

            Assert.AreEqual(ExpressionType.Convert, result.NodeType);
            var operand = result.Operand as BinaryExpression;

            var leftExpr = operand.Left as DynamicExpression;
            var rightExpr = operand.Right as DynamicExpression;

            Assert.IsInstanceOfType(leftExpr.Binder, typeof(ScriptConvertBinder));
            Assert.AreEqual(typeof(bool), (leftExpr.Binder as ScriptConvertBinder).Type);
            Assert.AreEqual(_leftExpr, leftExpr.Arguments[0]);

            Assert.IsInstanceOfType(rightExpr.Binder, typeof(ScriptConvertBinder));
            Assert.AreEqual(typeof(bool), (rightExpr.Binder as ScriptConvertBinder).Type);
            Assert.AreEqual(_rightExpr, rightExpr.Arguments[0]);

            Assert.AreEqual(expectedExpressionType, operand.NodeType);

        }
    }
}
