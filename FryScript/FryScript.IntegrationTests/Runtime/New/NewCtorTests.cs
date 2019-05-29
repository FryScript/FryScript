using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.New
{
    [TestClass]
    public class NewTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Script()
        {
            dynamic script = New("Scripts/newInstance");

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
    }
}
