using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FryScript.UnitTests.ScriptProviders
{
    [TestClass]
    public class DirectoryScriptProviderTests
    {
        private DirectoryScriptProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new DirectoryScriptProvider(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullPath()
        {
            new DirectoryScriptProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CtorPathDoesNotExist()
        {
            new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "No Directory"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryGetUriNullPathTest()
        {
            _provider.TryGetUri(null, out Uri uri);
        }

        [TestMethod]
        public void TryGetUriPathDoesNotExist()
        {
            Assert.IsFalse(_provider.TryGetUri("noFile.txt", out Uri uri));
            Assert.IsNull(uri);
        }

        [TestMethod]
        public void TryGetUriPathDoesExist()
        {
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry"));

            Assert.IsTrue(_provider.TryGetUri("./scripts/errorHandling1.fry", out Uri uri));
            Assert.AreEqual(expectedUri, uri);
        }

        [TestMethod]
        public void TryGetUriCannotSearchOutsideRoot()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));

            Assert.IsFalse(_provider.TryGetUri("../outside.txt", out Uri uri));
            Assert.IsNull(uri);
        }

        [TestMethod]
        public void TryGetUriRelativeToDoesNotExist()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));
            var relativeTo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "noFile.fry");

            Assert.IsFalse(_provider.TryGetUri("simpleImport.fry", out Uri uri, relativeTo));
            Assert.IsNull(uri);
        }

        [TestMethod]
        public void TryGetUriRelativeToDoesExist()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "simpleImport.fry"));
            var relativeTo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry");

            Assert.IsTrue(_provider.TryGetUri("simpleImport.fry", out Uri uri, relativeTo));
            Assert.AreEqual(expectedUri, uri);
        }

        [TestMethod]
        public void TryGetUriRelativeToSearchesUpDirectoryTree()
        {
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "outside.txt"));
            var relativeTo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry");
            Assert.IsTrue(_provider.TryGetUri("outside.txt", out Uri uri, new Uri(relativeTo).AbsoluteUri));
            Assert.AreEqual(expectedUri, uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetScriptNullUri()
        {
            _provider.GetScript(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetScriptNotAbsoluteUri()
        {
            _provider.GetScript(new Uri("/test", UriKind.Relative));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetScriptFileDoesNotExist()
        {
            _provider.GetScript(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "noFile.txt")));
        }

        [TestMethod]
        public void GetScript()
        {
            var uri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "protoImport.fry"));
            var expectedScript = File.ReadAllText(uri.LocalPath);
            Assert.AreEqual(expectedScript, _provider.GetScript(uri));
        }
    }
}
