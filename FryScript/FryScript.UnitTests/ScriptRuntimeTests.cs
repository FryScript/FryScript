using FryScript.Compilation;
using FryScript.HostInterop;
using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Dynamic;
using System.IO;
using System.Linq.Expressions;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptRuntimeTests
    {
        [ScriptableType("extendedScriptObject")]
        public class ExtendedScriptObject { }

        [ScriptableType("newScriptObject")]
        public class NewScriptObject : ScriptObject
        {
            [ScriptableProperty("arg1")]
            public string Arg1 { get; set; }

            [ScriptableProperty("arg2")]
            public string Arg2 { get; set; }

            [ScriptableMethod("ctor")]
            public void Ctor(string arg1, string arg2)
            {
                Arg1 = arg1;
                Arg2 = arg2;
            }
        }

        private ScriptRuntime _runtime;
        private IScriptCompiler _compiler;
        private IObjectRegistry _registry;
        private IScriptObjectFactory _objectFactory;

        private IScriptObject _obj;
        private IScriptProvider _scriptProvider;
        private IScriptObjectBuilder _objBuilder;
        private ITypeProvider _typeProvider;
        private string _name;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptProvider = Substitute.For<IScriptProvider>();
            _compiler = Substitute.For<IScriptCompiler>();
            _registry = Substitute.For<IObjectRegistry>();
            _objectFactory = Substitute.For<IScriptObjectFactory>();
            _typeProvider = Substitute.For<ITypeProvider>();
            _runtime = new ScriptRuntime(_scriptProvider, _compiler, _registry, _objectFactory, _typeProvider);

            _obj = Substitute.For<IScriptObject>();
            _objBuilder = Substitute.For<IScriptObjectBuilder>();

            _name = "test";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Script_Providers()
        {
            new ScriptRuntime(null, _compiler, _registry, _objectFactory, _typeProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Compiler()
        {
            new ScriptRuntime(_scriptProvider, null, _registry, _objectFactory, _typeProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Object_Registry()
        {
            new ScriptRuntime(_scriptProvider, _compiler, null, _objectFactory, _typeProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Builder_Factory()
        {
            new ScriptRuntime(_scriptProvider, _compiler, _registry, null, _typeProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Type_Provider()
        {
            new ScriptRuntime(_scriptProvider, _compiler, _registry, _objectFactory, null);
        }

        [TestMethod]
        public void Ctor_Registers_Built_In_Types()
        {
            _registry = Substitute.For<IObjectRegistry>();
            _runtime = new ScriptRuntime(_scriptProvider, _compiler, _registry, _objectFactory, _typeProvider);

            _registry.Received().Import("error", Arg.Is<IScriptObject>(a => a is ScriptError));
            _registry.Received().Import("array", Arg.Is<IScriptObject>(a => a is ScriptArray));
            _registry.Received().Import("function", Arg.Is<IScriptObject>(a => a is ScriptFunction));
            _registry.Received().Import("object", Arg.Is<IScriptObject>(a => a is ScriptObject));
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
        public void Get_Name_With_Extension_Already_Exists()
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
            _registry.TryGetObject("name.fry -> test:///relativeTo", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name.fry", new Uri("test:///relativeTo"));

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

            _scriptProvider.TryGetScriptInfo("name.fry", out ScriptInfo scriptInfoArg, new Uri("test:///relativeTo")).Returns(c =>
            {
                c[1] = scriptInfo;

                return true;
            });

            _registry.TryGetObject(scriptInfo.Uri.AbsoluteUri, out IScriptObject objArg).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            var result = _runtime.Get("name", new Uri("test:///relativeTo"));

            Assert.AreEqual(_obj, result);
            _registry.Received().Import("name.fry -> test:///relativeTo", _obj);
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
            
            _compiler.Compile("source", "test:///name.fry", Arg.Is<CompilerContext>(c =>
                c.Name == expectedScriptInfo.Uri.AbsoluteUri
                && c.ScriptRuntime == _runtime
            )).Returns(new Func<IScriptObject, object>(o => "Constructed!"));

            _objectFactory.Create(typeof(ScriptObject), Arg.Any<Func<IScriptObject, object>>(), new Uri("test:///name.fry"), Arg.Any<IScriptObjectBuilder>())
                .Returns(_obj);

            var result = _runtime.Get("name");

            Assert.AreEqual(_obj, result);
            _registry.Received().Import("test:///name.fry", _obj);
            _registry.Received().Import("name.fry", _obj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_Null_Type()
        {
            _runtime.Import(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Import_Type_Not_Decorated_With_Scriptable_Type_Attribute()
        {
            _runtime.Import(typeof(object));
        }

        [TestMethod]
        public void Import_Type_Already_Imported()
        {
            _registry.TryGetObject("extendedScriptObject", out IScriptObject obj)
                .Returns(c =>
                {
                    c[1] = _obj;
                    return true;
                });

            var result = _runtime.Import(typeof(ExtendedScriptObject));

            Assert.AreEqual(_obj, result);
        }

        [TestMethod]
        public void Import_Registers_New_Type()
        {
            _registry.TryGetObject("extendedScriptObject", out IScriptObject obj).Returns(false);

            _objectFactory.Create(typeof(ExtendedScriptObject), Arg.Any<Func<IScriptObject, object>>(), new Uri("runtime://extendedScriptObject.fry"), Builder.ScriptObjectBuilder)
                .Returns(_obj);

            var result = _runtime.Import(typeof(ExtendedScriptObject));

            Assert.AreEqual(_obj, result);
            _registry.Received().Import("extendedScriptObject", _obj);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_Invalid_Name(string name)
        {
            _runtime.Import(name, _obj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_Null_Instance()
        {
            _runtime.Import(_name, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Import_Name_Already_Imported()
        {
            _registry.TryGetObject(_name, out _).Returns(true);

            _runtime.Import(_name, _obj);
        }

        [TestMethod]
        public void Import_Instance_Imported_Successfully()
        {
            _registry.TryGetObject(_name, out _).Returns(false);

            var result = _runtime.Import(_name, _obj);

            Assert.AreEqual(_obj, result);
            _registry.Received().Import(_name, _obj);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void New_Invalid_Name(string name)
        {
            _runtime.New(name);
        }

        [TestMethod]
        public void New_Gets_Script_And_Creates_New_Instance()
        {
            _registry.TryGetObject("name", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            _obj.ObjectCore.Returns(new ObjectCore
            {
                Builder = _objBuilder
            });

            _objBuilder.Build().Returns(new ScriptObject());

            var result = _runtime.New("name");

            Assert.AreNotEqual(_obj, result);
        }

        [TestMethod]
        public void New_Gets_Script_And_Creates_New_Instance_With_Args()
        {
            _registry.TryGetObject("name", out IScriptObject obj).Returns(c =>
            {
                c[1] = _obj;

                return true;
            });

            _obj.ObjectCore.Returns(new ObjectCore
            {
                Builder = _objBuilder
            });

            _objBuilder.Build().Returns(new NewScriptObject());

            var result = _runtime.New("name", "test", "object") as NewScriptObject;

            Assert.AreNotEqual(_obj, result);
            Assert.AreEqual("test", result.Arg1);
            Assert.AreEqual("object", result.Arg2);

        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Eval_Invalid_Script(string source)
        {
            _runtime.Eval(source);
        }

        [TestMethod]
        public void Eval_Expression_Success()
        {
            _compiler.Compile("true;",
                Arg.Any<string>(),
                Arg.Is<CompilerContext>(c => c.IsEvalMode == true))
                .Returns(o => true);

            dynamic result = _runtime.Eval("true;");

            Assert.IsTrue(result);
        }
    }
}
