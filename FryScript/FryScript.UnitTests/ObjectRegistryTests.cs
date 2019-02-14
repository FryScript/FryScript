using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ObjectRegistryTests
    {
        private ObjectRegistry _objectRegistry;
        private IScriptObject _obj;

        [TestInitialize]
        public void TestInitialize()
        {
            //_obj = .For<IScriptObject>();
            //_objectRegistry = new ObjectRegistry();
        }

        [TestMethod]
        public void Import_Type_By_Name()
        {
            //_objectRegistry.Import()
        }
    }
}
