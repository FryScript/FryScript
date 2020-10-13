using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class FryScriptExceptionTests
    {
        [TestMethod]
        public void Ctor_Sets_Message()
        {
            var ex = new FryScriptException("Test");

            Assert.AreEqual("Test", ex.Message);
        }

        [TestMethod]
        public void Ctor_Sets_Message_Inner_Exception_Name_Line_And_Column()
        {
            var innerEx = new Exception();
            var ex = new FryScriptException("Test", innerEx, "Name", 1, 2);

            Assert.AreEqual("Test", ex.Message);
            Assert.AreEqual(innerEx, ex.InnerException);
            Assert.AreEqual("Name", ex.Name);
            Assert.AreEqual(1, ex.Line);
            Assert.AreEqual(2, ex.Column);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatException_Null_Ex()
        {
            FryScriptException.FormatException(null, "name", 1, 2);
        }

        [TestMethod]
        public void FormatException_Wraps_Non_FryScriptException()
        {
            var expectedEx = new Exception();
            var ex = FryScriptException.FormatException(expectedEx, "Name", 1, 2) as FryScriptException;

            Assert.AreEqual(expectedEx, ex.InnerException);
            Assert.AreEqual("Name", ex.Name);
            Assert.AreEqual(1, ex.Line);
            Assert.AreEqual(2, ex.Column);
        }
    }
}