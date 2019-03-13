﻿using FryScript.Compilation;
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

        private IScriptObject _obj;
        private IScriptProvider _scriptProvider;
        private IScriptObjectBuilder _objBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptProvider = Substitute.For<IScriptProvider>();
            _compiler = Substitute.For<IScriptCompiler>();
            _registry = Substitute.For<IObjectRegistry>();
            _runtime = new ScriptRuntime(_scriptProvider, _compiler, _registry);

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
            new ScriptRuntime(_scriptProvider, null, _registry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Object_Registry()
        {
            new ScriptRuntime(_scriptProvider, _compiler, null);
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
            _registry.TryGetObject("name", out IScriptObject obj).Returns(c =>
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
            _registry.TryGetObject("name : relativeTo", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name", "relativeTo");

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ScriptLoadException))]
        public void Get_Name_Does_Not_Exist_As_Script()
        {
            _registry.TryGetObject("name", out IScriptObject obj).Returns(false);

            _scriptProvider.TryGetScriptInfo("name", out ScriptInfo scriptInfo).Returns(false);

            _runtime.Get("name");
        }

        [TestMethod]
        public void Get_Name_Does_Exist_Secondary_Uri_Lookup()
        {
            _registry.TryGetObject("name", out IScriptObject obj).Returns(false);

            var scriptInfo = new ScriptInfo
            {
                Uri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory)),
                Source = "source"
            };

            _scriptProvider.TryGetScriptInfo("name", out ScriptInfo scriptInfoArg).Returns(c =>
            {
                c[1] = scriptInfo;

                return true;
            });

            _registry.TryGetObject(scriptInfo.Uri.AbsoluteUri, out IScriptObject objArg).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        public void Get_Name_Compiled_From_Source()
        {
            _registry.TryGetObject(Arg.Any<string>(), out IScriptObject obj).Returns(false);


            var expectedScriptInfo = new ScriptInfo
            {
                Source = "source",
                Uri = new Uri("test://name")
            };

            _scriptProvider.TryGetScriptInfo("name", out ScriptInfo scriptInfo).Returns(c =>
            {
                c[1] = expectedScriptInfo;

                return true;
            });

            _compiler.Compile2("source", "test://name", Arg.Is<CompilerContext>(c =>
                c.Name == expectedScriptInfo.Uri.AbsoluteUri
                && c.ScriptRuntime == _runtime
            ));
        }
    }
}
