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
        [DataRow("fibre", "f")]
        [DataRow("fibre-context", "fibreContext")]
        public void ScriptObject_Is_Same_ScriptObject_Type(string type, string arg = null)
        {
            arg = arg ?? type;

            Eval($"@import \"{type}\" as {arg};");

            var result = Eval($"{arg} is {arg};");

            Assert.IsTrue(result);
        } 

        [DataTestMethod]
        [DataRow("object", "array")]
        [DataRow("object", "error")]
        [DataRow("object", "function")]
        [DataRow("object", "fibre", null, "f")]
        [DataRow("object", "fibre-context", null, "fibreContext")]
        public void ScriptObject_Is_Not_Other_ScritpObject_Type(string type1, string type2, string arg1 = null, string arg2 = null)
        {
            arg1 = arg1 ?? type1;
            arg2 = arg2 ?? type2;

            Eval($"@import \"{type1}\" as {arg1};");
            Eval($"@import \"{type2}\" as {arg2};");

            var result = Eval($"{arg1} is {arg2};");

            Assert.IsFalse(result);
        }   

        [DataTestMethod]
        [DataRow("object", "int")]
        [DataRow("object", "boolean")]
        [DataRow("object", "single")]
        [DataRow("object", "string")]
        public void ScriptObject_Is_Not_Primitive_Type(string type, string arg)
        {
            Eval($"@import \"{type}\" as arg;");

            var result = Eval($"{type} is {arg};");

            Assert.IsFalse(result);
        }
    }
}