using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ContinueStatementNodeTests : AstNodeTestBase<ContinueStatementNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
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
