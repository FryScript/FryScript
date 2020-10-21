using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;

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
            var result = Node.GetExpression(Scope);
            throw new NotImplementedException();
        }
    }
}
