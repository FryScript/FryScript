using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FibreContextTests : IntegrationTestBase
    {
        [TestMethod]
        public void Invoked_Fibre_Returns_Fibre_Context()
        {
            Eval("var f = fibre() => {};");

            var result = Eval("f();");

            Assert.IsInstanceOfType(result, typeof(ScriptFibreContext));
        }

        [TestMethod]
        public void Yield_Without_Result()
        {
            Eval(@"
            var f = fibre () => {
                yield;
                yield;
                yield;
            };
            ");

            var fc = Eval("f();") as ScriptFibreContext;

            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);

            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);

            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);
        }

        [TestMethod]
        public void Yield_With_Result()
        {
            Eval(@"
            var f = fibre () => {
                yield 1;
                yield 2;
                yield 3;
            };

            var fc = f();

            var yield1 = fc.resume();
            var yield2 = fc.resume();
            var yield3 = fc.resume();
            ");

            Assert.AreEqual(1, Eval("yield1;"));
            Assert.AreEqual(2, Eval("yield2;"));
            Assert.AreEqual(3, Eval("yield3;"));
        }

        [TestMethod]
        public void Yield_Implicit_Return_On_Last_Yield()
        {
            Eval(@"
            var f = fibre () => {
                yield 1;
                yield 2;
            };
            ");

            var context = Eval("f();") as ScriptFibreContext;

            context.Resume();

            var result = context.Resume();

            Assert.AreEqual(2, result);
            Assert.IsTrue(context.Completed);
        }

        [TestMethod]
        public void Yield_States()
        {
            Eval(@"
            var f = fibre () => {
                yield 1;
                yield 2;
            };

            var fc = f();
            ");

            var context = Eval("fc;") as ScriptFibreContext;

            Assert.IsTrue(context.Pending);
            Assert.IsFalse(context.Running);
            Assert.IsFalse(context.Completed);

            context.Resume();

            Assert.IsFalse(context.Pending);
            Assert.IsTrue(context.Running);
            Assert.IsFalse(context.Completed);

            context.Resume();

            Assert.IsFalse(context.Pending);
            Assert.IsFalse(context.Running);
            Assert.IsTrue(context.Completed);
        }

        [TestMethod]
        public void Yield_Return_Without_Completes_Fibre_Context()
        {
            Eval(@"
            var f = fibre () => {
                yield return;
                yield;
            };
            ");

            var fc = Eval("f();") as ScriptFibreContext;

            fc.Resume();
            Assert.IsTrue(fc.Completed);
        }

        [TestMethod]
        public void Yield_Return_With_Result_Complets_Fibre_Context()
        {
            Eval(@"
            var f = fibre () => {
                yield return ""complete"";
                yield;
            };
            ");

            var fc = Eval("f();") as ScriptFibreContext;

            fc.Resume();

            Assert.AreEqual("complete", fc.Result);
            Assert.IsTrue(fc.Completed);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Yield_Invalid_Context()
        {
            Eval("yield;");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Yield_Return_Invalid_Context()
        {
            Eval("yield return;");
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void Resume_Completed_Context()
        {
            Eval(@"
            var f = fibre() => {yield;};
            ");

            var fc = Eval("f();") as ScriptFibreContext;

            fc.Resume();
            fc.Resume();

            Assert.IsTrue(fc.Completed);

            fc.Resume();
        }

        [TestMethod]
        public void Implicit_Yield_Return_At_Fibre_Body_End()
        {
            Eval("counter = { count: 0 };");

            Eval(@"
            var f = fibre() => {
                var count = 2;
                for(var i = 0; i < count; i++) {
                    counter.count++;
                }
            };
            ");

            var counter = Eval("counter;");
            var f = Eval("f;");
            var fc = f() as ScriptFibreContext;

            fc.Resume();

            Assert.IsTrue(fc.Completed);
            Assert.IsFalse(fc.HasResult);
        }
    }
}