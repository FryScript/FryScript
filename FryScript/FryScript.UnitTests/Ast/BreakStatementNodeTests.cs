using FryScript.Ast;
using FryScript.Compilation;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class BreakStatementNodeTests : AstNodeTestBase<BreakStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Nul_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_No_BreakTarget_Defined()
        {
            Node.ParseNode = new ParseTreeNode(new Token(new Terminal("test"), new SourceLocation(1, 2, 3), "test", "test"));

            Node.CompilerContext = new CompilerContext(Substitute.For<IScriptRuntime>(), new Uri("test://test"));

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_BreakTarget_Defined()
        {
            var expectedLabel = Expression.Label(typeof(object));
            Scope.SetData(ScopeData.BreakTarget, expectedLabel);

            var result = Node.GetExpression(Scope) as GotoExpression;

            Assert.AreEqual(expectedLabel, result.Target);
            Assert.AreEqual(typeof(object), result.Type);

            var valueExpr = result.Value as ConstantExpression;

            Assert.IsNull(valueExpr.Value);
        }
    }
}
