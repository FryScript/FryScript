using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class StringOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void String_Add()
        {
            var result = Eval("\"string 1\" + \"string 2\";");

            Assert.AreEqual("string 1" + "string 2", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void String_Subtract()
        {
            Eval("\"no\" - \"subtract\";");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void String_Multiply()
        {
            Eval("\"one\" * \"two\";");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void String_Divide()
        {
            Eval("\"ten\" / \"two\";");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void String_Modulo()
        {
            Eval("\"fifteen\" % \"five\";");
        }

        [TestMethod]
        public void String_Equal()
        {
            var result = Eval("\"string 1\" == \"string 2\";");

            Assert.AreEqual("string 1" == "string 2", result);
        }

        [TestMethod]
        public void String_Not_Equal()
        {
            var result = Eval("\"string a\" != \"string 2\";");

            Assert.AreEqual("string a" != "string 1", result);
        }

        [TestMethod]
        public void String_Interpolation()
        {
            Eval("name = \"Fry\";");

            var result = Eval("\"Hello, @{name}!\";");

            Assert.AreEqual("Hello, Fry!", result);
        }
    }
}
