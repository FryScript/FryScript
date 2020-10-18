using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Debugging;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    public class AstNodeTestBase<T> where T : AstNode, new()
    {
        public T Node { get; private set; }

        public Scope Scope { get; set; } = new Scope();

        [TestInitialize]
        public void TestInitialize()
        {
            Node = Substitute.ForPartsOf<T>();

            OnTestInitialize();
        }

        public virtual void OnTestInitialize()
        {

        }

        public void TestSingleChildNode()
        {
            Node.SetChildren(Node<AstNode>.Empty);

            var expr = Expression.Empty();

            Node.Configure().GetChildExpression(Scope).Returns(expr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expr, result);
        }

        public void StubParseNode(string valueString = "", object value = null, int position = 0, int line = 0, int column = 0)
        {
            Node.ParseNode = new ParseTreeNode(
                new Token(
                    new Terminal("test"),
                    new SourceLocation(position, line, column),
                    valueString,
                    value
                    ));
        }

        public void StubCompilerContext(DebugHook debugHook = null, bool detailedExceptions = false)
        {
            var runtime = Substitute.For<IScriptRuntime>();
            runtime.DebugHook = debugHook;
            runtime.DetailedExceptions = detailedExceptions;

            Node.CompilerContext = new CompilerContext(
                runtime,
                new Uri("test://test"));
        }
    }
}