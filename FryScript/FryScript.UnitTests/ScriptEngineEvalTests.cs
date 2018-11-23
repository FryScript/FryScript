﻿using FryScript.Binders;
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

            //_scriptEngine.DebugHook = TestDebugger.Hook;
        }

        [TestMethod]
        public void EvalNullTest()
        {
            Assert.IsNull(Eval("null;"));
        }

        [TestMethod]
        public void EvalTrueTest()
        {
            Assert.IsTrue((bool)Eval("true;"));
        }

        [TestMethod]
        public void EvalFalseTest()
        {
            Assert.IsFalse((bool)Eval("false;"));
        }

        [TestMethod]
        public void EvalStringTest()
        {
            Assert.AreEqual("Test", Eval("\"Test\";"));
        }

        [TestMethod]
        public void EvalIntTest()
        {
            Assert.AreEqual(100, Eval("100;"));
        }

        [TestMethod]
        public void EvalFloatTest()
        {
            Assert.AreEqual(1.1f, Eval("1.1;"));
        }

        [TestMethod]
        public void EvalThisTest()
        {
            Assert.IsTrue(Eval("this;") is ScriptObject);
        }

        [TestMethod]
        public void EvalGetMemberTest()
        {
            var obj = Eval("this.member = false; this.member;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalGetMemberImplicitThisTest()
        {
            var obj = Eval("member = true; member;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalGetMemberUndefinedTest()
        {
            Eval("this.undefined;");
        }

        [TestMethod]
        public void EvalSetMemberTest()
        {
            var obj = Eval("this.member = 100; this;");
            Assert.AreEqual(100, obj.member);
        }

        [TestMethod]
        public void EvalSetMemberMultipleTest()
        {
            var obj = Eval("this.member = this.member2 = 100; this;");
            Assert.AreEqual(100, obj.member);
            Assert.AreEqual(100, obj.member2);
        }

        [TestMethod]
        public void EvalGetIndexTest()
        {
            var obj = Eval("this.test = 100; this[\"test\"];");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalSetIndexText()
        {
            var obj = Eval("this[\"test\"] = false; this.test;");
            Assert.AreEqual(false, obj);
        }

        [TestMethod]
        public void EvalMultipleSetGetIndexTest()
        {
            var obj = Eval("this[\"test1\"] = 1; this[\"test2\"] = 2; this[\"test1\"] + this[\"test2\"];");
            Assert.AreEqual(3, obj);
        }

        [TestMethod]
        public void EvalExtendTest()
        {
            _scriptEngine.Compile("base", "this.baseProp = true;");
            var obj = Eval("@extend \"base\";");
            Assert.IsTrue((bool)obj.baseProp);
        }

        [TestMethod]
        public void EvalInheritanceTest()
        {
            var base1 = _scriptEngine.Compile("base1", "this;");
            var base2 = _scriptEngine.Compile("base2", "@extend \"base1\";");
            var base3 = _scriptEngine.Compile("base3", "@extend \"base2\";");
            ScriptObject obj = Eval("@extend \"base3\";");

            Assert.IsTrue(obj.IsScript(base1));
            Assert.IsTrue(obj.IsScript(base2));
            Assert.IsTrue(obj.IsScript(base3));
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void EvalMultipleExtendTest()
        {
            Eval("@extend \"base\"; @extend \"base\";");
        }

        [TestMethod]
        public void EvalImportTest()
        {
            _scriptEngine.Compile("import", "this.isImported = true;");
            var obj = Eval("@import \"import\" as imported;");

            Assert.IsTrue(obj.isImported);
        }

        [TestMethod]
        public void EvalVariableDeclarationTest()
        {
            var obj = Eval("var test;");
            Assert.AreEqual(null, obj);
        }

        [TestMethod]
        public void EvalVariableDeclarationAssignTest()
        {
            var obj = Eval("var test = true;");
            Assert.AreEqual(true, obj);
        }

        [TestMethod]
        [ExpectedException(typeof (CompilerException))]
        public void EvalMultipleDeclarationTest()
        {
            Eval("var test = 100; var test = 200;");
        }

        [TestMethod]
        public void EvalAssignSingleMemberTest()
        {
            var obj = Eval("var test; test = 200;");
            Assert.AreEqual(200, obj);
        }

        [TestMethod]
        public void EvalAssignImplicitThisTest()
        {
            var obj = Eval("test = 100; this;");
            Assert.AreEqual(100, obj.test);
        }

        [TestMethod]
        public void EvalObjectLiteralTest()
        {
            var obj = Eval("{test : false};");
            Assert.AreEqual(false, obj.test);
        }

        [TestMethod]
        public void EvalObjectLiteralIsAssignedScriptTypeTest()
        {
            ScriptObject obj = Eval("{test: 100};");
            Assert.AreEqual(ScriptObject.ObjectName, obj.GetScriptType());
        }

        [TestMethod]
        public void EvalObjectLiteralUsesDifferentThisTest()
        {
            var obj = Eval("this.test = 100; this.sub = {test : 200}; this;");
            Assert.AreEqual(100, obj.test);
            Assert.AreEqual(200, obj.sub.test);
        }

        [TestMethod]
        public void EvalObjectLiteralHoistedInFibreDoesNotUseScriptThisTest()
        {
            var obj = Eval(@"

this.f1 = fibre() => {
    yield return {
        prop1: 100,
        prop2: 200
    };
};

this;
");
            var result = obj.f1().execute();

            Assert.IsFalse((obj as ScriptObject).HasMember("prop1"));
            Assert.IsFalse((obj as ScriptObject).HasMember("prop2"));

            Assert.IsTrue((result as ScriptObject).HasMember("prop1"));
            Assert.IsTrue((result as ScriptObject).HasMember("prop2"));

        }

        [TestMethod]
        public void EvalObjectScriptTypeIsAssignedTest()
        {
            ScriptObject obj = Eval("this;");

            Assert.AreNotEqual(ScriptObject.ObjectName, obj.GetScriptType());
        }

        [TestMethod]
        public void EvalScriptTypeIsAssignedTest()
        {
            ScriptObject obj = Eval("this;");

            Assert.IsNotNull(obj.GetScriptType());
            Assert.AreNotEqual(ScriptObject.ObjectName, obj.GetScriptType());
        }

        [TestMethod]
        public void EvalFunctionTest()
        {
            var obj = Eval("()=>{};");

            Assert.IsTrue(obj is ScriptFunction);
        }

        [TestMethod]
        public void EvalShorthandFunctionTest()
        {
            var obj = Eval("var x=100; ()=> x;");
            Assert.AreEqual(100, obj());
        }

        [TestMethod]
        public void EvalFunctionReturnsLastExpressionTest()
        {
            var obj = Eval("()=>{var x = 100; x;};");
            Assert.AreEqual(100, obj());
        }

        [TestMethod]
        public void EvalFunctionArgsTest()
        {
            var obj = Eval("a=>{a;};");
            Assert.AreEqual("test", obj("test"));
        }

        [TestMethod]
        public void EvalFunctionArgsShortHandTest()
        {
            var obj = Eval("a=>a;");
            Assert.AreEqual("test", obj("test"));
        }

        [TestMethod]
        public void EvalFunctionReturnExpressionTest()
        {
            var obj = Eval("a=>{return a;};");
            Assert.AreEqual(true, obj(true));
        }

        [TestMethod]
        public void EvalFunctionReturnNoExpressionTest()
        {
            var obj = Eval("a => {return;};");
            Assert.IsNull(obj());
        }

        [TestMethod]
        public void EvalFunctionParamsTest()
        {
            var obj = Eval("params=>{return params;};");

            ScriptParams result = obj(10, 20, 30);

            Assert.AreEqual(10, result[0]);
            Assert.AreEqual(20, result[1]);
            Assert.AreEqual(30, result[2]);
        }

        [TestMethod]
        public void EvalFunctionParamsReturnTest()
        {
            var obj = Eval("((params) => params[0] + params[1])(10, 20);");

            Assert.AreEqual(obj, 30);
        }

        [TestMethod]
        [ExpectedException(typeof (CompilerException))]
        public void EvalReturnOutsideOfFunctionInvalidTest()
        {
            _scriptEngine.Eval("return 100;");
        }

        [TestMethod]
        public void EvalNestedFunctionsReturnTest()
        {
            var obj = Eval(@"
this.func = ()=> {
    return {
        childFunc: ()=>{
            return 100;
        }
    };
};
this;   
            ");

            Assert.AreEqual(100, obj.func().childFunc());
        }

        [TestMethod]
        public void EvalFunctionInvokeTest()
        {
            var obj = Eval("var f = (x) => x; f(100);");

            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalMemberInvokeTest()
        {
            var obj = Eval("x = {func: () => 100}; x.func();");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalEmptyArrayTest()
        {
            var obj = Eval("[];");
            Assert.AreEqual(0, obj.count);
        }

        [TestMethod]
        public void EvalArrayTest()
        {
            var obj = Eval("[1,true,\"test\", 2.0];");

            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(true, obj[1]);
            Assert.AreEqual("test", obj[2]);
            Assert.AreEqual(2.0f, obj[3]);
        }

        [TestMethod]
        public void EvalArrayGetIndexTest()
        {
            var obj = Eval("var test = [1]; test[0];");

            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void EvalArraySetIndexTest()
        {
            var obj = Eval("var test = [0]; test[0] = 100; test[0];");

            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalArrayMemberAccessTest()
        {
            var obj = Eval("var test = [1]; test.member = false; test.member;");

            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalArrayCountTest()
        {
            var obj = Eval("var test = [1,2,3,4,5]; test.count;");

            Assert.AreEqual(5, obj);
        }

        [TestMethod]
        public void EvalMathExpressionIntTest()
        {
            var obj = Eval("2+2/4*8-10+1;");
            Assert.AreEqual(2 + 2 / 4 * 8 - 10 + 1, obj);
        }

        [TestMethod]
        public void EvalMathExpressionFloatTest()
        {
            var obj = Eval("5.0+2.5/4.6*3.2-10.5-90.2;");
            Assert.AreEqual(5.0f + 2.5f / 4.6f * 3.2f - 10.5f - 90.2f, obj);
        }

        [TestMethod]
        public void EvalRelationalExpressionTest()
        {
            var obj = Eval("false && false || true && false || true && true;");
            Assert.AreEqual(false && false || true && false || true && true, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalBlockStatementScopeTest()
        {
            Eval("{var test = 100;} test;");
        }

        [TestMethod]
        public void EvalIfStatementTrueTest()
        {
            var obj = Eval("var x; if(true) x=true; x;");
            Assert.AreEqual(true, obj);
        }

        [TestMethod]
        public void EvalIfStatementFalseTest()
        {
            var obj = Eval("var x; if(true == false) x=true;");
            Assert.AreEqual(null, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalIfStatementScopeTest()
        {
            Eval("if(true) var x = true; x;");
        }

        [TestMethod]
        public void EvalIfElseStatementTest()
        {
            var obj = Eval("var x; if(true == false) x = true; else x = false; x;");
            Assert.AreEqual(false, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalIfElseStatementScopeTest()
        {
            Eval("if(true == false) var x = true; else var x = false; x;");
        }

        [TestMethod]
        public void EvalIfElseStatementTrueTest()
        {
            var obj = Eval("if(true == true) 100; else 200;");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalIfElseStatementFalseTest()
        {
            var obj = Eval("if(true == false) 100; else 200;");
            Assert.AreEqual(200, obj);
        }

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

        [TestMethod]
        public void EvalNotBoolTest()
        {
            var obj = Eval("!false;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIntPrePlusPlusTest()
        {
            var obj = Eval("var x = 0; ++x;");
            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void EvalIntPrePlusPlusMemberTest()
        {
            var obj = Eval("this.x = 0; ++x;");
            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void EvalIntPrePlusPlusIndexTest()
        {
            var obj = Eval("this.x = 0; ++this[\"x\"];");
            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void EvalIntPreMinusMinusTest()
        {
            var obj = Eval("var x = 0; --x;");
            Assert.AreEqual(-1, obj);
        }

        [TestMethod]
        public void EvalIntPreMinusMinusMemberTest()
        {
            var obj = Eval("this.x = 0; --x;");
            Assert.AreEqual(-1, obj);
        }

        [TestMethod]
        public void EvalIntPreMinusMinusIndexTest()
        {
            var obj = Eval("this.x = 0; --this[\"x\"];");
            Assert.AreEqual(-1, obj);
        }

        [TestMethod]
        public void EvalIntPostPlusPlusTest()
        {
            var obj = Eval("var x = 0; x++;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPostPlusPlusMemberTest()
        {
            var obj = Eval("this.x = 0; x++;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPostPlusPlusIndexTest()
        {
            var obj = Eval("this.x = 0; this[\"x\"]++;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPostMinusMinusTest()
        {
            var obj = Eval("var x = 0; x--;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPostMinusMinusMemberTest()
        {
            var obj = Eval("this.x = 0; x--;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPostMinusMinusIndexTest()
        {
            var obj = Eval("this.x = 0; this[\"x\"]--;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalIntPlusEqualTest()
        {
            var obj = Eval("x = 100; x += 10; x;");
            Assert.AreEqual(110, obj);
        }

        [TestMethod]
        public void EvalIntMinusEqualTest()
        {
            var obj = Eval("x = 100; x -= 10; x;");
            Assert.AreEqual(90, obj);
        }

        [TestMethod]
        public void EvalIntMultiplyEqualTest()
        {
            var obj = Eval("x = 100; x *= 10; x;");
            Assert.AreEqual(1000, obj);
        }

        [TestMethod]
        public void EvalIntDivideEqualTest()
        {
            var obj = Eval("x = 100; x /= 10; x;");
            Assert.AreEqual(10, obj);
        }

        [TestMethod]
        public void EvalIntModuloEqualTest()
        {
            var obj = Eval("x = 500; x %= 10; x;");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void EvalConditionalAssignTrueTest()
        {
            var obj = Eval("x = 100; x ?= 200; x;");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalConditionalAssignFalseTest()
        {
            var obj = Eval("x = null; x ?= 200; x;");
            Assert.AreEqual(200, obj);
        }

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
        public void EvalForLoopTest()
        {
            var obj = Eval("var x = 0; for(var i = 0; i < 10; i++) x += 1; x;");

            Assert.AreEqual(10, obj);
        }

        [TestMethod]
        public void EvalForLoopBreakTest()
        {
            var obj = Eval("var x = -1; for(var i = 0; i < 10; i++){ if(i >= 0)break; x = i;} x; for(var i = 0; i < 1; i++){}; x;");

            Assert.AreEqual(-1, obj);
        }

        [TestMethod]
        public void EvalForLoopContinueTest()
        {
            var obj = Eval("var x = -1; for(var i = 0; i < 10; i++){ if(i >= 0)continue; x = i;} x;");

            Assert.AreEqual(-1, obj);
        }

        [TestMethod]
        public void EvalNewTest()
        {
            var obj = Eval("this.x = 10; this.y = false; this.instance = () => new this(); this;");
            var instance = obj.instance();
            instance.x = 20;
            instance.y = true;

            Assert.AreNotEqual(obj, instance);
            Assert.AreEqual(20, instance.x);
            Assert.AreEqual(true, instance.y);
            Assert.AreEqual(10, obj.x);
            Assert.AreEqual(false, obj.y);
        }

        [TestMethod]
        public void EvalCtorTest()
        {
            var obj =
                Eval(
                    "this.x = 10; this.y = false; this.ctor = () => this.z = true; this.instance = () => new this(); this;");

            var instance = obj.instance();

            Assert.AreEqual(true, instance.z);
        }

        [TestMethod]
        public void EvalWhileTest()
        {
            var obj = Eval("var x = 0; while(x < 10) x++; this.x = x; this;");

            Assert.AreEqual(10, obj.x);
        }

        [TestMethod]
        public void EvalWhileBreakTest()
        {
            var obj = Eval("var x = 0; while(x < 10) {if(x == 5) break; x++;} this.x = x; this;");

            Assert.AreEqual(5, obj.x);
        }

        [TestMethod]
        public void EvalWhileContinueTest()
        {
            var obj = Eval("var x = 0; this.x = 0; while(x < 10) {x++; if(x > 5) continue; this.x++;}  this;");

            Assert.AreEqual(5, obj.x);
        }

        [TestMethod]
        public void EvalForEachTest()
        {
            var obj = Eval("var items = [1,2,3,4]; var items2 = []; foreach(var i in items) items2.add(i); items2;");

            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(3, obj[2]);
            Assert.AreEqual(4, obj[3]);
            Assert.AreEqual(4, obj.count);
        }

        [TestMethod]
        public void EvalForEachNonCollectionTest()
        {
            var obj = Eval("var item = false; var items2 = []; foreach(var i in item) items2.add(i); items2;");

            Assert.AreEqual(false, obj[0]);
        }

        [TestMethod]
        public void EvalForEachBreakTest()
        {
            var obj = Eval("var items = [1,2,3,4]; var items2 = []; foreach(var i in items){ items2.add(i); if(i == 2) break;} items2;");

            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(2, obj.count);
        }

        [TestMethod]
        public void EvalForEachContinueTest()
        {
            var obj = Eval("var items = [1,2,3,4]; var items2 = []; foreach(var i in items){ if(i > 2) continue; items2.add(i); } items2;");

            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(2, obj.count);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void EvalInvalidContinueContextTest()
        {
            Eval("var items = [1]; foreach(var i in items) { (() => { continue;} )(); }");
        }

        [TestMethod]
        public void EvalFunctionExtendTest()
        {
            var obj = Eval("var f = () => {}; f extend () => {this.x = \"extended\"; this;}; f;");

            var r = obj();

            Assert.AreEqual("extended", r.x);
        }

        [TestMethod]
        public void EvalFunctionExtendBaseTest()
        {
            var obj = Eval("var f = () => this.x = \"extended\"; f extend () => {base(); this.y = \"extended\"; this;}; f;");

            var r = obj();

            Assert.AreEqual("extended", r.x);
            Assert.AreEqual("extended", r.y);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalFunctionExtendInvalidTest()
        {
            Eval("var o = {}; o extend () => {};");
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

        [TestMethod]
        public void EvalIsObjectObjectTest()
        {
            var obj = Eval("var x = {}; var y = {}; x is y;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIsNullObjectTest()
        {
            var obj = Eval("var x = {}; null is x;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalIsTypeObjectNullTest()
        {
            var obj = Eval("var x = {}; x is null;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalIsIntTest()
        {
            var obj = Eval("0 is 100;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIsFloatTest()
        {
            var obj = Eval("0.0 is 100.0;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIsBoolTest()
        {
            var obj = Eval("true is false;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIsStringTest()
        {
            var obj = Eval("\"\" is \"\";");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsExtendingTest()
        {
            _scriptEngine.Compile("base1", "this;");
            _scriptEngine.Compile("base2", "@extend \"base1\";");
            var obj = Eval("@import \"base1\" as base1; @import \"base2\" as base2; base2 extends base1;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsNonExtendingTest()
        {
            _scriptEngine.Compile("base1", "this;");
            _scriptEngine.Compile("base2", "@extend \"base1\";");
            var obj = Eval("@import \"base1\" as base1; @import \"base2\" as base2; base1 extends base2;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalExtendsNullObjectTest()
        {
            var obj = Eval("var x = {}; null extends x;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalExtendsObjectNullTest()
        {
            var obj = Eval("var x = {}; x extends null;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalExtendsObjectTypeTest()
        {
            var obj = Eval("this extends ({});");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsSelfTest()
        {
            var obj = Eval("this extends this;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsIntTest()
        {
            var obj = Eval("0 extends 0;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsFloatTest()
        {
            var obj = Eval("0.0 extends 0.0;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsBoolTest()
        {
            var obj = Eval("true extends false;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalExtendsStringTest()
        {
            var obj = Eval("\"\" extends \"\";");
            Assert.IsTrue(obj);
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

        [TestMethod]
        public void EvalHasTrueTest()
        {
            var obj = Eval("this.x = 100; this has x;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalIntHasTrueTest()
        {
            var obj = Eval("0 has toString;");

            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalHasFalseTest()
        {
            var obj = Eval("this has x;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalPrimitiveHasTrueTest()
        {
            var obj = Eval("0 has toString;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalPrimitiveHasFalseTest()
        {
            var obj = Eval("0 has invalid;");
            Assert.IsFalse(obj);
        }

        [TestMethod]
        public void EvalObjectEqualsTest()
        {
            var obj = Eval("var x = {}; x == x;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalObjectNotEqualsTest()
        {
            var obj = Eval("var x = {}; var y = {}; x != y;");
            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalTryCatchTest()
        {
            var obj = Eval("var x = {}; try{ x.undefined; }catch error{ x.undefined = 100;} x;");
            Assert.AreEqual(100, obj.undefined);
        }

        [TestMethod]
        public void EvalTryFinallyTest()
        {
            var obj = Eval("var x = {}; try{ x.member = 100; }finally{ x.member = 200;} x;");
            Assert.AreEqual(200, obj.member);
        }

        [TestMethod]
        public void EvalTryCatchFinallyTest()
        {
            var obj = Eval("var x = {}; try{ x.undefined; }catch error{ x.undefined = 10;}finally{ x.undefined *= 200;} x;");
            Assert.AreEqual(2000, obj.undefined);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException), "error")]
        public void EvalThrowStringTest()
        {
            Eval("throw \"error\";");
        }

        [TestMethod]
        public void EvalThrowObjectTest()
        {
            try
            {
                Eval("throw {message:\"error\"};");
            }
            catch(FryScriptException ex)
            {
                dynamic data = ex.ScriptData;
                Assert.AreEqual("error", data.message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalThrowAsExpression()
        {
            Eval("x = {y: null }; x.y ?: throw \"error\";");
        }

        [TestMethod]
        public void EvalCatchThrownObjectTest()
        {
            var obj = Eval("var x; try{ throw {test: true}; } catch error { x = error; } x;");
            Assert.IsTrue(obj.test);
        }

        [TestMethod]
        public void EvalCatchThrownNullTest()
        {
            var obj = Eval("var x; try{ throw null; } catch error { x = error; } x;");
            Assert.AreEqual(string.Empty, obj);
        }

        [TestMethod]
        public void EvalFormatStringTest()
        {
            var obj = Eval("this.name=\"test\"; \"@{name}\";");
            Assert.AreEqual("test", obj);
        }

        [TestMethod]
        public void EvalFormatStringMultipleExpressionsTest()
        {
            var obj = Eval("this.name=\"test\"; \"@{name} has a name of @{name}\";");
            Assert.AreEqual("test has a name of test", obj);
        }

        [TestMethod]
        public void EvalFormatStringMultipleDifferentExpressionsTest()
        {
            var obj = Eval("this.name=\"test\"; this.age=2; \"@{name} has an age of @{age}\";");
            Assert.AreEqual("test has an age of 2", obj);
        }

        [TestMethod]
        public void EvalBeginMemberReturnsFibreContextTest()
        {
            var obj = Eval("var x = {}; x.f = fibre() => {}; x.f();") as ScriptFibreContext;
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void EvalResumeYieldImplicitExpressionYieldReturnTest()
        {
            var obj = Eval(@"
value = 1;
f = fibre(i) => {
    this.value += i;
    yield;
    this.value += i;
    yield;
    this.value += i;
};

fc = f(2);
fc.resume();
fc.resume();
fc.resume();
this;");

            var fc = obj.fc as ScriptFibreContext;

            Assert.IsTrue(fc.Completed);
            Assert.IsFalse(fc.Running);
            Assert.IsTrue(fc.HasResult);
            Assert.AreEqual(7, obj.value);
        }

        [TestMethod]
        public void EvalResumeYieldHasResultFalseTest()
        {
            var obj = Eval("f = fibre() => {yield; yield; yield; yield;}; f();");

            var fc = obj as ScriptFibreContext;

            Assert.IsNull(fc.Resume());
            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);

            Assert.IsNull(fc.Resume());
            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);

            Assert.IsNull(fc.Resume());
            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);

            Assert.IsNull(fc.Resume());
            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);
        }

        [TestMethod]
        public void EvalResumeYieldResultTest()
        {
            var obj = Eval(@"
var f = fibre(i) => {
    yield i++;
    yield i++;
    yield i++;
    yield i ++;
};
var fc = f(0);

var obj = {
    pass1: null,
    pass2: null,
    pass3: null,
    pass4: null
};

obj.pass1 = fc.resume();
obj.pass2 = fc.resume();
obj.pass3 = fc.resume();
obj.pass4 = fc.resume();

obj;
");

            Assert.AreEqual(0, obj.pass1);
            Assert.AreEqual(1, obj.pass2);
            Assert.AreEqual(2, obj.pass3);
            Assert.AreEqual(3, obj.pass4);
        }

        [TestMethod]
        public void EvalResumeYieldValueFibreContextTest()
        {
            var obj = Eval("f = fibre() => {yield 1; yield 2; yield 3; yield 4;}; f();");

            var fc = obj as ScriptFibreContext;

            Assert.AreEqual(1, fc.Resume());
            Assert.AreEqual(1, fc.Result);
            Assert.IsTrue(fc.HasResult);

            Assert.AreEqual(2, fc.Resume());
            Assert.AreEqual(2, fc.Result);
            Assert.IsTrue(fc.HasResult);

            Assert.AreEqual(3, fc.Resume());
            Assert.AreEqual(3, fc.Result);
            Assert.IsTrue(fc.HasResult);

            Assert.AreEqual(4, fc.Resume());
            Assert.AreEqual(4, fc.Result);
            Assert.IsTrue(fc.HasResult);
        }

        [TestMethod]
        public void EvalYieldReturnTest()
        {
            var obj = Eval("var f = fibre() => {yield return; yield; }; var fc = f(); fc.resume(); fc;");

            var fc = obj as ScriptFibreContext;

            Assert.IsTrue(fc.Completed);
            Assert.IsFalse(fc.HasResult);
            Assert.IsNull(fc.Result);
        }

        [TestMethod]
        public void EvalYieldReturnValueTest()
        {
            var obj = Eval("var f = fibre() => {yield return 100; yield; }; fc = f(); value = fc.resume(); this;");

            var fc = obj.fc as ScriptFibreContext;

            Assert.IsTrue(fc.Completed);
            Assert.IsTrue(fc.HasResult);
            Assert.AreEqual(100, obj.value);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void EvalYieldInvalidContextTest()
        {
            Eval("yield;");
        }

        [TestMethod]
        public void EvalBeginReturnsFibreContextTest()
        {
            var obj = Eval("var x = fibre() => {}; x();") as ScriptFibreContext;
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void EvalResumeTest()
        {
            var obj = Eval("var x = 0; var f = fibre(i) => x = i; var fc = f(100); fc.resume(); x;");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void EvalResumeFinishedContext()
        {
            Eval("var f = fibre() => {}; var fc = f(); fc.resume(); fc.resume();");
        }

        [TestMethod]
        public void EvalAwaitTest()
        {
            var obj = Eval(@"

var f3 = fibre() => {
    yield return 100;
};

var f2 = fibre() => {
    yield return await f3();
};

var f = fibre() => {
    yield return await f2();
};

var fc = f();

while(!fc.completed)
    fc.resume();

fc.result;
            ");

            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalAwaitMemberTest()
        {
            var obj = Eval(@"
var obj = {
    f1: fibre() => {
        yield return await obj.f2();
    },
    f2: fibre() => {
        yield return 100;
    }
};

var fc = obj.f1();
while(!fc.completed)
    fc.resume();
fc.result;
");
            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalYieldAwaitTest()
        {
            var obj = Eval(@"
var f1 = fibre() => 1; 
var f2 = fibre() => {
    yield await f1();
};

var fc = f2();
fc.resume();
");

            Assert.AreEqual(null, obj);
        }

        [TestMethod]
        public void EvalYieldAwaitMemberTest()
        {
            var obj = Eval(@"
this.f1 = fibre() => 1; 
this.f2 = fibre() => {
    yield await this.f1();
};

var fc = this.f2();

while(!fc.completed)
    fc.resume();

fc.result;
");

            Assert.AreEqual(1, obj);
        }

        [TestMethod]
        public void EvalBooleanAndAwaitsF1AndF2Test()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return true;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return true;
};

this.and = fibre() => await f1() && await f2();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsTrue(obj.f2Called);
        }

        [TestMethod]
        public void EvalBooleanAndAwaitsF1OnlyTest()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return false;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return true;
};

this.and = fibre() => await f1() && await f2();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsFalse(obj.f2Called);
        }

        [TestMethod]
        public void EvalBooleanOrAwaitsF1AndF2Test()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return false;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return true;
};

this.and = fibre() => await f1() || await f2();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsTrue(obj.f2Called);
        }

        [TestMethod]
        public void EvalBooleanOrAwaitsF1OnlyTest()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return true;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return true;
};

this.and = fibre() => await f1() || await f2();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsFalse(obj.f2Called);
        }

        [TestMethod]
        public void EvalBooleanAndNestedAndAwaitsF1AndF2AndF3Test()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;
this.f3Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return true;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return true;
};

this.f3 = fibre() => {
    f3Called = true;
    yield return true;
};

this.and = fibre() => await f1() && await f2() && await f3();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsTrue(obj.f2Called);
            Assert.IsTrue(obj.f3Called);
        }

        [TestMethod]
        public void EvalBooleanAndNestedOrAwaitsF1OrF2OrF3Test()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;
this.f3Called = false;

this.f1 = fibre() => {
    f1Called = true;
    yield return false;
};  

this.f2 = fibre() => {
    f2Called = true;
    yield return false;
};

this.f3 = fibre() => {
    f3Called = true;
    yield return true;
};

this.and = fibre() => await f1() || await f2() || await f3();

this;
");
            obj.and().execute();

            Assert.IsTrue(obj.f1Called);
            Assert.IsTrue(obj.f2Called);
            Assert.IsTrue(obj.f3Called);
        }

        [TestMethod]
        public void EvalSimpleTernaryLeftRightTrueTest()
        {
            var obj = Eval("true ?: false;");

            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalSimpleTernaryLeftRightFalseTest()
        {
            var obj = Eval("false ?: true;");

            Assert.IsTrue(obj);
        }

        [TestMethod]
        public void EvalSimpleTernaryLeftTrueDoesNotAwaitRight()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f1 = fibre() => f1Called = true;

this.ternary = fibre() => true ?: await f1();

this;
");
            obj.ternary().execute();
            Assert.IsFalse(obj.f1Called);
        }

        [TestMethod]
        public void EvalSimpleTernaryLeftFalseDoesAwaitRight()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f1 = fibre() => f1Called = true;

this.ternary = fibre() => false ?: await f1();

this;
");
            obj.ternary().execute();
            Assert.IsTrue(obj.f1Called);
        }

        [TestMethod]
        public void EvalSimpleTernaryAwaitableLeftFalseDoesNotAwaitRight()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {f1Called = true; yield return true;};

this.f2Called = false;
this.f2 = fibre() => f2Called = true;

this.ternary = fibre() => await f1() ?: await f2();

this;
");
            obj.ternary().execute();
            Assert.IsTrue(obj.f1Called);
            Assert.IsFalse(obj.f2Called);
        }

        [TestMethod]
        public void EvalTernaryTrueDoesAwaitLeft()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {f1Called = true; yield return true;};

this.f2Called = false;
this.f2 = fibre() => f2Called = true;

this.ternary = fibre() => true ? await f1() : await f2();

this;
");
            obj.ternary().execute();
            Assert.IsTrue(obj.f1Called);
            Assert.IsFalse(obj.f2Called);
        }

        [TestMethod]
        public void EvalTernaryFalseDoesAwaitRight()
        {
            var obj = Eval(@"
this.f1Called = false;
this.f2Called = false;

this.f1 = fibre() => {f1Called = true; yield return true;};

this.f2Called = false;
this.f2 = fibre() => f2Called = true;

this.ternary = fibre() => false ? await f1() : await f2();

this;
");
            obj.ternary().execute();
            Assert.IsFalse(obj.f1Called);
            Assert.IsTrue(obj.f2Called);
        }

        [TestMethod]
        public void EvalTernaryExpressionTrueTest()
        {
            var obj = Eval("x = 10; x > 5 ? x * 2 : null;");

            Assert.AreEqual(20, obj);
        }

        [TestMethod]
        public void EvalTernaryExpressionFalseTest()
        {
            var obj = Eval("x = \"name\"; x == \"number\" ? null : \"no match\";");

            Assert.AreEqual("no match", obj);
        }

        [TestMethod]
        public void EvalEmptyStringFalseTernaryExpressionTest()
        {
            var obj = Eval("\"\" ?: \"empty\";");

            Assert.AreEqual("empty", obj);
        }

        [TestMethod]
        public void EvalNonEmptyStringTrueTernaryExpressionTest()
        {
            var obj = Eval("x = \"not empty\"; x ?: \"\";");

            Assert.AreEqual("not empty", obj);
        }

        [TestMethod]
        public void EvalStringTrueTernaryExpressionTest()
        {
            var obj = Eval("x = \"value\"; x ? \"not empty\" : null;");

            Assert.AreEqual("not empty", obj);
        }

        [TestMethod]
        public void EvalIntZeroFalseTernaryExpressionTest()
        {
            var obj = Eval("0 ?: 100;");

            Assert.AreEqual(100, obj);
        }

        [TestMethod]
        public void EvalNonIntZeroFalseTernaryExpressionTest()
        {
            var obj = Eval("200 ?: 0;");

            Assert.AreEqual(200, obj);
        }

        [TestMethod]
        public void EvalIntZeroTrueTernaryExpressionTest()
        {
            var obj = Eval("x = 1; x ? 100 : null;");

            Assert.AreEqual(100, obj);
        }


        [TestMethod]
        public void EvalFloatZeroFalseTernaryExpressionTest()
        {
            var obj = Eval("0.0 ?: 100.0;");

            Assert.AreEqual(100f, obj);
        }

        [TestMethod]
        public void EvalNonFloatZeroFalseTernaryExpressionTest()
        {
            var obj = Eval("20.5 ?: 0.0;");

            Assert.AreEqual(20.5f, obj);
        }

        [TestMethod]
        public void EvalIntFloatTrueTernaryExpressionTest()
        {
            var obj = Eval("x = 1.0; x ? 100.0 : null;");

            Assert.AreEqual(100f, obj);
        }

        [TestMethod]
        public void EvalNotNullTrueTernaryExpressionTest()
        {
            var obj = Eval("var val = {notNull: true}; val ?: \"was null\";");

            Assert.IsTrue(obj.notNull);
        }

        [TestMethod]
        public void EvalNullFalseTernaryExpressionTest()
        {
            var obj = Eval("null ?: \"was null\";");

            Assert.AreEqual("was null", obj);
        }

        [TestMethod]
        public void TupleTest()
        {
            var obj = (ScriptTuple)Eval("{1,2,3};");

            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(3, obj[2]);
        }

        [TestMethod]
        public void TupleAssignTest()
        {
            var obj = Eval(@"
var x;
var y;

{x,y} = {10, 20};

{
    x: x,
    y: y
};
");

            Assert.AreEqual(10, obj.x);
            Assert.AreEqual(20, obj.y);
        }

        [TestMethod]
        public void TupleAssignNonTupleTest()
        {
            var obj = Eval(@"
var x;
var y;

{x,y} = 10;

{
    x: x,
    y: y
};
");

            Assert.AreEqual(10, obj.x);
            Assert.IsNull(obj.y);
        }

        [TestMethod]
        public void TupleAssignObjectMembers()
        {
            var obj = Eval(@"
var obj = {x: null, y: null};
{obj.x, obj.y} = {1, 2};
obj;
");
            Assert.AreEqual(1, obj.x);
            Assert.AreEqual(2, obj.y);
        }

        [TestMethod]
        public void MultipleTupleAssignTest()
        {
            var obj = Eval(@"
var x;
var y;
var z;

{x,y,z} = {10, 20, 30};

{
    x: x,
    y: y,
    z: z
};
");

            Assert.AreEqual(10, obj.x);
            Assert.AreEqual(20, obj.y);
            Assert.AreEqual(30, obj.z);

        }

        [TestMethod]
        public void MultipleTupleAssignNonTupleTest()
        {
            var obj = Eval(@"
var x;
var y;
var z;

{x,y,z} = 10;

{
    x: x,
    y: y,
    z: z
};
");

            Assert.AreEqual(10, obj.x);
            Assert.IsNull(obj.y);
            Assert.IsNull(obj.z);
        }

        [TestMethod]
        public void MultipleTupleAssignObjectMembers()
        {
            var obj = Eval(@"
var obj = {x: null, y: null, z: null};
{obj.x, obj.y, obj.z} = {1, 2, 3};
obj;
");
            Assert.AreEqual(1, obj.x);
            Assert.AreEqual(2, obj.y);
            Assert.AreEqual(3, obj.z);
        }

        [TestMethod]
        public void TupleAssignReturnsTupleTest()
        {
            var obj = Eval(@"
var x;
var y;
var z;

{x,y,z} = {1,2,3};
");

            Assert.IsInstanceOfType(obj, typeof(ScriptTuple));
            Assert.AreEqual(1, obj[0]);
            Assert.AreEqual(2, obj[1]);
            Assert.AreEqual(3, obj[2]);
        }

        [TestMethod]
        public void TupleDeclarationTest()
        {
            var obj = Eval(@"
var {x, y};

{ x: x, y: y};
");
            Assert.IsNull(obj.x);
            Assert.IsNull(obj.y);

        }

        [TestMethod]
        public void TupleDeclarationValuesTest()
        {
            var obj = Eval(@"
var {x, y} = {""tuple"", ""value""};

{ x: x, y: y};
");
            Assert.AreEqual("tuple", obj.x);
            Assert.AreEqual("value", obj.y);

        }

        [TestMethod]
        public void MultipleTupleDeclarationTest()
        {
            var obj = Eval(@"
var {x, y, z};

{ x: x, y: y, z: z};
");
            Assert.IsNull(obj.x);
            Assert.IsNull(obj.y);
            Assert.IsNull(obj.z);

        }

        [TestMethod]
        public void MultipleTupleDeclarationValuesTest()
        {
            var obj = Eval(@"
var {x, y, z} = {""multiple"", ""tuple"", ""values""};

{ x: x, y: y, z: z};
");
            Assert.AreEqual("multiple", obj.x);
            Assert.AreEqual("tuple", obj.y);
            Assert.AreEqual("values", obj.z);

        }


        private dynamic Eval(string script)
        {
            var curMethod = new StackTrace().GetFrames().Skip(1).First().GetMethod().Name;
            return _scriptEngine.Eval(curMethod, script);
        }
    }
}
