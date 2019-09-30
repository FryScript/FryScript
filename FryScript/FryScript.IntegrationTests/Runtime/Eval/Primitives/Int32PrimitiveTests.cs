using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class Int32PrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void Int()
        {
            Assert.AreEqual(10, Eval("10;"));
        }

        [TestMethod]
        public void New_Int32()
        {
            Eval("@import \"int32\" as int;");

            var result = Eval("new int();");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Cast_Bool_To_Int()
        {
            Eval("@import \"int32\" as int;");

            var result = Eval("int(true);");

            Assert.AreEqual(1, result);
        }


        [TestMethod]
        public void Cast_Float_To_Int()
        {
            Eval("@import \"int32\" as int;");

            var result = Eval("int(45.5);");

            Assert.AreEqual(46, result);
        }

        [TestMethod]
        public void Cast_String_To_Int()
        {
            Eval("@import \"int32\" as int;");

            var result = Eval("int(\"23\");");

            Assert.AreEqual(23, result);
        }
    }
}