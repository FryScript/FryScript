using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class TernaryOperationTests : IntegrationTestBase
    {
        [TestMethod]
        public void Short_Form_Ternary_True()
        {
            // Will use the condition as the return value
            Assert.IsTrue(Eval("true ?: false;"));
        }

        [TestMethod]
        public void Short_Form_Ternary_False()
        {
            // Will use the false (right hand) return value
            Assert.IsTrue(Eval("false ?: true;"));
        }
    }
}