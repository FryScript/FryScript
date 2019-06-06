using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
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

            Assert.IsTrue(result);
        }
    }
}
