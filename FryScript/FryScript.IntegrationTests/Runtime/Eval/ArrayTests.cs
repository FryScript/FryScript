using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class ArrayTests : IntegrationTestBase
    {
        [TestMethod]
        public void Empty_Array()
        {
            Assert.IsInstanceOfType(Eval("[];"), typeof(ScriptArray));
        }

        [TestMethod]
        public void Populated_Array()
        {
            var result = Eval("[1,2,3,4];") as ScriptArray;

            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void Get_Array_Index()
        {
            Eval("array = [1,2,3,4];");

            Assert.AreEqual(1, Eval("array[0];"));
            Assert.AreEqual(2, Eval("array[1];"));
            Assert.AreEqual(3, Eval("array[2];"));
            Assert.AreEqual(4, Eval("array[3];"));
        }

        [TestMethod]
        public void Set_Array_Tests()
        {
            var result = Eval("instance = [null, null];");
            Eval("instance[0] = true;");
            Eval("instance[1] = false;");
            
            Assert.IsTrue(result[0]);
            Assert.IsFalse(result[1]);
        }
    }
}