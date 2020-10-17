using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ConditionalAssignExpressionNodeTests : AstNodeTestBase<ConditionalAssignExpressionNode>
    {
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

        [TestMethod]
        public void GetExpression_Non_Hoisted_Scope()
        {
            var left = Node<IdentifierExpressionNode>.Empty;
            var right = Node<AstNode>.Empty;

            var expectedParamExpr = Expression.Parameter(typeof(object));
            var expectedRightExpr = Expression.Constant(new object());

            left.GetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope)).Returns(expectedParamExpr);
            right.GetExpression(Arg.Is<Scope>(s => s.Parent == Scope)).Returns(expectedRightExpr);

            var expectedSetExpr = Expression.Constant(new object());
            left.SetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope), expectedRightExpr).Returns(expectedSetExpr);

            Node.SetChildren(left, null, right);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);

            var tempVarAssignExpr = result.Expressions[0] as BinaryExpression;
            Assert.AreEqual(ExpressionType.Assign, tempVarAssignExpr.NodeType);

            var tempVarInfo = Scope.Children[0].GetAllowedMembers(includeCompilerMembers: true).Single();
            var tempVarExpr = tempVarAssignExpr.Left as ParameterExpression;
            Assert.AreEqual(tempVarInfo.Name, tempVarExpr.Name);
            Assert.AreEqual(tempVarInfo.Parameter, tempVarExpr);
            Assert.AreEqual(expectedParamExpr, tempVarAssignExpr.Right);

            var conditionalExpr = result.Expressions[1] as ConditionalExpression;
            var testExpr = conditionalExpr.Test as BinaryExpression;
            Assert.AreEqual(ExpressionType.NotEqual, testExpr.NodeType);
            Assert.AreEqual(tempVarExpr, conditionalExpr.IfTrue);
            Assert.AreEqual(expectedSetExpr, conditionalExpr.IfFalse);

            Assert.AreEqual(tempVarAssignExpr, result.Expressions[0]);
            Assert.AreEqual(conditionalExpr, result.Expressions[1]);
        }
    }
}
