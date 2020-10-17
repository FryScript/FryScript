using FryScript.Ast;
using FryScript.Compilation;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests.Ast
{
    public class AstNodeTestBase<T> where T : AstNode, new()
    {
        public T Node { get; private set; }

        public Scope Scope { get; set; } = new Scope();

        public AstNode.AstNodeTransformer NodeTransformer { get; private set; }

        public AstNode.GetChildExpressionVisitor ChildExpressionVisitor { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Node = new T();
          
            NodeTransformer = Substitute.For<AstNode.AstNodeTransformer>();
            ChildExpressionVisitor = Substitute.For<AstNode.GetChildExpressionVisitor>();

            Node.NodeTransformer = NodeTransformer;
            Node.ChildExpressionVisitor = ChildExpressionVisitor;

            OnTestInitialize();
        }

        public virtual void OnTestInitialize()
        {

        }

        public void StubParseNode(string valueString = "", object value = null, int position = 0, int line = 0,  int column = 0)
        {
            Node.ParseNode = new ParseTreeNode(
                new Token(
                    new Terminal("test"),
                    new SourceLocation(position, line, column),
                    valueString,
                    value
                    ));
        }

        public void StubCompilerContext()
        {
            Node.CompilerContext = new CompilerContext(
                Substitute.For<IScriptRuntime>(),
                new Uri("test://test"));
        }
    }
}