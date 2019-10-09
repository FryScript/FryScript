using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Statements
{
    [TestClass]
    public class ForEachLoopTests : IntegrationTestBase
    {
        [TestMethod]
        public void ForEach_Loop_Expression()
        {
            Eval("var source = [0,1,2,3,4];");
            Eval("var items = [];");

            Eval("foreach(var item in source) items.add(item);");

            var result = Eval("items;");

            Assert.AreEqual(5, result.count);
        }

        [TestMethod]
        public void ForEach_Loop_Statement()
        {
            Eval("var source = [0,1,2,3,4];");
            Eval("var items = [];");

            Eval(@"
            foreach(var item in source) {
                items.add(item);
                items.add(item);
            }");

            var result = Eval("items;");

            Assert.AreEqual(10, result.count);
        }

        [TestMethod]
        public void ForEach_Loop_Break()
        {
            Eval("var source = [0,1,2,3,4];");
            Eval("var items = [];");

            Eval(@"
            foreach(var item in source) {
                break;
                items.add(item);
            }");

            var result = Eval("items;");

            Assert.AreEqual(0, result.count);
        }

        [TestMethod]
        public void ForEach_Loop_Continue()
        {
            Eval("var source = [0,1,2,3,4];");
            Eval("var items = [];");

            Eval(@"
            foreach(var item in source) {
                items.add(item);
                continue;
                items.add(item);
            }");

            var result = Eval("items;");

            Assert.AreEqual(5, result.count);
        }

        [TestMethod]
        public void ForEach_Loop_Non_Collection()
        {
            Eval("toLoop = true;");
            Eval("var result;");

            Eval(@"
            foreach(var item in toLoop)
                result = toLoop;");

            var result = Eval("result;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ForEach_Loop_Closure_Scope()
        {
            Eval("var source = [{},{},{},{},{}];");
            Eval("var items = [];");

            Eval(@"
            foreach(var item in source) {
                items.add(() => item);
            }");

            var source = Eval("source;");
            var items = Eval("items;");

            for (var i = 0; i < source.count; i++)
            {
                var currentSource = source[i];
                var currentItem = items[i]();

                Assert.AreEqual(currentSource, currentItem);
            }
        }
    }
}