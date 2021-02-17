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

            Assert.IsTrue(_provider.TryGetScriptInfo("./Scripts/errorHandling1.fry", out ScriptInfo scriptInfo));
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
            var relativeTo = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "noFile.fry"));

            Assert.IsFalse(_provider.TryGetScriptInfo("simpleImport.fry", out ScriptInfo scriptInfo, relativeTo));
            Assert.IsNull(scriptInfo);
        }

        [TestMethod]
        public void TryGetScriptInfo_Relative_To_Does_Exist()
        {
            _provider = new DirectoryScriptProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "simpleImport.fry"));
            var expectedSource = File.ReadAllText(expectedUri.LocalPath);
            var relativeTo = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry"));

            Assert.IsTrue(_provider.TryGetScriptInfo("simpleImport.fry", out ScriptInfo scriptInfo, relativeTo));
            Assert.AreEqual(expectedUri, scriptInfo.Uri);
            Assert.AreEqual(expectedSource, scriptInfo.Source);
        }

        [TestMethod]
        public void TryGetScriptInfo_Relative_To_Searches_Up_Directory_Tree()
        {
            var expectedUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "outside.txt"));
            var expectedSource = File.ReadAllText(expectedUri.LocalPath);
            var relativeTo = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "errorHandling1.fry"));

            Assert.IsTrue(_provider.TryGetScriptInfo("outside.txt", out ScriptInfo scriptInfo, relativeTo));
            Assert.AreEqual(expectedUri, scriptInfo.Uri);
            Assert.AreEqual(expectedSource, scriptInfo.Source);
        }
    }
}
