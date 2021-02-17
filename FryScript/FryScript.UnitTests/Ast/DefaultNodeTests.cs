using System;
using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class DefaultNodeTests
    {
        private DefaultNode _defaultNode;

        [TestInitialize]
        public void TestInitialize()
        {
            _defaultNode = new DefaultNode();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Throws_Not_Implemented_Exception()
        {
            _defaultNode.GetExpression(null);
        }
    }
}