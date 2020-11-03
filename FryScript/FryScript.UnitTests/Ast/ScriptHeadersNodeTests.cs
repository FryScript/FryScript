using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptHeadersNodeTests : AstNodeTestBase<ScriptHeadersNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_No_Chldren_Returns_Null()
        {
            Node.SetChildren(new AstNode[0]);

            var result = Node.GetExpression(Scope);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_Multiple_Extend_Nodes_Throws_Compiler_Exception()
        {
            var extendNode1 = Node<ScriptExtendNode>.Empty;
            var extendNode2 = Node<ScriptExtendNode>.Empty;
            extendNode2.StubCompilerContext();
            extendNode2.StubParseNode();

            Node.SetChildren(extendNode1, extendNode2);

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_Orders_ChildNodes_And_Gets_Child_Expression()
        {
            var scriptImportFromNode = Node<ScriptImportFromNode>.Empty;
            var scriptImportNode = Node<ScriptImportNode>.Empty;
            var scriptExtendNode = Node<ScriptExtendNode>.Empty;

            Node.SetChildren(
                scriptImportFromNode,
                scriptImportNode,
                scriptExtendNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);

            Assert.AreEqual(scriptExtendNode, Node.ChildNodes.First());
            Assert.AreEqual(scriptImportNode, Node.ChildNodes.Skip(1).First());
            Assert.AreEqual(scriptImportFromNode, Node.ChildNodes.Skip(2).First());
        }
    }
}
