using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class FloatPrimitiveTests : IntegrationTestBase
    {
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
    }
}