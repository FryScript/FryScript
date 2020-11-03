using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptImportFromNodeTests : AstNodeTestBase<ScriptImportFromNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void Get_Expression_Transforms_Alias_List()
        {
            var aliasListNode = Node<ImportAliasListNode>.Empty;
            var scriptNameNode = Node<AstNode>.WithValueString("script");

            var expectedScriptObj = new ScriptObject();
            Node.StubCompilerContext();
            Node.CompilerContext.ScriptRuntime.Get("script", Node.CompilerContext.Uri)
                .Returns(expectedScriptObj);

            var expectedExpr = Expression.Constant(new object());
            aliasListNode.GetExpression(Scope, expectedScriptObj).Returns(expectedExpr);

            Node.SetChildren(null, aliasListNode, null, scriptNameNode);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }
    }
}
