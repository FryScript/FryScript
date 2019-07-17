using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class FloatOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void Foat_Add()
        {
            var result = Eval("10.0 + 4.0;");

            Assert.AreEqual(10.0f + 4.0f, result);
        }

        [TestMethod]
        public void Foat_Subtract()
        {
            var result = Eval("3.5 - 1.25;");

            Assert.AreEqual(3.5f - 1.25f, result);
        }

        [TestMethod]
        public void Foat_Multiply()
        {
            var result = Eval("2.25 * 10.5;");

            Assert.AreEqual(2.25f * 10.5f, result);
        }

        [TestMethod]
        public void Foat_Divide()
        {
            var result = Eval("100.0 / 0.25;");

            Assert.AreEqual(100.0f / 0.25f, result);
        }

        [TestMethod]
        public void Foat_Equal()
        {
            var result = Eval("5.0 == 5.0;");

            Assert.AreEqual(5.0f == 5.0f, result);
        }

        [TestMethod]
        public void Foat_Not_Equal()
        {
            var result = Eval("2.5 != 25.0;");

            Assert.AreEqual(2.5f != 25.0f, result);
        }

        [TestMethod]
        public void Foat_Less_Than()
        {
            var result = Eval("0.8 < 0.9;");

            Assert.AreEqual(0.8f < 0.9f, result);
        }

        [TestMethod]
        public void Foat_Less_Than_Or_Equal()
        {
            var result = Eval("0.75 <= 0.75;");

            Assert.AreEqual(0.75f <= 0.75f, result);
        }

        [TestMethod]
        public void Foat_Greater_Than()
        {
            var result = Eval("1.0 > 0.1;");

            Assert.AreEqual(1.0f > 0.1f, result);
        }

        [TestMethod]
        public void Foat_Greater_Than_Or_Equal()
        {
            var result = Eval("99.9 >= 99.9;");

            Assert.AreEqual(99.9f >= 99.9f, result);
        }
    }
}
