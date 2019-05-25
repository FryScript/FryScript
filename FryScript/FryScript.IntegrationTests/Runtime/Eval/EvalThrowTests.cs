using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalThrowTests : IntegrationTestBase
    {
        [TestMethod]
        public void Throw_Test()
        {
            try
            {
                Eval("throw \"error\";");
            }
            catch(FryScriptException ex )
            {
                Assert.AreEqual("error", ex.Message);
            }
        }

        [TestMethod]
        public void Throw_Error_Object_Uses_Message_Property()
        {
            try
            {
                Eval("var error = {message: \"error\"};");
                Eval("throw error;");
            }
            catch(FryScriptException ex)
            {
                Assert.AreEqual("error", ex.Message);
            }
        }

        [TestMethod]
        public void Throw_Error_Object_Populates_Exception_Script_Data()
        {
            try
            {
                Eval("var error = {};");
                Eval("throw error;");
            }
            catch(FryScriptException ex)
            {
                var expected = Eval("error;");
                Assert.AreEqual(expected, ex.ScriptData);
            }
        }
    }
}
