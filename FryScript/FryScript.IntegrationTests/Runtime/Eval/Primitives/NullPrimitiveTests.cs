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
    }
}