using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class ObjectLiteralTests : IntegrationTestBase
    {
        [TestMethod]
        public void Object_Literal()
        {
            var result = Eval(@"
            {
                prop1: 100, 
                prop2: false, 
                prop3: ""test""
                };");

            Assert.AreEqual(100, result.prop1);
            Assert.IsFalse(result.prop2);
            Assert.AreEqual("test", result.prop3);
        }
    }
}