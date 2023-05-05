using FryScript.ScriptProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;

namespace FryScript.UnitTests.ScriptProviders
{
    [TestClass]
    public  class PathProviderTests
    {
        [DataTestMethod]
        [DataRow("/file1", "file1")]
        [DataRow("./file1", "file1")]
        [DataRow("~/file1", "file1")]
        public void FixPath_Returns_Fixed_Path(string path, string expected) 
        {
            var result = PathProvider.FixPath(path);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(@"\")]
        [DataRow("<")]
        [DataRow(">")]
        [DataRow(":")]
        [DataRow("\"")]
        [DataRow("?")]
        [DataRow("*")]
        [DataRow("|")]
        [DataRow("//")]
        [ExpectedException(typeof(ScriptPathException))]
        public void EnsurePath_Handles_Invalid_Characters(string path)
        {
            PathProvider.EnsurePath(path);
        }

        [DataTestMethod]
        [DataRow("root/file.test")]
        [DataRow("../file.test")]
        [DataRow("../dir.test/file.test")]
        [DataRow("file.test")]
        [ExpectedException(typeof(ScriptPathException))]
        public void EnsurePath_Handles_File_Extension(string path) 
        {
            PathProvider.EnsurePath(path);
        }

        [DataTestMethod]
        [DataRow("", "file1", "file1")]
        [DataRow("root/directory1/../file1", "../file2", "file2")]
        [DataRow("./root/directory1/file1", "../../file2", "file2")]
        [DataRow("./root/directory1/directory2/../file1", "../file2", "root/file2")]
        [DataRow("./root/directory1/directory2/../file1", "../file2", "root/file2")]
        public void Resolve_Relative_To_Another_Path(string path1, string path2, string expected)
        {
            var result = PathProvider.Resolve(path1, path2);
  
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("~/root/directory/file1", "root/directory/file1")]
        [DataRow("~/root/directory/../file1", "root/file1")]
        public void Resolve_Relative_To_Another_Path_Using_Root_Operator(string path, string expected)
        {
            var result = PathProvider.Resolve("relative/file1", path);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ScriptPathException))]
        public void Resolve_Handles_Traversing_Outside_Root()
        {
            PathProvider.Resolve("../");
        }

        [DataTestMethod]
        [DataRow("root/directory1/../directory2/file1", "root/directory2/file1")]
        [DataRow("root/directory1/directory2/directory3/direcrtory4/../directory5/file1", "root/directory1/directory2/directory3/directory5/file1")]
        public void Resolve_Travserses_Path(string path, string expected)
        {
            var result = PathProvider.Resolve(path);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("root/directory1/file1", "file1")]
        [DataRow("../file1", "file1")]
        [DataRow("file1", "file1")]
        public void GetFileName_Returns_The_File_Name(string path, string expected)
        {
            var result = PathProvider.GetFileName(path);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("/")]
        [DataRow("..")]
        [DataRow(".")]
        [DataRow("~/")]
        [DataRow("~")]
        [DataRow("root/")]
        [DataRow("root/directory1/")]
        [DataRow("root/..")]
        [DataRow("root/.")]
        [ExpectedException(typeof(ScriptPathException))]
        public void GetFileName_Handles_Missing_File_Name(string path)
        {
            PathProvider.GetFileName(path);
        }
    }
}
