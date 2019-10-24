using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BlockStatements
{
    [TestClass]
    public class IfStatementTests : IntegrationTestBase
    {
        [TestMethod]
        public void If()
        {
            Eval("var x;");
            Eval("if(true) x = \"test\";");
            Assert.AreEqual("test", Eval("x;"));
        }

        [TestMethod]
        public void If_Else()
        {
            Eval("var x;");
            Eval("if(false) x; else x=\"test\";");
            Assert.AreEqual("test", Eval("x;"));
        }

        [TestMethod]
        public void If_Else_If()
        {
            Eval("var x;");
            Eval("if(false) x; else if (true) x =\"test\";");
            Assert.AreEqual("test", Eval("x;"));
        }

        [TestMethod]
        public void If_Else_If_Else()
        {
            Eval("var x;");
            Eval("if(false) x; else if(false) x; else x=\"test\";");
            Assert.AreEqual("test", Eval("x;"));
        }
    }
}