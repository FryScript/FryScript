using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class NumericTests : IntegrationTestBase
    {
        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(100 + 10, Eval("100 + 10;"));
        }

        [TestMethod]
        public void Subtract()
        {
            Assert.AreEqual(90-900, Eval("90 - 900;"));
        }

        [TestMethod]
        public void Multiply()
        {
            Assert.AreEqual(10 * 10, Eval("10 * 10;"));
        }

        [TestMethod]
        public void Divide()
        {
            Assert.AreEqual(100 /2, Eval("100 / 2;"));
        }

        [TestMethod]
        public void Modulo()
        {
            Assert.AreEqual(10 % 4, Eval("10 % 4;"));
        }

        [TestMethod]
        public void Operator_Precedence()
        {
            Assert.AreEqual(2 + 2 / 4 * 8 - 10 + 1, Eval("2 + 2 / 4 * 8 - 10 + 1;"));
        }
    }
}