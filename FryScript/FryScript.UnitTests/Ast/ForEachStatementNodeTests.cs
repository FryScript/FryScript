using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ForEachStatementNodeTests : AstNodeTestBase<ForEachStatementNode>
    {
        private IdentifierNode _identifier;
        private AstNode _enumerable;
        private AstNode _loopBody;

        public override void OnTestInitialize()
        {
            _identifier = Node<IdentifierNode>.Empty;
            _enumerable = Node<AstNode>.Empty;
            _loopBody = Node<AstNode>.Empty;

            Node.SetChildren(null, null, _identifier, null, _enumerable, _loopBody);
        }

        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Generates_For_Each_Loop()
        {
            var expectedEnumerableExpr = Expression.Constant("enumerable", typeof(object));
            _enumerable
                .GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedEnumerableExpr);

            var expectedLoopBodyExpr = Expression.Constant("loop body", typeof(object));
            _loopBody
                .GetExpression(Arg.Is<Scope>(s => s.Parent.Parent.Parent == Scope))
                .Returns(expectedLoopBodyExpr);

            var expectedIdentifierExpr = Expression.Constant("assign identifier", typeof(object));
            _identifier
                .SetIdentifier(Arg.Is<Scope>(s => s.Parent.Parent.Parent == Scope), Arg.Any<Expression>())
                .Returns(expectedIdentifierExpr);


            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);

            var assignEnumerator = result.Expressions[0] as BinaryExpression;
            Assert.AreEqual(ExpressionType.Assign, assignEnumerator.NodeType);
            Assert.AreEqual(typeof(IEnumerator), assignEnumerator.Left.Type);

            var rightAssign = assignEnumerator.Right as MethodCallExpression;
            Assert.AreEqual(typeof(EnumerableHelper), rightAssign.Method.DeclaringType);
            Assert.AreEqual(nameof(EnumerableHelper.GetEnumerator), rightAssign.Method.Name);
            Assert.AreEqual(expectedEnumerableExpr, rightAssign.Arguments[0]);

            var loopExpr = result.Expressions[1] as LoopExpression;

            Scope
                .Children[0]
                .Children[0]
                .Children[0]
                .TryGetData(ScopeData.BreakTarget, out LabelTarget expectedBreakLabel);
            Assert.AreEqual(expectedBreakLabel, loopExpr.BreakLabel);

            Scope
                .Children[0]
                .Children[0]
                .Children[0]
                .TryGetData(ScopeData.ContinueTarget, out LabelTarget expectedContinueLabel);
            Assert.AreEqual(expectedContinueLabel, loopExpr.ContinueLabel);

            var loopBodyExpr = loopExpr.Body as BlockExpression;

            var ifElseExpr = loopBodyExpr.Expressions[0] as ConditionalExpression;
            var testExpr = ifElseExpr.Test as MethodCallExpression;
            Assert.AreEqual(nameof(IEnumerator.MoveNext), testExpr.Method.Name);
            Assert.AreEqual(typeof(IEnumerator), testExpr.Method.DeclaringType);

            var ifTrueExpr = ifElseExpr.IfTrue as BlockExpression;
            Assert.AreEqual(expectedIdentifierExpr, ifTrueExpr.Expressions[0]);
            Assert.AreEqual(expectedLoopBodyExpr, ifTrueExpr.Expressions[1]);

            var ifFalseExpr = ifElseExpr.IfFalse as GotoExpression;
            Assert.AreEqual(expectedBreakLabel, ifFalseExpr.Target);
        }
    }
}
