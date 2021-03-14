using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
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
                _ = target ?? throw new ArgumentNullException(nameof(target));
                return new ImplicitTarget();
            }

            public static explicit operator ExplicitTarget(Target target)
            {
                _ = target ?? throw new ArgumentNullException(nameof(target));
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
            var target = new Target
            {
                Name = "test"
            };

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

        [TestMethod]
        public void Internal_Member_Array_Resizes_As_Members_Are_Added()
        {
            for(var i = 0; i < 100; i++)
            {
                _dynamicObj[$"member_{i}"] = i;
            }

            Assert.AreEqual(112, _scriptObject.ObjectCore.MemberData.Length);
        }
        public class Jimborb : ScriptArray
        {

        }
    }
}
