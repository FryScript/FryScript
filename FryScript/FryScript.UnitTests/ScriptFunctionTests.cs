using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptFunctionTests
    {
        private ScriptFunction _scriptFunction;
        private dynamic _dynamicObj;

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void InvokeVoidTest()
        {
            int inc = 0;
            Action<int, int> action = (x, y) =>
            {
                inc = x * y;
            };

            InitDelegate(action);

            var result = _dynamicObj(2, 3);
            Assert.IsNull(result);
            Assert.AreEqual(6, inc);
        }

        [TestMethod]
        public void InvokeReturnedResultTest()
        {
            Func<int, int, int> func = (x, y) => x * y;

            InitDelegate(func);

            var result = _dynamicObj(4, 5);

            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void InvokeWrapsParamsTest()
        {
            Func<ScriptParams, int> func = p => p.Get<int>(0) + p.Get<int>(1);

            InitDelegate(func);

            var result = _dynamicObj(10, 20);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void InvokeParamsFunctionTest()
        {
            Func<ScriptParams, int> func = p => p.Get<int>(0) + p.Get<int>(1);

            InitDelegate(func);

            var result = _dynamicObj(new ScriptParams(10, 20));

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void InvokeUnwrapsParamsFunctionTest()
        {
            Func<int, int, int, int> func = (x, y, z) => x + y + z;

            InitDelegate(func);

            var result = _dynamicObj(new ScriptParams(10, 20, 30));

            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void InvokeUnderloadedTest()
        {
            object underLoadedParam = 100;
            Action<int, int> func = (x, y) => underLoadedParam = y;

            InitDelegate(func);

            _dynamicObj(10);

            Assert.AreEqual(0, underLoadedParam);
        }

        [TestMethod]
        public void InvokeOverloadedTest()
        {
            object overLoadedParam = 0;
            Action func = () => overLoadedParam = 100;

            InitDelegate(func);

            _dynamicObj(10, 20, 30, 40, 50, 60);

            Assert.AreEqual(100, overLoadedParam);
        }

        private void InitDelegate(Delegate del)
        {
            _scriptFunction = new ScriptFunction(del);
            _dynamicObj = _scriptFunction;
        }
    }
}
