using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class BooleanOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bool_Add()
        {
            Eval("true + false;");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bol_subtract()
        {
            Eval("false - true;");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bool_Multiply()
        {
            Eval("true * true;");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bool_Divide()
        {
            Eval("true / false;");
        }

        [TestMethod]
        public void Bool_Equal()
        {
            var result = Eval("true == true;");

            Assert.AreEqual(true == true, result);
        }

        [TestMethod]
        public void Bool_Not_Equal()
        {
            var result = Eval("true != false;");

            Assert.AreEqual(true != false, result);
        }
    }
}
