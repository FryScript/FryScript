using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class EvalIsTests : IntegrationTestBase
    {
        [TestMethod]
        public void Script_A_Is_Script_B()
        {
            Eval("@import \"Scripts/baseScript\" as a;");
            Eval("@import \"Scripts/baseScript\" as b;");

            var result = Eval("a is b;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Script_B_Is_Not_Script_A()
        {
            Eval("@import \"Scripts/extendingScript\" as a;");
            Eval("@import \"Scripts/baseScript\" as b;");

            var result = Eval("b is a;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Null_Is_Other()
        {
            var result = Eval("null is 100;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Other_Is_Null()
        {
            var result = Eval("true is null;");

            Assert.IsFalse(result); 
        }

        [TestMethod]
        public void Null_Is_Null()
        {
            var result = Eval("null is null;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Int_Is_Int()
        {
            var result = Eval("100 is 900;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Float_Is_Float()
        {
            var result = Eval("1.0 is 2.0;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void String_Is_String()
        {
            var result = Eval("\"a\" is \"b\";");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Bool_Is_Bool()
        {
            var result = Eval("true is false;");

            Assert.IsTrue(result);
        }
    }
}
