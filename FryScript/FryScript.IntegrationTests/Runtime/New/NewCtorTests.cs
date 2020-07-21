using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.New
{
    [TestClass]
    public class NewTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Script()
        {
            var script = New("Scripts/newInstance");

            Assert.AreEqual("member1", script.member1);
        }

        [TestMethod]
        public void New_Script_With_Ctor()
        {
            IScriptObject newInstanceCtor = Get("Scripts/newInstanceCtor");
            var script = New("Scripts/newInstanceCtor");

            Assert.IsFalse(newInstanceCtor.HasMember("member1"));
            Assert.AreEqual("member1", script.member1);
        }

        [TestMethod]
        public void New_Script_With_Ctor_Params()
        {
            IScriptObject newInstanceCtorParams = Get("Scripts/newInstanceCtorParams");
            var script = New("Scripts/newInstanceCtorParams", "Member value");

            Assert.IsFalse(newInstanceCtorParams.HasMember("paramsMember"));
            Assert.AreEqual("Member value", script.member1);
        }

        [TestMethod]
        public void New_Script_With_Extending_Ctor_Params()
        {
            IScriptObject newExtendCtorParams = Get("Scripts/newExtendCtorParams");
            var script = New("Scripts/newExtendCtorParams", "a", 1);

            Assert.IsFalse(newExtendCtorParams.HasMember("member1"));
            Assert.IsFalse(newExtendCtorParams.HasMember("member2"));

            Assert.AreEqual("a", script.member1);
            Assert.AreEqual(1, script.member2);
        }

        [TestMethod]
        public void New_Operator_Script_Instance()
        {
            Eval("@import \"Scripts/newInstance\" as newInstance;");

            var script = Eval("new newInstance();");

            Assert.AreEqual("member1", script.member1);
        }

        [TestMethod]
        public void New_Operator_Script_Instance_With_Ctor_Params()
        {
            Eval("@import \"Scripts/newInstanceCtorParams\" as newInstanceCtorParams;");

            var script = Eval("new newInstanceCtorParams(100);");

            Assert.AreEqual(100, script.member1);
        }

        [TestMethod]
        public void New_Script_Instance_With_Extending_Ctor_Params()
        {
            IScriptObject newExtendCtorParams = Get("Scripts/newExtendCtorParams");
            Eval("@import \"Scripts/newExtendCtorParams\" as newExtendCtorParams;");
            var script = Eval("new newExtendCtorParams(\"a\", 1);");

            Assert.IsFalse(newExtendCtorParams.HasMember("member1"));
            Assert.IsFalse(newExtendCtorParams.HasMember("member2"));

            Assert.AreEqual("a", script.member1);
            Assert.AreEqual(1, script.member2);
        }
    }
}
