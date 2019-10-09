using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Statements
{
    [TestClass]
    public class WhileLoopTests : IntegrationTestBase
    {
        [TestMethod]
        public void While_Loop_Expression()
        {
            Eval("counter = 0;");

            Eval(@"
            while(counter < 10) 
                counter++;
            ");

            var result = Eval("counter;");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void While_Loop_Statement()
        {
            Eval("counter = 0;");
            Eval("items = [];");

            Eval(@"
            while(counter < 10) {
                items.add(counter);
                counter++;
            }
            ");

            var result = Eval("items;");

            Assert.AreEqual(10, result.count);
        }

        [TestMethod]
        public void While_Loop_Break()
        {
            Eval("counter = 0;");

            Eval(@"
            while(counter < 10) {
                break;
                counter++;
            }
            ");

            var result = Eval("counter;");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void While_Loop_Continue()
        {
            Eval("counter = 0;");

            Eval(@"
            while(counter < 10) {
                counter++;
                continue;
                counter++;
            }
            ");

            var result = Eval("counter;");

            Assert.AreEqual(10, result);
        }
    }
}