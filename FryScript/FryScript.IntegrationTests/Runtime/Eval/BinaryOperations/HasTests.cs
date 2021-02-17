using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class HasTests : IntegrationTestBase
    {
        [TestMethod]
        public void Null_Has_Member()
        {
            Assert.IsFalse(Eval("null has member;"));
        }

        [TestMethod]
        public void Has_Member_True()
        {
            Eval("this.member = 90;");

            Assert.IsTrue(Eval("this has member;"));
        }

        [TestMethod]
        public void Has_Member_False()
        {
            Assert.IsFalse(Eval("this has member;"));
        }

        [TestMethod]
        public void Has_Member_Obj_Method()
        {
            Eval("this.member = true;");

            Assert.IsTrue(Eval("this.hasMember(\"member\");"));
        }

        [TestMethod]
        public void Int_Has_ToString()
        {
            Assert.IsTrue(Eval("0.hasMember(\"toString\");"));
        }
    }
}