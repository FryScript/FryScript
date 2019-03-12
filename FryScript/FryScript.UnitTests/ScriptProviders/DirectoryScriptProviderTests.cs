﻿using FryScript.ScriptProviders;
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryGetScriptInfo_Invalid_Path(string path)
        {
            _provider.TryGetScriptInfo(path, out ScriptInfo scriptInfo);
        }

        [TestMethod]
        public void TryGetScriptInfo_Path_Does_Not_Exist()
        {
            Assert.IsFalse(_provider.TryGetScriptInfo("nofile.fry", out ScriptInfo sciprtInfo));
            Assert.IsNull(sciprtInfo);
        }

        [TestMethod]
        public void TryGetScriptInfo_Path_Does_Exist()
        {
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry"));
            var expectedSource = File.ReadAllText(expectedUri.LocalPath);

            Assert.IsTrue(_provider.TryGetScriptInfo("./scripts/errorHandling1.fry", out ScriptInfo scriptInfo));
            Assert.AreEqual(expectedUri, scriptInfo.Uri);
            Assert.AreEqual(expectedSource, scriptInfo.Source);
        }

        [TestMethod]
        public void TryGetScriptInfo_Cannot_Search_Outside_Root()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));

            Assert.IsFalse(_provider.TryGetScriptInfo("../outside.txt", out ScriptInfo scriptInfo));
            Assert.IsNull(scriptInfo);
        }

        [TestMethod]
        public void TryGetScriptInfo_Relative_To_Does_Not_Exist()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));
            var relativeTo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "noFile.fry");

            Assert.IsFalse(_provider.TryGetScriptInfo("simpleImport.fry", out ScriptInfo scriptInfo, relativeTo));
            Assert.IsNull(scriptInfo);
        }
    }
}
