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

        [TestMethod]
        public void Format_Exception_Sets_Line_Value()
        {
            var ex = new FryScriptException("Test")
            {
                Line = null
            };

            FryScriptException.FormatException(ex, "Name", 1, 2);

            Assert.AreEqual(1, ex.Line);
        }

        [TestMethod]
        public void Format_Exception_Sets_Column_Value()
        {
            var ex = new FryScriptException("Test")
            {
                Column = null
            };

            FryScriptException.FormatException(ex, "Name", 1, 2);

            Assert.AreEqual(2, ex.Column);
        }

        [TestMethod]
        public void Throw_Rethrow_True_Returns_Input_Exception()
        {
            var expectedEx = new Exception();

            var ex = FryScriptException.Throw(null, expectedEx, "Name", 1, 2, true);

            Assert.AreEqual(expectedEx, ex);
        }

        [TestMethod]
        public void Throw_Data_As_String_Wraps_String_As_New_Exception()
        {
            var expectedEx = new Exception();
            var ex = FryScriptException.Throw("Exception", expectedEx, "Name", 1, 2, false) as FryScriptException;

            Assert.AreEqual("Exception", ex.Message);
            Assert.AreEqual(expectedEx, ex.InnerException);
            Assert.AreEqual("Name", ex.Name);
            Assert.AreEqual(1, ex.Line);
            Assert.AreEqual(2, ex.Column);
        }

        [TestMethod]
        public void Throw_Object_Dynamicc_Message_Property()
        {
            dynamic obj = new ScriptObject();
            obj.message = "Message Exception";

            var expectedEx = new Exception();

            try
            {
                FryScriptException.Throw(obj, expectedEx, "Name", 1, 2, false);
            }
            catch (FryScriptException ex)
            {
                Assert.AreEqual("Message Exception", ex.Message);
                Assert.AreEqual(obj, ex.ScriptData);
                Assert.AreEqual(expectedEx, ex.InnerException);
                Assert.AreEqual("Name", ex.Name);
                Assert.AreEqual(1, ex.Line);
                Assert.AreEqual(2, ex.Column);
            }
        }

        [TestMethod]
        public void GetCatchObject_Exception_Is_Not_A_FryScriptException_Returns_Message()
        {
            var ex = new Exception("Error");

            var result = FryScriptException.GetCatchObject(ex);

            Assert.AreEqual("Error", result);
        }

        [TestMethod]
        public void GetCatchObject_Returns_ScriptData()
        {
            var ex = new FryScriptException("Error")
            {
                ScriptData = new object()
            };

            var result = FryScriptException.GetCatchObject(ex);

            Assert.AreEqual(ex.ScriptData, result);
        }
    }
}