using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ScriptArrayTests
    {
        private ScriptArray _scriptArray;
        private dynamic _dynamicArray => _scriptArray;

        [TestInitialize]
        public void TestInititalize()
        {
            _scriptArray = new ScriptArray();
        }

        [TestMethod]
        public void Ctor_Size()
        {
            _scriptArray = new ScriptArray(10);

            Assert.AreEqual(10, _scriptArray.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            _dynamicArray.add("test");

            var items = ScriptArray.GetItems(_scriptArray);
            
            Assert.IsTrue(items.All(i => (string) i == "test"));
        }

        [TestMethod]
        public void RemoveTest()
        {
            _dynamicArray.add("test");
            _dynamicArray.remove(0);

            Assert.AreEqual(0, _scriptArray.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void RemoveIndexOutOfBoundsTest()
        {
            _dynamicArray.remove(0);
        }

        [TestMethod]
        public void RemoveLastTest()
        {
            _dynamicArray.add("test1");
            _dynamicArray.add("test2");
            _dynamicArray.add("test3");

            _dynamicArray.removeLast();

            Assert.IsFalse(_scriptArray.Any(i => (string) i == "test3"));
        }

        [TestMethod]
        public void RemoveFirstTest()
        {
            _dynamicArray.add("test1");
            _dynamicArray.add("test2");
            _dynamicArray.add("test3");

            _dynamicArray.removeFirst();

            Assert.IsFalse(_scriptArray.Any(i => (string) i == "test1"));
        }

        [TestMethod]
        public void InsertTest()
        {
            _dynamicArray.add("test1");
            _dynamicArray.add("test3");

            _dynamicArray.insert(1, "test2");

            Assert.AreEqual("test1", _scriptArray[0]);
            Assert.AreEqual("test2", _scriptArray[1]);
            Assert.AreEqual("test3", _scriptArray[2]);
        }

        [TestMethod]
        public void GetCountTest()
        {
            _dynamicArray.add("test1");
            _dynamicArray.add("test2");
            _dynamicArray.add("test3");

            Assert.AreEqual(3, _dynamicArray.count);
        }

        [TestMethod]
        public void IndexTest()
        {
            _dynamicArray.add("test1");
            _dynamicArray.add("test2");
            _dynamicArray.add("test3");

            Assert.AreEqual("test1", _dynamicArray[0]);
            Assert.AreEqual("test2", _dynamicArray[1]);
            Assert.AreEqual("test3", _dynamicArray[2]);
        }

        [TestMethod]
        public void SetGetMemberTest()
        {
            _dynamicArray.test = 100;

            Assert.AreEqual(100, _dynamicArray.test);
        }

        [TestMethod]
        public void ConvertToObjectArrayTest()
        {
            _dynamicArray.add(1);
            _dynamicArray.add(2);
            _dynamicArray.add(3);

            var r = (object[])_dynamicArray;

            Assert.IsTrue(r.Any(i => (int)i == 1));
            Assert.IsTrue(r.Any(i => (int)i == 2));
            Assert.IsTrue(r.Any(i => (int)i == 3));
        }

        [TestMethod]
        public void ConvertToListTest()
        {
            _dynamicArray.add(1);
            _dynamicArray.add(2);
            _dynamicArray.add(3);

            var r = (List<object>)_dynamicArray;

            Assert.IsTrue(r.Any(i => (int)i == 1));
            Assert.IsTrue(r.Any(i => (int)i == 2));
            Assert.IsTrue(r.Any(i => (int)i == 3));
        }
    }
}
