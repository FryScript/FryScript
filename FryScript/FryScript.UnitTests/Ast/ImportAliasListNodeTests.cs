using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ImportAliasListNodeTests : AstNodeTestBase<ImportAliasListNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Not_Implemented()
        {
            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void Get_Expression_With_Scope_And_Object()
        {
            var obj = Substitute.For<IScriptObject>();
            var expectedImport1 = Expression.Constant(new object());
            var importAliasNode1 = Node<ImportAliasNode>.Empty;
            importAliasNode1.GetExpression(Scope, obj).Returns(expectedImport1);

            var expectedImport2 = Expression.Constant(new object());
            var importAliasNode2 = Node<ImportAliasNode>.Empty;
            importAliasNode2.GetExpression(Scope, obj).Returns(expectedImport2);

            Node.SetChildren(importAliasNode1, importAliasNode2);

            var result = Node.GetExpression(Scope, obj) as BlockExpression;

            Assert.AreEqual(typeof(object), result.Type);
            Assert.AreEqual(expectedImport1, result.Expressions[0]);
            Assert.AreEqual(expectedImport2, result.Expressions[1]);
        }
    }
}
