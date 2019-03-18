using FryScript.Compilation;
using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IO;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptRuntimeTests
    {
        private ScriptRuntime _runtime;
        private IScriptCompiler _compiler;
        private IObjectRegistry _registry;
        private IScriptObjectBuilderFactory _factory;

        private IScriptObject _obj;
        private IScriptProvider _scriptProvider;
        private IScriptObjectBuilder _objBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptProvider = Substitute.For<IScriptProvider>();
            _compiler = Substitute.For<IScriptCompiler>();
            _registry = Substitute.For<IObjectRegistry>();
            _factory = Substitute.For<IScriptObjectBuilderFactory>();
            _runtime = new ScriptRuntime(_scriptProvider, _compiler, _registry, _factory);

            _obj = Substitute.For<IScriptObject>();
            _objBuilder = Substitute.For<IScriptObjectBuilder>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Script_Providers()
        {
            new ScriptRuntime(null, _compiler, _registry, _factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Compiler()
        {
            new ScriptRuntime(_scriptProvider, null, _registry, _factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Object_Registry()
        {
            new ScriptRuntime(_scriptProvider, _compiler, null, _factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Factory()
        {
            new ScriptRuntime(_scriptProvider, _compiler, _registry, null);
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
        public void Get_Name_Already_Exists()
        {
            _registry.TryGetObject("name.fry", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        public void Get_Name_And_Relative_To_Already_Exists()
        {
            _registry.TryGetObject("name.fry -> relativeTo", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name.fry", "relativeTo");

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        public void Get_Name_Does_Exist_Secondary_Uri_Lookup()
        {
            _registry.TryGetObject("name.fry", out IScriptObject obj).Returns(false);

            var scriptInfo = new ScriptInfo
            {
                Uri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory)),
                Source = "source"
            };

            _scriptProvider.TryGetScriptInfo("name.fry", out ScriptInfo scriptInfoArg, "relativeTo").Returns(c =>
            {
                c[1] = scriptInfo;

                return true;
            });

            _registry.TryGetObject(scriptInfo.Uri.AbsoluteUri, out IScriptObject objArg).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name", "relativeTo");

            Assert.AreEqual(_obj, result);
            _registry.Received().Import("name.fry -> relativeTo", _obj);
        }

        [TestMethod]
        public void Get_Name_Compiled_From_Source()
        {
            _registry.TryGetObject(Arg.Any<string>(), out IScriptObject obj).Returns(false);


            var expectedScriptInfo = new ScriptInfo
            {
                Source = "source",
                Uri = new Uri("test:///name.fry")
            };

            _scriptProvider.TryGetScriptInfo("name.fry", out ScriptInfo scriptInfo).Returns(c =>
            {
                c[1] = expectedScriptInfo;

                return true;
            });

            _compiler.Compile2("source", "test:///name.fry", Arg.Is<CompilerContext>(c =>
                c.Name == expectedScriptInfo.Uri.AbsoluteUri
                && c.ScriptRuntime == _runtime
            )).Returns(new Func<IScriptObject, object>(o => "Constructed!"));

            _factory.Create(typeof(ScriptObject), Arg.Any<Func<IScriptObject, object>>(), new Uri("test:///name.fry"))
                .Returns(_objBuilder);

            _objBuilder.Build().Returns(_obj);

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
            _registry.Received().Import("test:///name.fry", _obj);
        }

        [TestMethod]
        public void Get()
        {
            var sr = new ScriptRuntime();
            var imp = sr.Get("C:/source/git/FryScript/FryScript/FryScript.UnitTests/bin/Debug/scripts/nestedimport3.fry");
        }
    }
}
