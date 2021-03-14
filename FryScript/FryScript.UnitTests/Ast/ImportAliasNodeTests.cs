using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ImportAliasNodeTests : AstNodeTestBase<ImportAliasNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Not_Implemented()
        {
            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Single_Identifier()
        {
            var expectedImport = new object();
            dynamic obj = new ScriptObject();
            obj.member = expectedImport;

            var expectedIdentifierExpr = Expression.Constant(new object());
            var identifierNode = Node<AstNode>.Empty;
            identifierNode
                .SetIdentifier(Scope, Arg.Is<ConstantExpression>(e => e.Value == expectedImport))
                .Returns(expectedIdentifierExpr);

            identifierNode.ValueString.Returns("member");

            Node.SetChildren(identifierNode);

            var result = Node.GetExpression(Scope, obj);

            Assert.AreEqual(expectedIdentifierExpr, result);
            identifierNode.Received().CreateIdentifier(Scope);
        }

        [TestMethod]
        public void GetExpression_Single_Identifier_And_Alias()
        {
            var expectedImport = new object();
            dynamic obj = new ScriptObject();
            obj.member = expectedImport;

            var identifierNode = Node<AstNode>.Empty;
            identifierNode.ValueString.Returns("member");

            var expectedAliasExpr = Expression.Constant(new object());
            var aliasNode = Node<AstNode>.Empty;
            aliasNode
                .SetIdentifier(Scope, Arg.Is<ConstantExpression>(e => e.Value == expectedImport))
                .Returns(expectedAliasExpr);

            Node.SetChildren(identifierNode, null, aliasNode);

            var result = Node.GetExpression(Scope, obj);

            Assert.AreEqual(expectedAliasExpr, result);
            aliasNode.Received().CreateIdentifier(Scope);

        }
    }
}
