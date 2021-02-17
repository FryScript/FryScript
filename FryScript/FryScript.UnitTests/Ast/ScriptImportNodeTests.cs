using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptImportNodeTests : AstNodeTestBase<ScriptImportNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Imports_Script()
        {
            var scriptNameNode = Node<AstNode>.WithValueString("script");
            var aliasNode = Node<AstNode>.WithValueString("alias");

            var expectedAliasExpr = Expression.Constant(new object());
            aliasNode.SetIdentifier(Scope, Arg.Any<Expression>())
                .Returns(expectedAliasExpr);

            Node.StubCompilerContext();

            var expectedScriptObj = new ScriptObject();
            Node.CompilerContext.ScriptRuntime.Get("script", Node.CompilerContext.Uri)
                .Returns(expectedScriptObj);

            Node.SetChildren(null, scriptNameNode, null, aliasNode);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedAliasExpr, result);
            aliasNode.Received().CreateIdentifier(Scope);

            Assert.AreEqual(1, Node.CompilerContext.ImportInfos.Count);

            var importInfo = Node.CompilerContext.ImportInfos.Single();

            Assert.AreEqual("alias", importInfo.Alias);
            Assert.IsNotNull(importInfo.Object);

            var aliasObj = importInfo.Object as ScriptImport;

            Assert.AreEqual(expectedScriptObj, aliasObj.Target);

        }
    }
}
