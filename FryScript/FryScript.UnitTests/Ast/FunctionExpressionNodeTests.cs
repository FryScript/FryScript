using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FunctionExpressionNodeTests : AstNodeTestBase<FunctionExpressionNode>
    {
        FunctionParametersNode _paramsNode;
        AstNode _blockNode;

        public override void OnTestInitialize()
        {
            _paramsNode = Node<FunctionParametersNode>.Empty;
            _blockNode = Node<AstNode>.Empty;

            Node.SetChildren(_paramsNode, _blockNode);

            _paramsNode.StubCompilerContext();
            _paramsNode.StubParseNode();

            Node.StubCompilerContext();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_More_Than_Sixteen_Parameters()
        {
            _paramsNode
                .Configure()
                .When(p => p.DeclareParameters(Arg.Any<Scope>()))
                .Do(c =>
                {
                    var scope = c[0] as Scope;

                    for (var i = 0; i <= 16; i++)
                        scope.AddMember($"{i}", _paramsNode);
                });
            
            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Empty_Function()
        {
            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(typeof(ScriptFunction), result.Constructor.DeclaringType);
            Assert.AreEqual(1, result.Arguments.Count);

            var lambdaExpr = result.Arguments[0] as LambdaExpression;
            Assert.AreEqual(typeof(object), lambdaExpr.ReturnType);

            var bodyExpr = lambdaExpr.Body as LabelExpression;
            var constantExpr = bodyExpr.DefaultValue as ConstantExpression;
            Assert.IsNull(constantExpr.Value);

            Scope
                .Children[0]
                .Children[0]
                .TryGetData(ScopeData.ReturnTarget, out LabelTarget expectedReturnTarget);

            Assert.AreEqual(expectedReturnTarget, bodyExpr.Target);
        }

        [TestMethod]
        public void GetExpression_With_Function_Body()
        {
            var expectedBlockExpr = Expression.Constant("block", typeof(object));
            var blockChildren = Node<AstNode>.Empty;
            
            _blockNode.SetChildren(blockChildren);
            _blockNode
                .GetExpression(Arg.Is<Scope>(s => s.Parent.Parent == Scope))
                .Returns(expectedBlockExpr);

            var result = Node.GetExpression(Scope) as NewExpression;

            Assert.AreEqual(typeof(ScriptFunction), result.Constructor.DeclaringType);
            Assert.AreEqual(1, result.Arguments.Count);

            var lambdaExpr = result.Arguments[0] as LambdaExpression;
            Assert.AreEqual(typeof(object), lambdaExpr.ReturnType);

            var bodyExpr = lambdaExpr.Body as LabelExpression;
            var constantExpr = bodyExpr.DefaultValue as ConstantExpression;
            Assert.AreEqual(expectedBlockExpr, constantExpr);

            Scope
                .Children[0]
                .Children[0]
                .TryGetData(ScopeData.ReturnTarget, out LabelTarget expectedReturnTarget);

            Assert.AreEqual(expectedReturnTarget, bodyExpr.Target);
        }
    }
}
