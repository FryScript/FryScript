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
        private class Target : ScriptObject
        {
            public readonly object[] Indexes = new object[10];

            [ScriptableProperty("name")]
            public string Name { get; set; }

            [ScriptableProperty("child")]
            public Target Child { get; set; }

            [ScriptableIndex]
            public object this[int key]
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
        public void Set_Dynamic_Member_Get_Dynamic_Member()
        {
            _dynamicObj.test = 100;

            Assert.AreEqual(100, _dynamicObj.test);
        }

        [TestMethod]
        public void Set_Dynamic_Member_Get_Object_Member()
        {
            dynamic target = new Target();
            
            target.name = "test";

            Assert.AreEqual("test", (target as Target).Name);
        }

        [TestMethod]
        public void Set_Object_Member_Get_Dynamic_Member()
        {
            var target = new Target();

            target.Name = "test";

            Assert.AreEqual("test", (target as dynamic).name);
        }

        [TestMethod]
        public void Get_Undefined_Member()
        {
            var result = _dynamicObj.test;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Set_Dynamic_Index_Get_Dynamic_Index()
        {
            _dynamicObj["test"] = 100;

            Assert.AreEqual(100, _dynamicObj["test"]);
        }

        [TestMethod]
        public void Set_Dynamic_Index_Get_Object_Index()
        {
            dynamic target = new Target();

            target[5] = "test";

            Assert.AreEqual("test", (target as Target)[5]);
        }

        [TestMethod]
        public void Set_Object_Index_Get_Dynamic_Index()
        {
            var target = new Target();

            target[5]  ="test";

            Assert.AreEqual("test", (target as dynamic)[5]);
        }

        [TestMethod]
        public void Invoke_Member()
        {
            _dynamicObj.func = new ScriptFunction(new Func<int, int, int>((x, y) => x*y));

            var result = _dynamicObj.func(10, 2);

            Assert.AreEqual(20, result);
        }

        // [TestMethod]
        
        // public void SetMemberThreadingTest()
        // {
        //     const int numLoops = 100;

        //     var task1 = Task.Run(() =>
        //     {
        //         for (var i = 0; i < numLoops; i++)
        //         {
        //             _dynamicObj["i" + i] = "i" + i;
        //         }
        //     });

        //     var task2 = Task.Run(() =>
        //     {
        //         for (var j = 0; j < numLoops; j++)
        //         {
        //             _dynamicObj["j" + j] = "j" + j;
        //         }
        //     });

        //     Task.WaitAll(task1, task2);

        //     for (var x = 0; x < numLoops; x++)
        //     {
        //         Assert.AreEqual(_dynamicObj["i" + x], "i" + x);
        //         Assert.AreEqual(_dynamicObj["j" + x], "j" + x);
        //     }
        // }

        // [TestMethod]
        // [Ignore]
        // public void GetMemberThreadingTest()
        // {
        //     const int numLoops = 100;

        //     var task1 = Task.Run(() =>
        //     {
        //         for (var i = 0; i < numLoops; i++)
        //         {
        //             _dynamicObj["i" + i] = "i" + i;
        //             Assert.AreEqual("i" + i, _dynamicObj["i" + i]);
        //         }
        //     });

        //     var task2 = Task.Run(() =>
        //     {
        //         for (var j = 0; j < numLoops; j++)
        //         {
        //             _dynamicObj["j" + j] = "j" + j;
        //             Assert.AreEqual("j" + j, _dynamicObj["j" + j]);
        //         }
        //     });

        //     Task.WaitAll(task1, task2);
        // }

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
