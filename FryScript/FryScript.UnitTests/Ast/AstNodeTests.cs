using FryScript.Ast;
using FryScript.Compilation;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    public class ProxyAstNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new System.NotImplementedException();
        }

        new public Expression GetChildExpression(Scope scope)
        {
            return base.GetChildExpression(scope);
        }

        new public T Transform<T>(params AstNode[] childNodes) where T : AstNode, new()
        {
            return (T)base.Transform<T>(childNodes);
        }
    }

    [TestClass]
    public class AstNodeTests : AstNodeTestBase<ProxyAstNode>
    {
        private ParseTreeNode _parseNode;
        private CompilerContext _compilerContext;

        public override void OnTestInitialize()
        {
            _parseNode = new ParseTreeNode(new Token(new Terminal("Test"), new SourceLocation(), "Test", "Test"));
            _compilerContext = new CompilerContext(Substitute.For<IScriptRuntime>(), new Uri("test://test"));
        }

        [TestClass]
        public class AstNodeTransformerTests
        {
            
        }

        [TestMethod]
        public void ValueString_Gets_Parse_Node_Value_String()
        {
            Node.ParseNode = _parseNode;
            Assert.AreEqual(Node.ParseNode.Token.ValueString, Node.ValueString);
        }

        [TestMethod]
        public void Value_Gets_Parse_Node_Value()
        {
            Node.ParseNode = _parseNode;
            Assert.AreEqual(Node.ParseNode.Token.Value, Node.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetIdentifier_Not_Implemented()
        {
            Node.GetIdentifier(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SetIdentifier_Not_Implemented()
        {
            Node.SetIdentifier(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateIdentifier_Not_Implemented()
        {
            Node.CreateIdentifier(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Init_Null_Context()
        {
            Node.Init(null, _parseNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Init_Null_Parse_Node()
        {
            Node.Init(_compilerContext, null);
        }

        [TestMethod]
        public void Init_Transforms_ParseNode_Children_To_Ast_Children()
        {
            _parseNode.AstNode = Node;
            _parseNode.AstNode = Node;

            Node.ParseNode = _parseNode;

            Node.ParseNode.ChildNodes.AddRange(new[]
            {
                _parseNode,
                _parseNode
            });

            Node.Init(_compilerContext, _parseNode);

            Assert.AreEqual(Node, Node.ChildNodes[0]);
            Assert.AreEqual(Node, Node.ChildNodes[1]);
        }

        [TestMethod]
        public void FindChild_Of_Given_Type()
        {
            Node.ChildNodes = new AstNode[]
            {
                new IdentifierExpressionNode()
                {
                    ChildNodes = new AstNode[]
                    {
                         new ObjectLiteralExpressionNode()
                    }
                }
            };

            var result = Node.FindChild<ObjectLiteralExpressionNode>();

            Assert.AreEqual(Node.ChildNodes[0].ChildNodes[0], result);
        }

        [TestMethod]
        public void FindChild_No_Match_Returns_Default()
        {
            Node.ChildNodes = new AstNode[0];

            var result = Node.FindChild<ObjectLiteralExpressionNode>();

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildExpression_Null_Scope()
        {
            Node.GetChildExpression(null);
        }

        [TestMethod]
        public void GetChildExpression_Single_Null_Child()
        {
            Node.SetChildren(new AstNode[] { null });

            var result = Node.GetChildExpression(Scope) as ConstantExpression;

            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void GetChildExpression_Single_Child_Calls_Child_Get_Expression()
        {
            var childExpr = Expression.Constant(true);
            var child = Node<AstNode>.Empty;

            child.GetExpression(childExpr);

            Node.SetChildren(child);

            var result = Node.GetChildExpression(Scope);

            Assert.AreEqual(childExpr, result);
        }

        [TestMethod]
        public void GetExpression_Multiple_Children_Calls_Multiple_Get_Expression()
        {
            var childExpr1 = Expression.Constant(new object());
            var childExpr2 = Expression.Constant(new object());

            var child1 = Node<AstNode>.Empty;
            child1.GetExpression(childExpr1);

            var child2 = Node<AstNode>.Empty;
            child2.GetExpression(childExpr2);

            Node.SetChildren(child1, child2);

            var result = Node.GetChildExpression(Scope) as BlockExpression;

            Assert.AreEqual(2, result.Expressions.Count);
            Assert.AreEqual(childExpr1, result.Expressions[0]);
            Assert.AreEqual(childExpr2, result.Expressions[1]);
        }

        [TestMethod]
        public void Transform_Transforms_Node()
        {
            var parseNode = new ParseTreeNode(new Token(new Terminal("test"), new SourceLocation(), "test", "test"));
            var compilerContext = new CompilerContext(Substitute.For<IScriptRuntime>(), new Uri("test://test"));
            var children = new[] { Node<AstNode>.Empty };

            Node.ParseNode = parseNode;
            Node.CompilerContext = compilerContext;
            Node.SetChildren(children);

            var result = Node.Transform<ProxyAstNode>(children);

            Assert.AreEqual(parseNode, result.ParseNode);
            Assert.AreEqual(compilerContext, result.CompilerContext);
            Assert.AreEqual(children, result.ChildNodes);
        }
    }
}
