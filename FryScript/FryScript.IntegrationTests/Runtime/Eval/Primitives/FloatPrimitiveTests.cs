using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class FloatPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void Float()
        {
            Assert.AreEqual(15.8f, Eval("15.8;"));
        }

        [TestMethod]
        public void New_Float()
        {
            Eval("@import \"single\" as float;");

            var result = Eval("new float();");

            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void Cast_Bool_To_Float()
        {
            Eval("@import \"single\" as float;");

            var result = Eval("float(true);");

            Assert.AreEqual(1.0, result);
        }

        [TestMethod]
        public void Cast_Int_To_Float()
        {
            Eval("@import \"single\" as float;");

            var result = Eval("float(500);");

            Assert.AreEqual(500.0f, result);
        }

        [TestMethod]
        public void Cast_String_To_Float()
        {
            Eval("@import \"single\" as float;");

            var result = Eval("float(\"99.9\");");

            Assert.AreEqual(99.9f, result);
        }

        [TestMethod]
        public void Zero_Float_Evaluates_To_False()
        {
            var result = Eval("0.0 && true;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Non_Zero_Float_Evaluates_To_False()
        {
            var result = Eval("45.5 && true;");

            Assert.IsTrue(result);
        }
    }
}