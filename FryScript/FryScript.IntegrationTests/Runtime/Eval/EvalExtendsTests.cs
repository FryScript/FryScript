using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalExtendsTests : IntegrationTestBase
    {
        [ScriptableType("importedObject")]
        public class ImportedObject
        {
        }

        [TestMethod]
        public void Script_A_Extends_Script_B()
        {
            Eval("@import \"Scripts/extendingScript\" as a;");
            Eval("@import \"Scripts/baseScript\" as b;");

            var result = Eval("a extends b;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Imported_Object_Extends_Object_Root()
        {
            ScriptRuntime.Import<ImportedObject>();

            Eval("@import \"importedObject\" as importedObject;");
            Eval("object = {};");

            var result = Eval("importedObject extends object;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void True_Extends_False()
        {
            var result = Eval("true extends false;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Float_Extends_Float()
        {
            var result = Eval("0.0 extends 0.0;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Int_Extends_Int()
        {
            var result = Eval("0 extends 0;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void String_Extends_String()
        {
            var result = Eval("\"\" extends \"\";");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void This_Extends_Object()
        {
            var Result = Eval("this extends ({});");

            Assert.IsTrue(Result);
        }

        [TestMethod]
        public void This_Extends_This()
        {
            var result = Eval("this extends this;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Float_Extends_Int()
        {
            var result = Eval("0.0 extends 0;");

            Assert.IsTrue(result);
        }
    }
}
