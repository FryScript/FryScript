using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class NullPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void Null(){
            Assert.IsNull(Eval("null;"));
        }

        [TestMethod]
        public void Null_Evaluates_To_False()
        {
            var result = Eval("null && true;");

            Assert.IsFalse(result);
        }
    }
}