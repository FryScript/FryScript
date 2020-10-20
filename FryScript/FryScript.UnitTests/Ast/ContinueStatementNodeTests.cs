using FryScript.Ast;
using FryScript.Compilation;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ContinueStatementNodeTests : AstNodeTestBase<ContinueStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope_Test()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_Invalid_Continue_Context()
        {
            Scope.RemoveData(ScopeData.ContinueTarget);
            Node.StubParseNode();
            Node.StubCompilerContext();

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Continue()
        {
            var expectedLabel = Expression.Label();
            Scope.SetData(ScopeData.ContinueTarget, expectedLabel);

            var result = Node.GetExpression(Scope) as GotoExpression;

            Assert.AreEqual(expectedLabel, result.Target);
            Assert.AreEqual(typeof(object), result.Type);
        }
    }
}
