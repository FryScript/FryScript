using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptFibreContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullTarget()
        {
            new ScriptFibreContext(null);
        }

        [TestMethod]
        public void ResumeStateChange()
        {
            var factory = Factory(context =>
            {
                if(context.YieldState == 0)
                {
                    context.YieldState = 1;
                    return "second";
                }
                if(context.YieldState == 1)
                {
                    context.YieldState = ScriptFibreContext.CompletedState;
                    return "done";
                }
                context.YieldState = 0;
                return "first";
            });

            var fc = new ScriptFibreContext(factory);

            Assert.IsTrue(fc.Pending);
            Assert.AreEqual("first", fc.Resume());

            Assert.IsTrue(fc.Running);
            Assert.AreEqual("second", fc.Resume());

            Assert.IsTrue(fc.Running);
            Assert.AreEqual("done", fc.Resume());

            Assert.IsTrue(fc.Completed);
        }

        [TestMethod]
        public void ResumeHasResult()
        {
            var factory = Factory(context =>
            {
                return "value";
            });

            var fc = new ScriptFibreContext(factory);

            fc.Resume();

            Assert.IsTrue(fc.HasResult);
            Assert.AreEqual("value", fc.Result);
        }

        [TestMethod]
        public void ResumeHasResultFalse()
        {
            var factory = Factory(context =>
            {
                return ScriptFibreContext.NoResult;
            });

            var fc = new ScriptFibreContext(factory);

            fc.Resume();

            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void ResumeCompletedContext()
        {
            var fc = new ScriptFibreContext(c => c);
            fc.YieldState = ScriptFibreContext.CompletedState;
            fc.Resume();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwaitNullContext()
        {
            var fc = new ScriptFibreContext(c => c);
            fc.Await(null);
        }

        [TestMethod]
        public void AwaitOtherContext()
        {
            var factory1 = Factory(context =>
            {
                context.YieldState = ScriptFibreContext.CompletedState;
                return "done1";
            });

            var fc1 = new ScriptFibreContext(factory1);

            var factory2 = Factory(context =>
            {
                if(context.YieldState == 0)
                {
                    return "done2" + fc1.Result as string;
                }

                context.YieldState = 0;
                
                context.Await(fc1);
                return ScriptFibreContext.NoResult;
            });

            var fc2 = new ScriptFibreContext(factory2);

            fc2.Resume();
            Assert.IsFalse(fc2.HasResult);

            fc2.Resume();
            Assert.AreEqual("done2done1", fc2.Result);
        }

        private static Func<ScriptFibreContext, object> Factory(Func<ScriptFibreContext, object> factory)
        {
            return factory;
        }
    }
}
