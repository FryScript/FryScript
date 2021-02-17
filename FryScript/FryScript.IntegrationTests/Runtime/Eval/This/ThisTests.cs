using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.This
{
    [TestClass]
    public class ThisTests : IntegrationTestBase
    {
        [TestMethod]
        public void This()
        {
            Assert.IsInstanceOfType(Eval("this;"), typeof(IScriptObject));
        }

        [TestMethod]
        public void This_Get_Set_Member()
        {
            Eval("this.member = 100;");

            var result = Eval("this.member;");

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void This_Get_Set_Member_Implicit()
        {
            Eval("member = 50;");

            var result = Eval("member;");

            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void This_Get_Undefined_Member()
        {
            var result = Eval("this.unsetMember;");

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void This_Get_Undefuned_Member_Of_Unset_Member()
        {
            Eval("this.unsetMember.exceptionHere;");
        }

        [TestMethod]
        public void Get_Set_Index()
        {
            Eval("this[\"member\"] = 100;");

            var result = Eval("this[\"member\"];");

            Assert.AreEqual(100, result);
        }
    }
}