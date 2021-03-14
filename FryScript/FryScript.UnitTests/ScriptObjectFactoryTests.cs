using FryScript.HostInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptObjectBuilderFactoryTests
    {
        private ScriptObjectFactory _factory;
        private ITypeFactory _typeFactory;
        private IScriptObject _obj;
        private IScriptObjectBuilder _builder;
        private Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder, IScriptObjectBuilder> _factoryFunc;
        private IScriptObjectBuilder _parent;

        [TestInitialize]
        public void TestIntialize()
        {
            _builder = Substitute.For<IScriptObjectBuilder>();
            _obj = Substitute.For<IScriptObject>();
            _factoryFunc = Substitute.For<Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder, IScriptObjectBuilder>>();
            _factoryFunc.Invoke(Arg.Any<Type>(), Arg.Any<Func<IScriptObject, object>>(), Arg.Any<Uri>(), Arg.Any<IScriptObjectBuilder>());
            _typeFactory = Substitute.For<ITypeFactory>();
            _factory = new ScriptObjectFactory(_factoryFunc, _typeFactory);
            _parent = Substitute.For<IScriptObjectBuilder>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Factory()
        {
            new ScriptObjectFactory(null, _typeFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Type_Factory()
        {
            new ScriptObjectFactory(_factoryFunc, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Type()
        {
            _factory.Create(null, o => o, new Uri("test:///name"), _parent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Ctor()
        {
            _factory.Create(typeof(object), null, new Uri("test:///name"), _parent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Uri()
        {
            _factory.Create(typeof(object), o => o, null, _parent);
        }

        [TestMethod]
        public void Create_Success()
        {
            var type = typeof(ScriptObject);
            var scriptableType = typeof(ScriptObject);
            Func<IScriptObject, object> ctor = o => o;
            var uri = new Uri("test:///name");

            _typeFactory.CreateScriptableType(type).Returns(scriptableType);
            _factoryFunc.Invoke(
                typeof(ScriptObjectBuilder<>).MakeGenericType(scriptableType),
                ctor,
                uri, 
                _parent).Returns(_builder);

            _builder.Build().Returns(_obj);

            var result = _factory.Create(type, ctor, uri, _parent);

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePrimitive_Null_Type()
        {
            _factory.CreatePrimitive(null);
        }

        [TestMethod]
        public void CreatePrimitive_Success()
        {
            var result = _factory.CreatePrimitive(typeof(string));

            Assert.IsInstanceOfType(result, typeof(ScriptPrimitive<string>));
        }
    }
}
