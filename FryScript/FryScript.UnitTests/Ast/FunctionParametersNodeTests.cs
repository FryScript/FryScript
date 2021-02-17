using FryScript.Ast;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FunctionParametersNodeTests : AstNodeTestBase<FunctionParametersNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Get_Expression_Throws_Not_Implemented()
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
        public void DeclareParameters_No_Parameters_Defined()
        {
            Node.DeclareParameters(Scope);

            Assert.AreEqual(0, Scope.GetAllowedMembers(true).Count());
        }

        [TestMethod]
        public void DeclareParameters_Handles_Parameter_Names_Node()
        {
            var paramNamesNode = Node<ParameterNamesNode>.Empty;

            Node.SetChildren(paramNamesNode);

            Node.DeclareParameters(Scope);

            paramNamesNode.Received().DeclareParameters(Scope);
        }

        [TestMethod]
        public void DeclareParameters_Handles_Params_Node()
        {
            var paramsNode = Node<ParamsNode>.Empty;

            Node.SetChildren(paramsNode);

            Node.DeclareParameters(Scope);

            var expectedParam = Scope.GetMemberExpression(Keywords.Params);

            Assert.AreEqual(Keywords.Params, expectedParam.Name);
        }

        [TestMethod]
        public void DeclareParameters_Handles_Identifier()
        {
            var identifierNode = Node<IdentifierNode>.Empty;

            Node.SetChildren(identifierNode);

            Node.DeclareParameters(Scope);

            identifierNode.Received().CreateIdentifier(Scope);
        }

        [TestMethod]
        public void Count_Returns_Child_Node_Count()
        {
            Node.SetChildren(null, null, null, null);

            Assert.AreEqual(4, Node.Count);
        }
    }
}
