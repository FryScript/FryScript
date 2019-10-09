using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Statements
{
    [TestClass]
    public class ForLoopTests : IntegrationTestBase
    {
        [TestMethod]
        public void For_Loop_Expression()
        {
            Eval("items = [];");
            Eval(@"
            for(var i = 0; i < 10; i++) 
                items.add(i);
            ");

            var items = Eval("items;");

            Assert.AreEqual(10, items.count);
        }

        [TestMethod]
        public void For_Loop_Statement()
        {
            Eval("items = [];");
            Eval(@"
            for(var i = 0; i < 10; i++) 
            { 
                var label = ""label "" + i.toString(); 
                items.add(label);
            }");

            var items = Eval("items;");

            Assert.AreEqual(10, items.count);
            for (var i = 0; i < items.count; i++)
            {
                Assert.AreEqual($"label {i}", items[i]);
            }
        }

        [TestMethod]
        public void For_Loop_Break()
        {
            Eval("items = [];");

            Eval(@"
            for(var i = 0; i < 1000; i++){
                break;

                items.add(i);
            }");

            var items = Eval("items;");

            Assert.AreEqual(0, items.count);
        }

        [TestMethod]
        public void For_Loop_Continue()
        {
            Eval("items = [];");

            Eval(@"
            for(var i = 0; i < 1000; i++){
                items.add(1);

                continue;

                items.add(i);
            }");

            var items = Eval("items;");

            Assert.AreEqual(1000, items.count);
        }

        [TestMethod]
        public void For_Loop_Closure_Scope()
        {
            Eval("items = [];");

            Eval(@"
            for(var i = 0; i < 5; i++){
                items.add(() => i);
            }");

            var items = Eval("items;");

            for (var i = 0; i < items.count; i++)
            {
                var itemFunc = items[i];
                Assert.AreEqual(items.count, itemFunc());
            }
        }
    }
}