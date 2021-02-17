using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class VariableDeclarationTests : IntegrationTestBase
    {
        [TestMethod]
        public void Var()
        {
            Eval("var x;");

            Assert.IsTrue(Eval("this has x;"));
        }

        [TestMethod]
        public void Var_Assign()
        {
            Eval("var x = true;");

            Assert.IsTrue(Eval("this has x;"));
            Assert.IsTrue(Eval("x;"));
        }


        [TestMethod]
        public void As()
        {
            Eval("100 as x;");

            Assert.IsTrue(Eval("this has x;"));
            Assert.AreEqual(100, Eval("x;"));
        }
    }
}