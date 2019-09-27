using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class BooleanPrimiiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Bool()
        {
            Eval("@import \"boolean\" as bool;");

            var result = Eval("new bool();");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Cast_Int_To_Bool()
        {
            Eval("@import \"boolean\" as bool;");

            var result = Eval("bool(0);");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Cast_Float_To_Bool()
        {
            Eval("@import \"boolean\" as bool;");

            var result = Eval("bool(1.0);");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Cast_String_To_Bool()
        {
            Eval("@import \"boolean\" as bool;");

            var result = Eval("bool(\"true\");");

            Assert.IsTrue(result);
        }
    }
}