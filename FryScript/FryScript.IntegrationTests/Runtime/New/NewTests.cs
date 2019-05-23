using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.New
{
    [TestClass]
    public class NewTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Script_From_Import()
        {
            dynamic script = ScriptRuntime.New("Scripts/newTest");

            Assert.AreEqual("test1", script.property1);
            Assert.AreEqual(90, script.property2);
        }
    }
}
