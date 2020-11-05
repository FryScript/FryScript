using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ObjectLiteralExpressionNodeTests : AstNodeTestBase<ObjectLiteralExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_No_Children_Returns_New_Empty_Object()
        {
            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(typeof(ScriptObject), result.Type);
            Assert.AreEqual(0, result.Arguments.Count);
        }

        [TestMethod]
        public void GetExpression_With_Children_Returns_New_Object()
        {
            var expectedChildExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedChildExpr);

            Node.SetChildren(Node<AstNode>.Empty);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Scope.Children.First().TryGetData(
                ScopeData.ObjectLiteralContext,
                out ParameterExpression expectedParamExpr);

            Assert.AreEqual(3, result.Expressions.Count);

            var assignExpr = result.Expressions[0] as BinaryExpression;

            Assert.AreEqual(expectedParamExpr, assignExpr.Left);

            var newExpr = assignExpr.Right as NewExpression;
            
            Assert.AreEqual(typeof(ScriptObject), newExpr.Type);
            Assert.AreEqual(0, newExpr.Arguments.Count);

            var bodyExpr = result.Expressions[1];

            Assert.AreEqual(expectedChildExpr, bodyExpr);

            Assert.AreEqual(expectedParamExpr, result.Expressions[2]);
        }

        [TestMethod]
        public void GetExpression_Null_Child_Expression_Returns_New_Empty_Object()
        {
            Node.Configure()
                .GetChildExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(null as Expression);

            Node.SetChildren(Node<AstNode>.Empty);

            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(typeof(ScriptObject), result.Type);
            Assert.AreEqual(0, result.Arguments.Count);
        }
    }
}
