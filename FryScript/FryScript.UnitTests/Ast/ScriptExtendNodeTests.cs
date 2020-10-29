using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptExtendNodeTests : AstNodeTestBase<ScriptExtendNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_Extend_Unavailable_In_Eval_Mode()
        {
            Node.StubCompilerContext(isEvalMode: true);
            Node.StubParseNode();

            Node.GetExpression(Scope);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            Assert.Fail();
        }
    }
}
