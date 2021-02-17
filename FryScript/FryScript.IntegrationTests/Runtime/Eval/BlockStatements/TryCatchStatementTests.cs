using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BlockStatements
{
    [TestClass]
    public class TryCatchStatementTests : IntegrationTestBase
    {
        [TestMethod]
        public void Try_Catch()
        {

            Eval(@"
            try
            {
                null.member;
            }
            catch error
            {
                this.errorMessage = error;
            }");

            Assert.IsNotNull(Eval("this.errorMessage;"));
        }

        [TestMethod]
        public void Try_Finally()
        {
            Eval(@"
            try
            {
                this.finallyFlag = false;
            }
            finally
            {
                this.finallyFlag = true;
            }
            ");

            Assert.IsTrue(Eval("this.finallyFlag;"));
        }

        [TestMethod]
        public void Try_Catch_Finally()
        {
            Eval(@"
            try
            {
                this.undefined();
            }
            catch error 
            {
                this.undefined = 100;
            }
            finally
            {
                this.undefined *= 5;
            }
            ");

            Assert.AreEqual(500, Eval("this.undefined;"));
        }

        [TestMethod]
        public void Try_Catch_Thrown_String()
        {
            Eval(@"
            try
            {
                throw ""boom"";
            }
            catch error
            {
                this.caughtError = error;
            }
            ");

            Assert.AreEqual("boom", Eval("this.caughtError;"));
        }

        [TestMethod]
        public void Try_Catch_Thrown_Object()
        {
            Eval(@"
            try
            {
                throw {};
            }
            catch error
            {
                this.caughtError = error;
            }
            ");

            Assert.IsInstanceOfType(Eval("this.caughtError;"), typeof(ScriptObject));
        }
    }
}