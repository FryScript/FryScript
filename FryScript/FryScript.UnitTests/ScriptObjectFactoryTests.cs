using System;
using FryScript.HostInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptObjectBuilderFactoryTests
    {
        private ScriptObjectFactory _factory;
        private ITypeFactory _typeFactory;
        private IScriptObject _obj;
        private IScriptObjectBuilder _builder;
        private Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder> _factoryFunc;

        [TestInitialize]
        public void TestIntialize()
        {
            _builder = Substitute.For<IScriptObjectBuilder>();
            _obj = Substitute.For<IScriptObject>();
            _factoryFunc = Substitute.For<Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder>>();
            _factoryFunc.Invoke(Arg.Any<Type>(), Arg.Any<Func<IScriptObject, object>>(), Arg.Any<Uri>());
            _typeFactory = Substitute.For<ITypeFactory>();
            _factory = new ScriptObjectFactory(_factoryFunc, _typeFactory);
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
            _factory.Create(null, o => o, new Uri("test:///name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Ctor()
        {
            _factory.Create(typeof(object), null, new Uri("test:///name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Uri()
        {
            _factory.Create(typeof(object), o => o, null);
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
                uri).Returns(_builder);

            _builder.Build().Returns(_obj);

            var result = _factory.Create(type, ctor, uri);

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
