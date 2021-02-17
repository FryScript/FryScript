using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class IntOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void Int_Add()
        {
            var result = Eval("1 + 2;");

            Assert.AreEqual(1 + 2, result);
        }

        [TestMethod]
        public void Int_Subtract()
        {
            var result = Eval("2 - 1;");

            Assert.AreEqual(2 - 1, result);
        }

        [TestMethod]
        public void Int_Multiply()
        {
            var result = Eval("2 * 5;");

            Assert.AreEqual(2 * 5, result);
        }

        [TestMethod]
        public void Int_Divide()
        {
            var result = Eval("10 / 2;");

            Assert.AreEqual(10 / 2, result);
        }

        [TestMethod]
        public void Int_Equal()
        {
            var result = Eval("20 == 20;");

            Assert.AreEqual(20 == 20, result);
        }

        [TestMethod]
        public void Int_Not_Equal()
        {
            var result = Eval("20 != 20;");

            Assert.AreEqual(20 != 20, result);
        }

        [TestMethod]
        public void Int_Less_Than()
        {
            var result = Eval("10 < 100;");

            Assert.AreEqual(10 < 100, result);
        }

        [TestMethod]
        public void Int_Less_Than_Or_Equal()
        {
            var result = Eval("100 <= 100;");

            Assert.AreEqual(100 <= 100, result);
        }

        [TestMethod]
        public void Int_Greater_Than()
        {
            var result = Eval("200 > 100;");

            Assert.AreEqual(200 > 100, result);
        }

        [TestMethod]
        public void Int_Greater_Than_Or_Equal()
        {
            var result = Eval("200 >= 200;");

            Assert.AreEqual(200 >= 200, result);
        }
    }
}
