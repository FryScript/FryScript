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

        [TestMethod]
        public void If_Then_Awaits_Condition()
        {
            Eval(@"
            var condition = fibre() => true;

            var f = fibre() => {
                if(await condition())
                    yield return true;
            };
            ");

            var f = Eval("f;");
            var fc = f() as ScriptFibreContext;

            fc.Execute();

            Assert.AreEqual(true, fc.Result);
        }

        [TestMethod]
        public void If_Then_Else_Awaits_Condition()
        {
            Eval(@"
            var condition = fibre() => false;

            var f = fibre() => {
                if(await condition())
                    yield return true;
                else
                    yield return false;
            };
            ");

            var f = Eval("f;");
            var fc = f() as ScriptFibreContext;

            fc.Execute();

            Assert.AreEqual(false, fc.Result);
        }
    }
}