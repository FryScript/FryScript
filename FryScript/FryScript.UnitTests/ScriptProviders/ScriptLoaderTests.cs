using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace FryScript.UnitTests.ScriptProviders
{
    [TestClass]
    public class ScriptLoaderTests
    {
        private IScriptLoader _loader;
        private IScriptProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = Substitute.For<IScriptProvider>();
            _loader = new ScriptLoader(new[] { _provider });
        }

        [DataTestMethod]
        [DynamicData(nameof(CtorDynamicData), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Script_Providers(IScriptProvider[] scriptProviders)
        {
            _loader = new ScriptLoader(scriptProviders);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Load_Invalid_Path(string path)
        {
            _loader.Load(path, "relativeTo");
        }

        [TestMethod]
        public void Load_Null_Relative_Path()
        {
            var outUri = new Uri("file://test");
            _provider.TryGetUri("path", out Uri uri, null).Returns(c =>
             {
                 c[1] = outUri;
                 return true;
             });

            _provider.GetScript(outUri).Returns("script");

            var result = _loader.Load("path", null);

            Assert.AreEqual("script", result);
        }

        [TestMethod]
        public void Load_Relative_Path()
        {
            var outUri = new Uri("file://test");

            _provider.TryGetUri("path", out Uri uri, "relativeTo").Returns(c =>
             {
                 c[1] = outUri;
                 return true;
             });

            _provider.GetScript(outUri).Returns("script");


            var result = _loader.Load("path", "relativeTo");

            Assert.AreEqual("script", result);
        }

        [TestMethod]
        public void Load_Calls_Multiple_Providers()
        {
            var provider2 = Substitute.For<IScriptProvider>();
            _loader = new ScriptLoader(new[]
            {
                _provider,
                provider2
            });

            _provider.TryGetUri("path", out Uri uri, null).Returns(false);
            provider2.TryGetUri("path", out uri, null).Returns(true);

            _loader.Load("path", null);

            provider2.Received().GetScript(Arg.Any<Uri>());
        }

        [TestMethod]
        [ExpectedException(typeof(ScriptLoadException))]
        public void Load_Throws_Script_Load_Exception()
        {
            _provider.TryGetUri(Arg.Any<string>(), out Uri uri, Arg.Any<string>()).Returns(false);

            _loader.Load("path", null);
        }

        private static IEnumerable<object[]> CtorDynamicData()
        {
            yield return new object[] { null };
            yield return new object[] { new IScriptProvider[0] };
        }
    }
}
