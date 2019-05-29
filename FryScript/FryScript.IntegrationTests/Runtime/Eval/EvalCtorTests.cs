using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalCtorTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Instance_From_Imported_Script()
        {
            Eval("@import \"Scripts/newInstance\" as newInstance;");

            var result = Eval("new newInstance();");

            Assert.AreEqual("member1", result.member1);
        }

        [TestMethod]
        public void New_Instance_From_Imported_Script_With_Ctor()
        {
            Eval("@import \"Scripts/newInstanceCtor\" as newInstanceCtor;");

            IScriptObject newInstanceCtor = Eval("newInstanceCtor;");
            var result = Eval("new newInstanceCtor();");

            Assert.IsFalse(newInstanceCtor.HasMember("member1"));
            Assert.AreEqual("member1", result.member1);
        }

        [TestMethod]
        public void New_Instance_From_Imported_Script_With_Ctor_Params()
        {
            Eval("@import \"Scripts/newInstanceCtorParams\" as newInstanceCtorParams;");

            IScriptObject newInstanceCtorParams = Eval("newInstanceCtorParams;");
            var result = Eval("new newInstanceCtorParams(1000);");

            Assert.IsFalse(newInstanceCtorParams.HasMember("member1"));
            Assert.AreEqual(1000, result.member1);
        }

        [TestMethod]
        public void New_Instance_From_Imported_Script_With_Extended_Ctor_Params()
        {
            Eval("@import \"Scripts/newExtendCtorParams\" as newExtendCtorParams;");

            IScriptObject newExtendCtorParams = Eval("newExtendCtorParams;");
            var result = Eval("new newExtendCtorParams(true, 90);");

            Assert.IsFalse(newExtendCtorParams.HasMember("member1"));
            Assert.IsFalse(newExtendCtorParams.HasMember("member2"));
            Assert.AreEqual(true, result.member1);
            Assert.AreEqual(90, result.member2);
        }
    }
}
