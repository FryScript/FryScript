using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class LogicalOrTests : IntegrationTestBase
    {
        [DataTestMethod]
        [DataRow("true", "true", true)]
        [DataRow("false", "true", true)]
        [DataRow("true", "false", true)]
        [DataRow("false", "false", false)]
        public void Variables_Ored_Together(string literal1, string literal2, bool expected)
        {
            Eval($"var x = {literal1};");
            Eval($"var y = {literal2};");

            var result = Eval("x || y;");

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("{ member: true }", "{ member: true }", true)]
        [DataRow("{ member: true }", "{ member: false }", true)]
        [DataRow("{ member: false }", "{ member: true }", true)]
        [DataRow("{ member: false }", "{ member: false }", false)]
        public void Members_Ored_Together(string literal1, string literal2, bool expected)
        {
            Eval($"var x = {literal1};");
            Eval($"var y = {literal2};");

            var result = Eval("x.member || y.member;");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Value_Ored_With_Null_Member_Access_Does_Not_Fail_When_X_Is_True()
        {
            Eval($"var x = true;");
            Eval($"var y = null;");

            var result = Eval("x || y.member;");

            Assert.AreEqual(true, result);
        }
    }
}
