using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class ObjectPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Object()
        {
            Eval("@import \"object\" as object;");

            var result = Eval("new object();") as ScriptObject;

            Assert.AreEqual(0, result.GetMembers().Count());
        }
    }
}