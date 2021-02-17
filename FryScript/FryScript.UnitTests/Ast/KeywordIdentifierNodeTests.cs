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
    public class KeywordIdentifierNodeTests : AstNodeTestBase<KeywordIdentifierNode>
    {
        [TestMethod]
        public void GetExpression_Calls_GetIdentifier()
        {
            var expectedExpr = Expression.Constant(new object());
            Node.Configure().GetIdentifier(Scope).Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetIdentifier_Null_Scope()
        {
            Node.GetIdentifier(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetIdentiier_Scope_Does_Not_Have_Member()
        {
            Node.StubParseNode();
            Node.StubCompilerContext();

            Node.Configure().ValueString.Returns("keyword");

            Node.GetIdentifier(Scope);
        }

        [TestMethod]
        public void GetExpression_Gets_Keyword_Scope_Member()
        {
            Node.Configure().ValueString.Returns("keyword");

            var expectedParamExpr = Scope.AddKeywordMember<ParameterExpression>("keyword", Node);

            var result = Node.GetIdentifier(Scope);

            Assert.AreEqual(expectedParamExpr, result);
        }
    }
}
