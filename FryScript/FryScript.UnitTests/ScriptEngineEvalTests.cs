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
    }
}
