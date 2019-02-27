using FryScript.Compilation;
using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptRuntimeTests
    {
        private ScriptRuntime _runtime;
        private IScriptProvider[] _scriptProviders;
        private IScriptCompiler _compiler;
        private IObjectRegistry _registry;

        private IScriptObject _obj;
        private IScriptProvider _scriptProvider;
        private IScriptObjectBuilder _objBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptProvider = Substitute.For<IScriptProvider>();
            _scriptProviders = new[] { _scriptProvider };
            _compiler = Substitute.For<IScriptCompiler>();
            _registry = Substitute.For<IObjectRegistry>();
            _runtime = new ScriptRuntime(_scriptProviders, _compiler, _registry);

            _obj = Substitute.For<IScriptObject>();
            _objBuilder = Substitute.For<IScriptObjectBuilder>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Script_Providers()
        {
            new ScriptRuntime(null, _compiler, _registry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Compiler()
        {
            new ScriptRuntime(_scriptProviders, null, _registry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Object_Registry()
        {
            new ScriptRuntime(_scriptProviders, _compiler, null);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Invalid_Name(string name)
        {
            _runtime.Get(name);
        }

        [TestMethod]
        public void Get_Script_Already_Exists()
        {
            _registry.TryGetObject("name", out IScriptObject obj)
                .Returns(c =>
                {
                    c[1] = _obj;
                    return true;
                });

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        public void Get_Script_Compiles_From_Source()
        {
            _registry.TryGetObject("name", out IScriptObject obj).Returns(false);

            _compiler.Compile(
                "name",
                Arg.Is<CompilerContext>(c => c.ScriptProviders == _scriptProviders))
                .Returns(_objBuilder);

            _objBuilder.Build().Returns(_obj);

            _registry.Import("name", _obj);

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
        }
    }
}
