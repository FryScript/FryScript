using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BlockStatements
{
    [TestClass]
    public class ThrowStatementTests : IntegrationTestBase
    {
        [TestMethod]
        public void Throw_String()
        {
            try
            {
                Eval("throw \"error message\";");
            }
            catch (FryScriptException ex)
            {
                Assert.AreEqual("error message", ex.Message);
            }
        }

        [TestMethod]
        public void Throw_Built_In_Error()
        {
            Eval("@import \"error\" as error;");

            try
            {
                Eval("new error(\"boom\", { });");
            }
            catch (FryScriptException ex)
            {
                Assert.AreEqual("boom", ex.Message);
                Assert.IsInstanceOfType(ex.ScriptData, typeof(ScriptError));
            }
        }


        [TestMethod]
        public void Throw_Object_Literal_Uses_Message_Property()
        {
            try
            {
                Eval("var error = {message: \"error\"};");
                Eval("throw error;");
            }
            catch (FryScriptException ex)
            {
                Assert.AreEqual("error", ex.Message);
            }
        }

        [TestMethod]
        public void Throw_Error_Object_Literal_Populates_Exception_Script_Data()
        {
            try
            {
                Eval("var error = {};");
                Eval("throw error;");
            }
            catch (FryScriptException ex)
            {
                var expected = Eval("error;");
                Assert.AreEqual(expected, ex.ScriptData);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void Throw_As_Expression()
        {
            Eval(@"
                var x = 0;
                x += throw ""can't add here!"";
            ");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Throw_Rethrow_Outside_Catch()
        {
            Eval(@"
                throw;
            ");
        }

        [TestMethod]
        public void Throw_Rethrow_Inside_Catch()
        {

            try
            {
                Eval(@"
                    try
                    {
                        throw ""boom!"";
                    }
                    catch ex
                    {
                        throw;
                    }
                ");
            }
            catch (FryScriptException ex)
            {
                Assert.AreEqual("boom!", ex.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Throw_Null()
        {
            Eval(@"
                throw null;
            ");
        }
    }
}