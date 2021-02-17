using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FibreExpressionNodeTests : AstNodeTestBase<FibreExpressionNode>
    {
        private FunctionExpressionNode _function;
        private FunctionParametersNode _functionParams;
        private FunctionBlockNode _functionBlock;

        public override void OnTestInitialize()
        {
            _function = Node<FunctionExpressionNode>.Empty;

            _functionParams = Node<FunctionParametersNode>.Empty;
            _functionBlock = Node<FunctionBlockNode>.Empty;

            _function.SetChildren(_functionParams, _functionBlock);

            Node.SetChildren(null, _function);

            _functionParams
                .StubCompilerContext()
                .StubParseNode();
        }

        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Get_Expression_Greater_Than_16_Parameters()
        {
            _functionParams
                .When(n => n.DeclareParameters(Arg.Is<Scope>(s => s.Parent == Scope)))
                .Do(c =>
                {
                    var scope = Scope.Children[0];

                    for (var i = 0; i <= 16; i++)
                        scope.AddMember($"{i}", Node);
                });

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Creates_Fibre_Context_Expression()
        {
            var fibreBlock = Node<FibreBlockNode>.Empty;
            Node.Configure()
                .Transform<FibreBlockNode>(_functionBlock.ChildNodes)
                .Returns(fibreBlock);

            var fibreContextExpr = Expression.Constant(new ScriptFibreContext());
            Node.Configure()
                .GetFibreContextExpression(Arg.Is<Scope>(s => s.Parent.Parent == Scope), fibreBlock)
                .Returns(fibreContextExpr);

            var result = Node.GetExpression(Scope) as MethodCallExpression;

            Assert.AreEqual(typeof(ScriptFibre), result.Type);
            Assert.AreEqual(1, result.Arguments.Count);

            var lambdaArg = result.Arguments[0] as LambdaExpression;
            Assert.AreEqual(typeof(Func<ScriptFibreContext>), lambdaArg.Type);

            var lambdaBody = lambdaArg.Body as BlockExpression;

            Assert.AreEqual(fibreContextExpr, lambdaBody.Expressions[0]);
        }

        [TestMethod]
        public void GetFibreContextExpression_Creates_Context_Generator()
        {
            var blockExpr = Expression.Constant("block expr", typeof(object));
            _functionBlock
                .GetExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(blockExpr);

            var yieldSwitchExpr = Expression.Constant("yield expr", typeof(object));
            Node.Configure()
                .AddYieldSwitchExpression(
                    Arg.Is<Scope>(s => s.Parent == Scope),
                    Arg.Any<ParameterExpression>(),
                    blockExpr,
                    Arg.Any<List<LabelTarget>>())
                .Returns(yieldSwitchExpr);

            var returnExpr = Expression.Constant("return expr", typeof(object));
            Node.Configure()
                .AddReturnExpression(
                    Arg.Any<LabelTarget>(),
                    yieldSwitchExpr)
                .Returns(returnExpr);

            var result = Node.GetFibreContextExpression(Scope, _functionBlock) as MethodCallExpression;

            Assert.AreEqual(typeof(ScriptFibreContext), result.Type);

            var arg = result.Arguments[0] as LambdaExpression;
            Assert.AreEqual(typeof(Func<ScriptFibreContext, object>), arg.Type);
            Assert.AreEqual(returnExpr, arg.Body);
        }

        [TestMethod]
        public void AddYieldSwitchExpression_No_Yield_Labels()
        {
            var yieldLabels = new List<LabelTarget>();
            var expectedExpr = Expression.Constant(new object());

            var result = Node.AddYieldSwitchExpression(Scope, null, expectedExpr, yieldLabels);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void AddYieldSwitchExpression_Generates_State_Machine()
        {
            var yieldLabels = new List<LabelTarget>
            {
                Expression.Label(),
                Expression.Label()
            };

            var paramExpr = Expression.Parameter(typeof(ScriptFibreContext));
            var bodyExpr = Expression.Constant(new object());

            var result = Node.AddYieldSwitchExpression(Scope, paramExpr, bodyExpr, yieldLabels) as BlockExpression;

            var switchExpr = result.Expressions[0] as SwitchExpression;
            Assert.AreEqual(2, switchExpr.Cases.Count);

            var case1 = switchExpr.Cases[0] as SwitchCase;
            Assert.AreEqual(0, (case1.TestValues[0] as ConstantExpression).Value);
            Assert.AreEqual(yieldLabels[0], (case1.Body as GotoExpression).Target);

            var case2 = switchExpr.Cases[1] as SwitchCase;
            Assert.AreEqual(1, (case2.TestValues[0] as ConstantExpression).Value);
            Assert.AreEqual(yieldLabels[1], (case2.Body as GotoExpression).Target);

            Assert.AreEqual(bodyExpr, result.Expressions[1]);
        }

        [TestMethod]
        public void AddReturnExpression_Generates_Return_Label()
        {
            var yieldTarget = Expression.Label(typeof(object));
            var bodyExpr = Expression.Constant(typeof(object));

            var result = Node.AddReturnExpression(yieldTarget, bodyExpr) as LabelExpression;

            Assert.AreEqual(yieldTarget, result.Target);
            Assert.AreEqual(bodyExpr, result.DefaultValue);
        }
    }
}
