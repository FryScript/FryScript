using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Threading.Tasks;

    [TestClass]
    public class ScriptObjectTests
    {
        private class Target
        {
            public readonly Dictionary<string, object> Indexes = new Dictionary<string, object>();

            [ScriptableProperty("name")]
            public string Name { get; set; }

            [ScriptableProperty("child")]
            public Target Child { get; set; }

            [ScriptableIndex]
            public object this[string key]
            {
                get { return Indexes[key]; }
                set { Indexes[key] = value; }
            }

            public static implicit operator ImplicitTarget(Target target)
            {
                return new ImplicitTarget();
            }

            public static explicit operator ExplicitTarget(Target target)
            {
                return new ExplicitTarget();
            }
        }

        private class ImplicitTarget
        {
            
        }

        private class ExplicitTarget
        {

        }

        private ScriptObject _scriptObject;
        private dynamic _dynamicObj;

        [TestInitialize]
        public void TestInitialize()
        {
            BinderCache.Current = new BinderCache();

            _scriptObject = new ScriptObject();
            _dynamicObj = _scriptObject;
        }

        [TestMethod]
        [Ignore]
        public void SetMemberTest()
        {
            //_dynamicObj.test = 100;
            //_dynamicObj.test = 200;

            //MemberLookupInfo info;
            ////MemberIndexLookup.Current.TryGetMemberIndex(ScriptObject.GetMemberIndex(_scriptObject), "test", out info);
            //var memberData = _scriptObject.ObjectCore.MemberData;

            //Assert.AreEqual(200, memberData[info.Index]);
        }

        [TestMethod]
        public void GetMemberTest()
        {
            _dynamicObj.test = 100;

            Assert.AreEqual(100, _dynamicObj.test);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void GetMemberUndefinedTest()
        {
            var result = _dynamicObj.test;
        }

        [TestMethod]
        public void SetDynamicIndexTest()
        {
            _dynamicObj["test"] = 100;

            Assert.AreEqual(100, _dynamicObj.test);
        }

        [TestMethod]
        [Ignore]
        public void SetTargetDynamicIndexTest()
        {
            //var target = new Target();
            //ScriptObject.SetTarget(_scriptObject, target);

            //_dynamicObj["name"] = "test";

            //Assert.AreEqual(target.Indexes["name"], "test");
        }

        [TestMethod]
        public void SetIndexTest()
        {
            _scriptObject["test"] = 100;
            Assert.AreEqual(100, _dynamicObj.test);
        }

        [TestMethod]
        [Ignore]
        public void SetTargetIndexTest()
        {
            //var target = new Target();
            //ScriptObject.SetTarget(_scriptObject, target);

            //_scriptObject["name"] = "test";
  
            //Assert.AreEqual("test", _dynamicObj.name);
        }

        [TestMethod]
        public void GetIndexTest()
        {
            _dynamicObj.test = 100;
            Assert.AreEqual(100, _scriptObject["test"]);
        }

        [TestMethod]
        [Ignore]
        public void GetTargetIndexTest()
        {
            //var target = new Target();
            //ScriptObject.SetTarget(_scriptObject, target);

            //_dynamicObj.name = "test";
            //Assert.AreEqual("test", _scriptObject["name"]);
        }

        [TestMethod]
        public void GetDynamicIndexTest()
        {
            _dynamicObj.test = 100;

            Assert.AreEqual(100, _dynamicObj["test"]);
        }

        [TestMethod]
        [Ignore]
        public void GetTargetDynamicIndexTest()
        {
            //var target = new Target();
            //ScriptObject.SetTarget(_scriptObject, target);

            //target.Indexes["name"] = "test";

            //Assert.AreEqual(_dynamicObj["name"], "test");
        }

        [TestMethod]
        public void InvokeMemberTest()
        {
            _dynamicObj.func = new ScriptFunction(new Func<int, int, int>((x, y) => x*y));

            var result = _dynamicObj.func(10, 2);

            Assert.AreEqual(20, result);
        }

        [TestMethod]
        [Ignore]
        public void SetTargetMemberTest()
        {
            //var target = new Target();
            //ScriptObject.SetTarget(_scriptObject, target);

            //_dynamicObj.name = "Test";

            //Assert.AreEqual(target.Name, "Test");
        }

        [TestMethod]
        [Ignore]
        public void GetTargetMemberTest()
        {
            //var target = new Target
            //{
            //    Name = "Test"
            //};
            //ScriptObject.SetTarget(_scriptObject, target);

            //Assert.AreEqual("Test", _dynamicObj.name);
        }

        //[TestMethod]
        //[Ignore]
        //public void ConvertToTargetTest()
        //{
        //    //var target = new Target();

        //    //ScriptObject.SetTarget(_scriptObject, target);

        //    //var obj = (Target) _dynamicObj;

        //    //Assert.AreEqual(target, obj);
        //}

        [TestMethod]
        [Ignore]
        public void SetMemberThreadingTest()
        {
            const int numLoops = 100;

            var task1 = Task.Run(() =>
            {
                for (var i = 0; i < numLoops; i++)
                {
                    _dynamicObj["i" + i] = "i" + i;
                }
            });

            var task2 = Task.Run(() =>
            {
                for (var j = 0; j < numLoops; j++)
                {
                    _dynamicObj["j" + j] = "j" + j;
                }
            });

            Task.WaitAll(task1, task2);

            for (var x = 0; x < numLoops; x++)
            {
                Assert.AreEqual(_dynamicObj["i" + x], "i" + x);
                Assert.AreEqual(_dynamicObj["j" + x], "j" + x);
            }
        }

        [TestMethod]
        [Ignore]
        public void GetMemberThreadingTest()
        {
            const int numLoops = 100;

            var task1 = Task.Run(() =>
            {
                for (var i = 0; i < numLoops; i++)
                {
                    _dynamicObj["i" + i] = "i" + i;
                    Assert.AreEqual("i" + i, _dynamicObj["i" + i]);
                }
            });

            var task2 = Task.Run(() =>
            {
                for (var j = 0; j < numLoops; j++)
                {
                    _dynamicObj["j" + j] = "j" + j;
                    Assert.AreEqual("j" + j, _dynamicObj["j" + j]);
                }
            });

            Task.WaitAll(task1, task2);
        }

        //[TestMethod]
        //[Ignore]
        //public void ConvertTargetImplicitTest()
        //{
        //    //var target = new Target();

        //    //ScriptObject.SetTarget(_scriptObject, target);

        //    //ImplicitTarget obj = _dynamicObj;

        //    //Assert.AreEqual(typeof(ImplicitTarget), obj.GetType());
        //}

        //[TestMethod]
        //[Ignore]
        //public void ConvertTargetExplicitTest()
        //{
        //    //var target = new Target();

        //    //ScriptObject.SetTarget(_scriptObject, target);

        //    //var obj = (ExplicitTarget)_dynamicObj;

        //    //Assert.AreEqual(typeof(ExplicitTarget), obj.GetType());
        //}

        //[TestMethod]
        public void ScriptObjVsExpando()
        {
            dynamic expando = new ExpandoObject();
            //IDictionary<string, object> expando = new ExpandoObject();
            expando.init = 100;

            dynamic so = new ScriptObject();
            so.init = 100;

            for(var x = 0; x < 100; x++)
            {
                const int numLoops = 100;

                var expandoSw = new Stopwatch();
                expandoSw.Start();
                for (var i = 0; i < numLoops; i++)
                {
                    //expando.init = 100;
                    var y = expando.init;
                    //expando.test = i;
                    //expando[i.ToString()] = i;
                }
                expandoSw.Stop();

                var objectSw = new Stopwatch();
                objectSw.Start();
                for (var i = 0; i < numLoops; i++)
                {
                    //so.init = 100;
                    var p = so.init;
                    //so.test = i;
                    //so[i.ToString()] = i;
                }
                objectSw.Stop();

                expandoSw.Reset();
                objectSw.Reset();
            }
        }
    }
}
