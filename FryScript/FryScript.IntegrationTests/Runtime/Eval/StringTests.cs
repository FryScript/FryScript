using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Expressions
{
    [TestClass]
    public class StringTests : IntegrationTestBase
    {
        [TestMethod]
        public void Interpolation_Single_Expression()
        {
            Eval("var greeting = \"hello\";");

            var result = Eval("\"Say @{greeting}\";");

            Assert.AreEqual("Say hello", result);
        }

        [TestMethod]
        public void Interpolation_Multiple_Expressions()
        {
            Eval("var greeting = \"Hello\";");
            Eval("var name=\"Leela\";");

            var result = Eval("\"@{greeting} @{name}\";");
        
            Assert.AreEqual("Hello Leela", result);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Interpolation_No_Expressions()
        {
            Eval("\"@{}\";");
        }
    }
}