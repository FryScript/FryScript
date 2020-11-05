using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TernaryExpressionNodeTests : AstNodeTestBase<TernaryExpressionNode>
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
        public void GetExpression_Three_Operands()
        {
            var testNode = Node<AstNode>.Empty;
            var trueNode = Node<AstNode>.Empty;
            var falseNode = Node<AstNode>.Empty;

            var expectedTestExpr = Expression.Constant("test", typeof(object));
            testNode.GetExpression(expectedTestExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var expectedTrueExpr = Expression.Constant(true, typeof(object));
            trueNode.GetExpression(expectedTrueExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var expectedFalseExpr = Expression.Constant(false, typeof(object));
            falseNode.GetExpression(expectedFalseExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            Node.SetChildren(testNode, trueNode, falseNode);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);

            var expectedFlagExpr = Scope.Children.Single().GetAllowedMembers(true).Single().Parameter;
            var assignFlagExpr = result.Expressions[0] as BinaryExpression;

            Assert.AreEqual(ExpressionType.Assign, assignFlagExpr.NodeType);
            Assert.AreEqual(expectedFlagExpr, assignFlagExpr.Left);
            Assert.AreEqual(expectedTestExpr, assignFlagExpr.Right);

            var conditionExpr = result.Expressions[1] as ConditionalExpression;
            var testExpr = conditionExpr.Test as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, testExpr.NodeType);

            var binder = testExpr.Binder as ScriptConvertBinder;
            Assert.AreEqual(typeof(bool), binder.Type);

            Assert.AreEqual(1, testExpr.Arguments.Count);
            Assert.AreEqual(expectedFlagExpr, testExpr.Arguments[0]);

            Assert.AreEqual(expectedTrueExpr, conditionExpr.IfTrue);
            Assert.AreEqual(expectedFalseExpr, conditionExpr.IfFalse);
        }

        [TestMethod]
        public void GetExpression_Two_Operands_Uses_Test_Result_As_True_Condition()
        {
            var testNode = Node<AstNode>.Empty;
            var falseNode = Node<AstNode>.Empty;

            var expectedTestExpr = Expression.Constant("test", typeof(object));
            testNode.GetExpression(expectedTestExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            var expectedFalseExpr = Expression.Constant(false, typeof(object));
            falseNode.GetExpression(expectedFalseExpr, Arg.Is<Scope>(s => s.Parent == Scope));

            Node.SetChildren(testNode, falseNode);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);

            var expectedFlagExpr = Scope.Children.Single().GetAllowedMembers(true).Single().Parameter;
            var assignFlagExpr = result.Expressions[0] as BinaryExpression;

            Assert.AreEqual(ExpressionType.Assign, assignFlagExpr.NodeType);
            Assert.AreEqual(expectedFlagExpr, assignFlagExpr.Left);
            Assert.AreEqual(expectedTestExpr, assignFlagExpr.Right);

            var conditionExpr = result.Expressions[1] as ConditionalExpression;
            var testExpr = conditionExpr.Test as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, testExpr.NodeType);

            var binder = testExpr.Binder as ScriptConvertBinder;
            Assert.AreEqual(typeof(bool), binder.Type);

            Assert.AreEqual(1, testExpr.Arguments.Count);
            Assert.AreEqual(expectedFlagExpr, testExpr.Arguments[0]);

            Assert.AreEqual(expectedFlagExpr, conditionExpr.IfTrue);
            Assert.AreEqual(expectedFalseExpr, conditionExpr.IfFalse);
        }
    }
}
