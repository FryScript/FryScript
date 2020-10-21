using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ForStatementNodeTests : AstNodeTestBase<ForStatementNode>
    {
        private AstNode
            _initNode,
            _conditionNode,
            _actionNode,
            _statementNode;

        public override void OnTestInitialize()
        {
            _initNode = Node<AstNode>.Empty;
            _conditionNode = Node<AstNode>.Empty;
            _actionNode = Node<AstNode>.Empty;
            _statementNode = Node<AstNode>.Empty;

            Node.SetChildren(null, _initNode, _conditionNode, _actionNode, _statementNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpresion_Generates_For_Loop()
        {
            var expectedInitExpr = Expression.Constant("init expression", typeof(object));
            _initNode
                .GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedInitExpr);

            var expectedConditionExpr = Expression.Constant(true);
            _conditionNode
                .GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedConditionExpr);

            var expectedActionExpr = Expression.Constant("action expression", typeof(object));
            _actionNode
                .GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedActionExpr);

            var expectedStatement = Expression.Constant("statement", typeof(object));
            _statementNode
                .GetExpression(Arg.Is<Scope>(s => s.Parent.Parent == Scope))
                .Returns(expectedStatement);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);
            Assert.AreEqual(expectedInitExpr, result.Expressions[0]);

            var loopExpr = result.Expressions[1] as LoopExpression;
            Scope
                .Children[0]
                .TryGetData(ScopeData.BreakTarget, out LabelTarget expectedBreakTarget);
            Assert.AreEqual(expectedBreakTarget, loopExpr.BreakLabel);

            Scope
                .Children[0]
                .TryGetData(ScopeData.ContinueTarget, out LabelTarget expectedContinueTarget);
            Assert.AreEqual(expectedContinueTarget, loopExpr.ContinueLabel);

            var bodyExpr = loopExpr.Body as BlockExpression;
            Assert.AreEqual(2, bodyExpr.Expressions.Count);
            Assert.AreEqual(expectedActionExpr, bodyExpr.Expressions[1]);

            var ifExpr = bodyExpr.Expressions[0] as ConditionalExpression;
            var convertTestExpr = ifExpr.Test as DynamicExpression;
            var convertBinder = convertTestExpr.Binder as ScriptConvertBinder;
            Assert.AreEqual(typeof(bool), convertBinder.Type);

            Assert.AreEqual(expectedStatement, ifExpr.IfTrue);

            var gotoExpr = ifExpr.IfFalse as GotoExpression;
            Assert.AreEqual(expectedBreakTarget, gotoExpr.Target);
        }
    }
}
