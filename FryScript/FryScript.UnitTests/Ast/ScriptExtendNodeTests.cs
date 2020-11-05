using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptExtendNodeTests : AstNodeTestBase<ScriptExtendNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
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
        public void GetExpression_Extends_Script()
        {
            Node.StubCompilerContext();

            var nameNode = Node<AstNode>.WithValueString("script");

            Node.SetChildren(null, nameNode);

            var extendScript = new ScriptObject();
            Node.CompilerContext.ScriptRuntime.Get(
                "script",
                Node.CompilerContext.Uri)
                .Returns(extendScript);

            Scope.AddMember(Keywords.This, Node, typeof(IScriptObject));

            var result = Node.GetExpression(Scope) as MethodCallExpression;

            Assert.AreEqual(1, result.Arguments.Count);
            Assert.AreEqual(Scope.GetMemberExpression(Keywords.This), result.Arguments.Single());

            Assert.AreEqual(extendScript.GetType(), Node.CompilerContext.ScriptType);
            Assert.AreEqual(extendScript.ObjectCore.Builder, Node.CompilerContext.ScriptObjectBuilder);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetExpression_Throws_Compiler_Exception_In_Eval_Mode()
        {
            Node.StubParseNode();
            Node.StubCompilerContext(isEvalMode: true);

            Node.GetExpression(Scope);
        }
    }
}
