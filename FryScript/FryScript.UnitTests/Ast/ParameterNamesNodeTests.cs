using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Collections.Generic;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ParameterNamesNodeTests : AstNodeTestBase<ParameterNamesNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Not_Implemented()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeclareParameters_Null_Scope()
        {
            Node.DeclareParameters(null);
        }

        [TestMethod]
        public void DeclareParameters_Calls_Declare_Parameters_With_Identifiers()
        {
            var expectedNames = new List<IdentifierNode>
            {
                Node<IdentifierNode>.Empty
            };

            Node.Configure()
                .DeclareParameters(Scope, Arg.Is<List<IdentifierNode>>(i => i.Count == 0))
                .Returns(expectedNames);

            Node.DeclareParameters(Scope);

            expectedNames[0].Received().CreateIdentifier(Scope);
        }

        [TestMethod]
        public void DeclareParameters_Single_Identifier()
        {
            var childNode = Node<IdentifierNode>.Empty;
            Node.SetChildren(childNode);

            var exprs = new List<IdentifierNode>();

            Node.DeclareParameters(Scope, exprs);

            Assert.AreEqual(1, exprs.Count);
            Assert.AreEqual(childNode, exprs[0]);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            Assert.Fail();
        }
    }
}
