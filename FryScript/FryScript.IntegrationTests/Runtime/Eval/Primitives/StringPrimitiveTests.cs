using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class StringPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_String()
        {
            Eval("@import \"string\" as string;");

            var result = Eval("new string();");

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Cast_Bool_To_String()
        {
            Eval("@import \"string\" as string;");

            var result = Eval("string(false);");

            Assert.AreEqual("False", result);
        }

        [TestMethod]
        public void Cast_Float_To_String()
        {
            Eval("@import \"string\" as string;");

            var result = Eval("string(99.99);");

            Assert.AreEqual("99.99", result);
        }

        [TestMethod]
        public void Cast_Int_To_String()
        {
            Eval("@import \"string\" as string;");

            var result = Eval("string(42);");

            Assert.AreEqual("42", result);
        }
    }
}