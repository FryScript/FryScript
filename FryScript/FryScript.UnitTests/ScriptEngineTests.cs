using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    public class DoesNotImplementIScriptable
    {
        
    }

    [ScriptableType("test")]
    public class NoPublicParameterlessConstructor : IScriptable
    {
        private NoPublicParameterlessConstructor()
        {
        }

        public dynamic Script { get; set; }
    }

    [ScriptableType("test")]
    public class Importable : IScriptable
    {
        private readonly string _name;

        public string Name { get { return _name; } }

        public dynamic Script { get; set; }

        public Importable()
        {
        }

        public Importable(string name)
        {
            _name = name;
        }

        [ScriptableProperty("count")]
        public int Count { get; set; }

        [ScriptableMethod("sayHelloTo")]
        public string SayHelloTo(Importable importable)
        {
            return string.Format("{0} says hello to {1}!", _name, importable._name);
        }
    }

    [ScriptableType("newable")]
    public class Constructible : IScriptable
    {
        public dynamic Script { get; set; }

        [ScriptableProperty("ctor")]
        public ScriptFunction Ctor { get; set; }

        [ScriptableProperty("onCtor")]
        public int OnCtor { get; set; }

        public Constructible()
        {
            Ctor = ScriptFunction.WrapMethod<int, int>(OnCtorInvoke);
        }

        private void OnCtorInvoke(int i, int j)
        {
            OnCtor =  i * j;
        }
    }

    [TestClass]
    public class ScriptEngineTests
    {
        private ScriptEngine _scriptEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ImportTypeDoesNotImplementIScriptableTest()
        {
            _scriptEngine.Import(typeof(DoesNotImplementIScriptable));
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof (ArgumentException))]
        public void ImportTypeDoesNotHavePublicParameterlessCtorTest()
        {
            _scriptEngine.Import(typeof(NoPublicParameterlessConstructor));
        }

        [TestMethod]
        [Ignore]

        public void ImportTest()
        {
            //_scriptEngine.Import(typeof(Importable));

            //var obj = _scriptEngine.Get("test");

            //var target = (Importable)ScriptObject.GetTarget(obj);

            //Assert.IsNotNull(target);
            //Assert.AreEqual(obj, target.Script);
            //Assert.AreEqual("test", obj.GetScriptType());
        }

        [TestMethod]
        [Ignore]

        public void ImportCtorTest()
        {
            //_scriptEngine.Import("test", () => new Importable("Test Name"));

            //var obj = _scriptEngine.Get("test");

            //var target = (Importable)ScriptObject.GetTarget(obj);

            //Assert.IsNotNull(target);
            //Assert.AreEqual("Test Name", target.Name);
            //Assert.AreEqual(obj, target.Script);
            //Assert.AreEqual("test", obj.GetScriptType());
        }

        [TestMethod]
        [Ignore]

        public void ImportSetPropertyTest()
        {
            //_scriptEngine.Import(typeof(Importable));

            //dynamic obj = _scriptEngine.Get("test");

            //var target = (Importable) ScriptObject.GetTarget(obj);

            //obj.count = 100;

            //Assert.AreEqual(100, target.Count);
        }

        [TestMethod]
        [Ignore]

        public void ImportGetPropertyTest()
        {
            //_scriptEngine.Import(typeof(Importable));

            //dynamic obj = _scriptEngine.Get("test");

            //var target = (Importable)ScriptObject.GetTarget(obj);
            //target.Count = 100;

            //Assert.AreEqual(100, obj.count);
        }

        [TestMethod]
        [Ignore]

        public void BindTest()
        {
            //var importable = new Importable
            //{
            //    Count = 200
            //};

            //_scriptEngine.Import(typeof(Importable));
            //ScriptObject obj = _scriptEngine.Bind(importable, "test");

            //Assert.AreEqual(importable, ScriptObject.GetTarget(obj));
            //Assert.AreEqual(obj, importable.Script);
        }

        [Ignore]
        [TestMethod]
        public void BindWithArgsTest()
        {
            //var constructible = new Constructible();

            //_scriptEngine.Import(typeof(Constructible));
            //dynamic newable = _scriptEngine.Get("newable");
            //dynamic obj = _scriptEngine.Bind(constructible, "newable", 5, 3);

            //Assert.AreEqual(constructible, ScriptObject.GetTarget(obj));
            //Assert.AreNotEqual(newable, constructible.Script);
            //Assert.AreEqual(15, constructible.OnCtor);
        }

        [TestMethod]
        public void ExtendTypeTest()
        {
            _scriptEngine.Import<Importable>();
            dynamic obj = _scriptEngine.Compile("test2", "@extend \"test\"; this.count = 100;");

            Assert.AreEqual(100, obj.count);
        }

        [Ignore]
        [TestMethod]
        public void ConvertTest()
        {
            //var importable = new Importable();

            //_scriptEngine.Import(typeof(Importable));
            //ScriptObject obj = _scriptEngine.Bind(importable, "test");

            //Assert.AreEqual(importable, ScriptObject.GetTarget(obj));
            //Assert.AreEqual(obj, importable.Script);
        }

        [TestMethod]
        [Ignore]
        public void MethodCallTest()
        {
            //var importable1 = new Importable("Mango");
            //var importable2 = new Importable("Mikey");

            //_scriptEngine.Import(typeof(Importable));

            //dynamic obj1 = _scriptEngine.Bind(importable1, "test");
            //dynamic obj2 = _scriptEngine.Bind(importable2, "test");

            //var result = obj1.sayHelloTo(obj2);

            //Assert.AreEqual("Mango says hello to Mikey!", result);
        }

        [Ignore]
        [TestMethod]
        public void NewTest()
        {
            //_scriptEngine.Import<Importable>();

            //dynamic obj = _scriptEngine.New("test");

            //Assert.IsTrue(ScriptObject.GetTarget(obj) is Importable);
        }

        [TestMethod]
        [Ignore]
        public void NewWithArgsTest()
        {
            //_scriptEngine.Import<Constructible>();

            //dynamic newable = _scriptEngine.Get("newable");
            //dynamic obj = _scriptEngine.New("newable", 10, 10);

            //Assert.AreNotEqual(newable, obj);
            //Assert.AreEqual(100, obj.onCtor);
        }

        [TestMethod]
        [Ignore]
        public void NewByTypeTest()
        {
            _scriptEngine.Import<Importable>();

            var obj = _scriptEngine.New<Importable>("test");

            Assert.IsNotNull(obj.Script);
        }


        [TestMethod]
        [Ignore]
        public void NewByTypeTestWithArgs()
        {
            _scriptEngine.Import<Constructible>();

            var newable = _scriptEngine.Get("newable");
            var obj = _scriptEngine.New<Constructible>("newable", 5, 2);

            Assert.AreNotEqual(newable, obj.Script);
            Assert.AreEqual(10, obj.OnCtor);
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void NewByTypeInvalidTypeTest()
        {
            _scriptEngine.Compile("invalid", "this;");
            _scriptEngine.Import<Importable>();

            _scriptEngine.New<Importable>("invalid");
        }
    }
}
