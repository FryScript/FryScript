using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.IsOperation
{
    [TestClass]
    public class IsOperationTests : IntegrationTestBase
    {
        [DataTestMethod]
        [DataRow("object")]
        [DataRow("array")]
        [DataRow("error")]
        [DataRow("function")]
        [DataRow("fibre")]
        [DataRow("fibre-context", "fibreContext")]
        public void ScriptObject_Is_Same_ScriptObject_Type(string type, string arg = null)
        {
            arg = arg ?? type;

            Eval($"@import \"{type}\" as {arg};");

            var result = Eval($"{type} is {arg};");

            Assert.IsTrue(result);
        } 

        [DataTestMethod]
        [DataRow("object", "array")]
        [DataRow("object", "error")]
        [DataRow("object", "function")]
        [DataRow("object", "fibre", "f")]
        [DataRow("object", "fibre-context", "fibreContext")]
        public void ScriptObject_Is_Not_Other_ScritpObject_Type(string type1, string type2, string arg = null)
        {
            arg = arg ?? type2;

             Eval($"@import \"{type1}\" as {type1};");
             Eval($"@import \"{type2}\" as {arg};");

            var result = Eval($"{type1} is {arg};");

            Assert.IsFalse(result);
        }   

        [TestMethod]
        public void ScriptObject_Is_Non_ScriptObject_Type()
        {
            Assert.Fail("Write this test!");
        }
    }
}