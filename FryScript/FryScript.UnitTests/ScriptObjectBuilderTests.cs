﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptObjectBuilderTests
    {
        private ScriptObjectBuilder<ScriptObject> _builder;
        private Func<IScriptObject, object> _ctor;
        private Uri _uri;
        private Func<ScriptObject> _factory;
        private ScriptObject _instance;

        [TestInitialize]
        public void TestInitialize()
        {
            _ctor = Substitute.For<Func<IScriptObject, object>>();
            _uri = new Uri("test:///file");
            _factory = Substitute.For<Func<ScriptObject>>();
            _builder = new ScriptObjectBuilder<ScriptObject>(_factory, _ctor, _uri);
            _instance = new ScriptObject();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Factory_Func()
        {
            new ScriptObjectBuilder<ScriptObject>(null, _ctor, _uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Ctor_Func()
        {
            new ScriptObjectBuilder<ScriptObject>(_factory, null,  _uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Uri()
        {
            new ScriptObjectBuilder<ScriptObject>(_factory, _ctor, null);
        }

        [TestMethod]
        public void Build_Success()
        {
            _factory.Invoke().Returns(_instance);

            var result = _builder.Build();

            Assert.AreEqual(_instance, result);
            Assert.AreEqual(_builder, result.ObjectCore.Builder);
            Assert.AreEqual(_uri, result.ObjectCore.Builder.Uri);
            _ctor.Received().Invoke(_instance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Extend_Null_Obj()
        {
            _builder.Extend(null);
        }

        [TestMethod]
        public void Extend_Success()
        {
            var result = _builder.Extend(_instance);

            Assert.AreEqual(_instance, result);
            _ctor.Received().Invoke(_instance);
        }
    }
}
