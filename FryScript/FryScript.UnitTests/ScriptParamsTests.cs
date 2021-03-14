using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptParamsTests
    {
        [TestMethod]
        public void GetCountTest()
        {
            dynamic param = new ScriptParams(100);

            Assert.AreEqual(1, param.count);
        }

        [TestMethod]
        public void GetIndexTest()
        {
            dynamic param = new ScriptParams(100, 200, 300);
            Assert.AreEqual(100, param[0]);
            Assert.AreEqual(200, param[1]);
            Assert.AreEqual(300, param[2]);
        }

        [TestMethod]
        [ExpectedException(typeof (FryScriptException))]
        public void GetIndexInvalidTest()
        {
            dynamic param = new ScriptParams();
            var result = param[false];
        }

        [TestMethod]
        public void GetIndexNamedTest()
        {
            dynamic param = new ScriptParams(new object());
            var result = param["count"];

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DynamicConvertToScriptParamsTest()
        {
            dynamic param = new ScriptParams();

            var converted = (ScriptParams) param;

            Assert.IsTrue(converted is ScriptParams);
        }

        [TestMethod]
        public void GetTypedIndexTest()
        {
            var param = new ScriptParams(100);
            Assert.AreEqual(100, param.Get<int>(0));
        }
    }
}
