using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class EvalIsTests : IntegrationTestBase
    {
        [TestMethod]
        public void Script_A_Is_Script_B()
        {
            Eval("@import \"Scripts/baseScript\" as a;");
            Eval("@import \"Scripts/baseScript\" as b;");

            var result = Eval("a is b;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Script_B_Is_Not_Script_A()
        {
            Eval("@import \"Scripts/extendingScript\" as a;");
            Eval("@import \"Scripts/baseScript\" as b;");

            var result = Eval("b is a;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Null_Is_Other()
        {
            var result = Eval("null is 100;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Other_Is_Null()
        {
            var result = Eval("true is null;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Null_Is_Null()
        {
            var result = Eval("null is null;");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Int_Is_Int()
        {
            var result = Eval("100 is 900;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Float_Is_Float()
        {
            var result = Eval("1.0 is 2.0;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void String_Is_String()
        {
            var result = Eval("\"a\" is \"b\";");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Bool_Is_Bool()
        {
            var result = Eval("true is false;");

            Assert.IsTrue(result);
        }

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
