using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class AwaitTests : IntegrationTestBase
    {
        [TestMethod]
        public void Fibre_Awaits_Other_Fibre()
        {
            Eval("var f1 = fibre() => \"I awaited!\";");
            Eval("var f2 = fibre() => await f1();");

            var result = Eval("f2();") as ScriptFibreContext;

            result.Resume(); // Yields control to f1 then stops execution
            Assert.AreEqual("I awaited!", result.Resume()); // Resumes control from f1
        }

        [TestMethod]
        public void Fibre_Yields_Awaited_Fibre()
        {
            Eval("var f1 = fibre() => \"I awaited again!\";");
            Eval(@"
            var f2 = fibre() => {
                yield await f1();
                yield ""Done!"";
            };
            ");

            var result = Eval("f2();") as ScriptFibreContext;

            result.Resume(); // Yields control to f1 then stops execution
            Assert.AreEqual("I awaited again!", result.Resume()); // Resumes control from f1 then yields it's result
            Assert.AreEqual("Done!", result.Resume());
        }

        [TestMethod]
        public void Fibre_Await_True_And_True()
        {
            Eval(@"
            var f1Called = false;
            var f2Called = false;

            var f1 = fibre() => f1Called = true;
            var f2 = fibre() => f2Called = true;

            var and = fibre () => await f1() && await f2();
            ");

            var and = Eval("and();") as ScriptFibreContext;

            and.Resume();

            Assert.IsFalse(and.HasResult);

            and.Resume();

            Assert.IsFalse(and.HasResult);

            and.Resume();

            Assert.IsTrue(and.Completed);
            Assert.IsTrue(and.HasResult);
            Assert.IsTrue((bool)and.Result);
            Assert.IsTrue(Eval("f1Called;"));
        }

        [TestMethod]
        public void Fibre_Await_True_Or_False_Left_Value()
        {
            Eval(@"
            var f2Called = false;

            var f1 = fibre() => true;
            var f2 = fibre() => f2Called = true;

            var or = fibre () => await f1() || await f2();
            ");

            var or = Eval("or();") as ScriptFibreContext;

            or.Resume();

            Assert.IsFalse(or.HasResult);

            or.Resume();

            Assert.IsTrue(or.Completed);
            Assert.IsTrue(or.HasResult);
            Assert.IsTrue((bool)or.Result);
            Assert.IsFalse(Eval("f2Called;"));
        }


        [TestMethod]
        public void Fibre_Await_False_Or_True_Right_Value()
        {
            Eval(@"
            var f1Called = false;
            var f2Called = false;

            var f1 = fibre() => {
                f1Called = true;
                false;
            };

            var f2 = fibre() => f2Called = true;

            var or = fibre () => await f1() || await f2();
            ");

            var or = Eval("or();") as ScriptFibreContext;

            or.Resume();

            Assert.IsFalse(or.HasResult);

            or.Resume();

            Assert.IsFalse(or.HasResult);

            or.Resume();

            Assert.IsTrue(or.Completed);
            Assert.IsTrue(or.HasResult);
            Assert.IsTrue((bool)or.Result);
            Assert.IsTrue(Eval("f1Called;"));
            Assert.IsTrue(Eval("f2Called;"));
        }
            
    }
}