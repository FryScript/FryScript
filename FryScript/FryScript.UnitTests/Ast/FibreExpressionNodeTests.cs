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
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
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
        public void MyTestMethod()
        {
            throw new NotImplementedException();
        }
    }
}
