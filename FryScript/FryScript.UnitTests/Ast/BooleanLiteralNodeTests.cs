using FryScript.Ast;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class BooleanLiteralTests : AstNodeTestBase<BooleanLiteralNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Parses_Boolean_String()
        {
            var valueNode = Node<AstNode>.Empty;
            valueNode.Value.Returns("true");

            Node.ParseNode = new ParseTreeNode(new Token(new Terminal("test"), new SourceLocation(), "true", "true"));
            Node.ParseNode.ChildNodes.Add(new ParseTreeNode(new Token(new Terminal("test"), new SourceLocation(), "true", "true")));

            var result = Node.GetExpression(Scope) as UnaryExpression;
            var operand = result.Operand as ConstantExpression;

            Assert.AreEqual(ExpressionType.Convert, result.NodeType);
            Assert.IsTrue((bool)operand.Value);
        }
    }
}