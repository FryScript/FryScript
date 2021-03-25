using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class CommentsTests : IntegrationTestBase
    {
        [TestMethod]
        public void Comment_Single_Line()
        {
            try
            {
                Eval("// A comment");
            }
            catch (ParserException)
            {
                Assert.Fail("Comment should not cause a parser exception");
            }
        }

        [TestMethod]
        public void Comment_Block()
        {
            try
            {
                Eval(@"
                /*
                    A comment block
                */
            ");
            }
            catch (ParserException)
            {
                Assert.Fail("Comment block should not cause a parser exception");
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            Eval("crunk = null;");

            var r = Eval("crunk == null || crunk.completed;");
        }
    }
}
