using FryScript.Binders;
using FryScript.Compilation;
using FryScript.Debugging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FryScript.UnitTests
{
    public static class TestDebugger
    {
        private static StreamWriter _writer;

        static TestDebugger()
        {
            var path = Path.Combine("C:\\", "Source Control", "debug.txt");
            _writer = new StreamWriter(path, true);
        }

        public static void Hook(DebugContext context)
        {
            if (context.DebugEvent == DebugEvent.Statement)
            {
                _writer.WriteLine(string.Format("Script {0} {1}:{2}", context.Name, context.Line, context.Column));
                _writer.WriteLine("Variables:");

                foreach (var variable in context.Variables)
                {
                    _writer.WriteLine($"{variable.Name} = {(variable.Value as ScriptObject)?.GetScriptType() ?? variable.Value}");
                }

                _writer.WriteLine();
            }
        }
    }

    [TestClass]
    public class ScriptEngineEvalTests
    {
        private ScriptEngine _scriptEngine;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            BinderCache.Current = new BinderCache();
            _scriptEngine = new ScriptEngine
            {
                DetailedExceptions = false
            };
        }

        [TestMethod]
        public void EvalBlockStatementScopeTest()
        {
            var obj = Eval("{var test = 100;} test;");
            Assert.IsNull(obj);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void EvalMultipleProtoTest()
        {
           Eval("@proto{} @proto{}");
        }

        [TestMethod]
        public void EvalProtoTest()
        {
           var obj = Eval("@proto{ this.protoMember = true; }");
           Assert.IsTrue(obj.protoMember);
        }

        private dynamic Eval(string script)
        {
            var curMethod = new StackTrace().GetFrames().Skip(1).First().GetMethod().Name;
            return _scriptEngine.Eval(curMethod, script);
        }

        [Ignore]
        [TestMethod]
        public void EvalIfStatementAwaitConditionTrueTest()
        {
            var obj = Eval(@"

var f1 = fibre() => true;
var f2 = fibre() => true;

var f3 = fibre() => {
    if(await f1() && await f2())
        yield return true;
};
");
            Assert.IsTrue(obj().execute());
        }

        [Ignore]
        [TestMethod]
        public void EvalIfStatementAwaitConditionFalseTest()
        {
            var obj = Eval(@"

var f1 = fibre() => true;
var f2 = fibre() => false;

var f3 = fibre() => {
    if(await f1() && await f2()) {
    }
    else {
        yield return false;
    }
};
");
            Assert.IsFalse(obj().execute());
        }

        [Ignore]
        [TestMethod]
        public void EvalElseIfStatementAwaitConditionTrueTest()
        {
            var obj = Eval(@"

var f1 = fibre() => true;
var f2 = fibre() => false;

var f3 = fibre() => {
    if(await f1() && await f2()) {
    }
    else if (await f1() && await f1()) {
        yield return true;
    }
};
");
            Assert.IsTrue(obj().execute());
        }

        [Ignore]
        [TestMethod]
        public void EvalElseIfStatementAwaitConditionFalseTest()
        {
            var obj = Eval(@"

var f1 = fibre() => true;
var f2 = fibre() => false;

var f3 = fibre() => {
    if(await f1() && await f2()) {
    }
    else if (await f1() && await f2()) {
        yield return true;
    } else {
        yield return false;
    }
};
");
            Assert.IsFalse(obj().execute());
        }

        [Ignore]
        [TestMethod]
        public void EvalConditionalAssignTrueAwaitNotCalledTest()
        {
            var obj = Eval(@"

f1Called = false;

f1 = fibre() => {
    yield return f1Called = true;
};

conditional = fibre() => {
    var x = 10;
    yield return x ?= await f1(); 
};

this;
");
            obj.conditional().execute();

            Assert.IsFalse(obj.f1Called);
        }

        [Ignore]
        [TestMethod]
        public void EvalConditionalAssignFalseAwaitCalledTest()
        {
            var obj = Eval(@"

f1Called = false;

f1 = fibre() => {
    yield return f1Called = true;
};

conditional = fibre() => {
    var x = null;
    yield return x ?= await f1(); 
};

this;
");
            obj.conditional().execute();

            Assert.IsTrue(obj.f1Called);
        }

                [TestMethod]
        public void EvalFibreExtendTest()
        {
            var obj = Eval(@"
 f1 = fibre() => {
    yield 1;
    yield 2;
    yield return 3;
};

f1 extend fibre() => {
    var baseFc = base();

    while(!baseFc.completed)
        yield baseFc.resume();

    yield 4;
    yield 5;
    yield return 6;
};

fc = f1();
values = [fc.resume(), fc.resume(), fc.resume(), fc.resume(), fc.resume(), fc.resume()];
");
            Assert.AreEqual(6, obj.count);
            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(3, obj[2]);
            Assert.AreEqual(4, obj[3]);
            Assert.AreEqual(5, obj[4]);
            Assert.AreEqual(6, obj[5]);
        }

        [TestMethod]
        public void EvalFibreExtendDoesNotShareBaseBetweenFibresExtendedWithinaFibre()
        {
            var obj = Eval(@"

var f = fibre() => {
    var base1 = fibre() => ""base1"";
    var base2 = fibre() => ""base2"";

    base1 extend fibre() => await base();
    base2 extend fibre() => await base();

    yield return {
        e1: base1,
        e2: base2
    };
};

f().resume();
");

            Assert.AreEqual("base1", obj.e1().execute());
            Assert.AreEqual("base2", obj.e2().execute());
        }
    }
}
