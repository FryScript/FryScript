using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class LogicalAndTests : IntegrationTestBase
    {
        [DataTestMethod]
        [DataRow("true", "true", true)]
        [DataRow("false", "true", false)]
        [DataRow("true", "false", false)]
        [DataRow("false", "false", false)]
        public void Variables_Anded_Together(string literal1, string literal2, bool expected)
        {
            Eval($"var x = {literal1};");
            Eval($"var y = {literal2};");

            var result = Eval("x && y;");

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("{ member: true }", "{ member: true }", true)]
        [DataRow("{ member: true }", "{ member: false }", false)]
        [DataRow("{ member: false }", "{ member: true }", false)]
        [DataRow("{ member: false }", "{ member: false }", false)]
        public void Members_Anded_Together(string literal1, string literal2, bool expected)
        {
            Eval($"var x = {literal1};");
            Eval($"var y = {literal2};");

            var result = Eval("x.member && y.member;");

            Assert.AreEqual(expected, result);
        }
    }
}
