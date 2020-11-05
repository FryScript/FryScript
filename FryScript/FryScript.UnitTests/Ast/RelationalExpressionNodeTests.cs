using FryScript.Ast;
using FryScript.Binders;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class RelationalExpressionNodeTests : AstNodeTestBase<RelationalExpressionNode>
    {
        private AstNode
            _leftNode,
            _opNode,
            _rightNode;

        private Expression
            _leftExpr,
            _rightExpr;

        public override void OnTestInitialize()
        {
            _leftExpr = Expression.Constant(new object());
            _rightExpr = Expression.Constant(new object());

            _leftNode = Node<AstNode>.Empty;
            _leftNode.GetExpression(_leftExpr, Scope);

            _rightNode = Node<AstNode>.Empty;
            _rightNode.GetExpression(_rightExpr, Scope);
        }

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

        [DataTestMethod]
        [DataRow(Operators.Equal, ExpressionType.Equal)]
        [DataRow(Operators.NotEqual, ExpressionType.NotEqual)]
        [DataRow(Operators.GreaterThan, ExpressionType.GreaterThan)]
        [DataRow(Operators.LessThan, ExpressionType.LessThan)]
        [DataRow(Operators.GreaterThanOrEqual, ExpressionType.GreaterThanOrEqual)]
        [DataRow(Operators.LessThanOrEqual, ExpressionType.LessThanOrEqual)]
        public void GetExpression_Expected_Operator(string op, ExpressionType expectedOp)
        {
            SetOperator(op);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(_leftExpr, result.Arguments[0]);
            Assert.AreEqual(_rightExpr, result.Arguments[1]);

            var binder = result.Binder as ScriptBinaryOperationBinder;

            Assert.AreEqual(expectedOp, binder.Operation);
        }

        private void SetOperator(string op)
        {
            _opNode = Node<AstNode>.WithValueString(op);

            Node.SetChildren(_leftNode, _opNode, _rightNode);
        }
    }
}
