using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute.Extensions;
using System;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;
using FryScript.Compilation;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TupleNamesNodeTests : AstNodeTestBase<TupleNamesNode>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Not_Implemented()
        {
            Node.GetExpression(Scope);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Getdentifiers_Null_Scope()
        {
            Node.GetIdentifiers(null, new List<AstNode>());
        }

        [TestMethod]
        public void GetIdentifiers_Handles_First_And_Second_Nodes()
        {
            var nodes = new List<AstNode>();

            Node.Configure()
                .WhenForAnyArgs(n => n.HandleFirstNode(default, default))
                .DoNotCallBase();

            Node.Configure()
                .WhenForAnyArgs(n => n.HandleSecondNode(default))
                .DoNotCallBase();

            Node.GetIdentifiers(Scope, nodes);

            Node.Received().HandleFirstNode(Scope, nodes);
            Node.Received().HandleSecondNode(nodes);
        }

        [TestMethod]
        public void HandleFirstNode_Tuple_Names_Node()
        {
            var nodes = new List<AstNode>();

            var tupleNamesNode = Node<TupleNamesNode>.Empty;

            Node.SetChildren(tupleNamesNode);

            Node.HandleFirstNode(Scope, nodes);

            tupleNamesNode.Received().GetIdentifiers(Scope, nodes);
        }

        [TestMethod]
        public void HandleFirstNode_Identifier_Expression_Node()
        {
            var nodes = new List<AstNode>();
            var firstChild = Node<AstNode>.Empty;
            var identifierExpressionNode = Node<IdentifierExpressionNode>.Empty;

            firstChild.SetChildren(identifierExpressionNode);

            Node.SetChildren(firstChild);

            Node.HandleFirstNode(Scope, nodes);

            Assert.AreEqual(identifierExpressionNode, nodes.Single());
        }

        [TestMethod]
        public void HandleFirstNode_Tuple_Out_Node()
        {
            var nodes = new List<AstNode>();

            var tupleOut = Node<TupleOut>.Empty;

            Node.SetChildren(tupleOut);

            Node.HandleFirstNode(Scope, nodes);

            Assert.AreEqual(tupleOut, nodes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void HandleFirstNode_Invalid_Node()
        {
            var nodes = new List<AstNode>();

            var firstNode = Node<AstNode>.Empty;
            firstNode
                .StubParseNode()
                .StubCompilerContext();

            Node.SetChildren(firstNode);

            Node.HandleFirstNode(Scope, nodes);
        }

        [TestMethod]
        public void HandleSecondNode_Identifier_Expression()
        {
            var nodes = new List<AstNode>();

            var secondNode = Node<AstNode>.Empty;
            var identifierExpressionNode = Node<IdentifierExpressionNode>.Empty;

            secondNode.SetChildren(identifierExpressionNode);

            Node.SetChildren(null, secondNode);

            Node.HandleSecondNode(nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(identifierExpressionNode, nodes.Single());
        }

        [TestMethod]
        public void HandleSecondNode_Tuple_Out()
        {
            var nodes = new List<AstNode>();

            var tupleOut = Node<TupleOut>.Empty;

            Node.SetChildren(null, tupleOut);

            Node.HandleSecondNode(nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(tupleOut, nodes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void HandleSecondNode_Invalid_Node()
        {
            var nodes = new List<AstNode>();

            var secondNode = Node<AstNode>.Empty;
            secondNode.StubCompilerContext()
                .StubParseNode();

            Node.SetChildren(null, secondNode);

            Node.HandleSecondNode(nodes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeclareVariables_Null_Scope()
        {
            Node.DeclareVariables(null);
        }

        [TestMethod]
        public void DeclareVariables_Declares_Identifiers()
        {
            var identifierNode1 = Node<IdentifierNode>.Empty;
            var identifierNode2 = Node<IdentifierNode>.Empty;

            var identifiers = new List<IdentifierNode>
            {
                identifierNode1,
                identifierNode2
            };

            Node.Configure()
                .DeclareVariables(Scope, Arg.Any<List<IdentifierNode>>())
                .Returns(identifiers);

            Node.DeclareVariables(Scope);

            identifierNode1.Received().CreateIdentifier(Scope);
            identifierNode2.Received().CreateIdentifier(Scope);
        }

        [TestMethod]
        public void DeclareVariables_Declares_First_And_Second_Node_Variables()
        {
            var nodes = new List<IdentifierNode>();

            Node.Configure()
                .WhenForAnyArgs(n => n.DeclareFirstNode(default, default))
                .DoNotCallBase();

            Node.Configure()
                .WhenForAnyArgs(n => n.DeclareSecondNode(default))
                .DoNotCallBase();

            Node.DeclareVariables(Scope, nodes);

            Node.Received().DeclareFirstNode(Scope, nodes);
            Node.Received().DeclareSecondNode(nodes);
        }

        [TestMethod]
        public void DeclareFiristNode_Tuple_Names()
        {
            var nodes = new List<IdentifierNode>();

            var tupleNamesNode = Node<TupleNamesNode>.Empty;

            Node.SetChildren(tupleNamesNode);

            Node.DeclareFirstNode(Scope, nodes);

            tupleNamesNode.Received().DeclareVariables(Scope, nodes);
        }

        [TestMethod]
        public void DeclareFirstNode_Identifier()
        {
            var nodes = new List<IdentifierNode>();

            var firstNode = Node<AstNode>.Empty;
            var identifierNode = Node<IdentifierNode>.Empty;

            firstNode.SetChildren(identifierNode);

            Node.SetChildren(firstNode);

            Node.DeclareFirstNode(Scope, nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(identifierNode, nodes.Single());
        }

        [TestMethod]
        public void DeclareFirstNode_Tuple_Out()
        {
            var nodes = new List<IdentifierNode>();

            var tupleOut = Node<TupleOut>.Empty;

            Node.SetChildren(tupleOut);

            Node.DeclareFirstNode(Scope, nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(tupleOut, nodes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void DeclareFirstNode_Invalid_Node()
        {
            var nodes = new List<IdentifierNode>();

            var firstNode = Node<AstNode>.Empty;
            firstNode
                .StubCompilerContext()
                .StubParseNode();

            Node.SetChildren(firstNode);

            Node.DeclareFirstNode(Scope, nodes);
        }

        [TestMethod]
        public void DeclareSecondNode_Identifier_Node()
        {
            var nodes = new List<IdentifierNode>();

            var secondNode = Node<AstNode>.Empty;
            var identifierNode = Node<IdentifierNode>.Empty;

            secondNode.SetChildren(identifierNode);

            Node.SetChildren(null, secondNode);

            Node.DeclareSecondNode(nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(identifierNode, nodes.Single());
        }

        [TestMethod]
        public void DeclareSecondNode_Tuple_Out()
        {
            var nodes = new List<IdentifierNode>();

            var tupleOut = Node<TupleOut>.Empty;

            Node.SetChildren(null, tupleOut);

            Node.DeclareSecondNode(nodes);

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(tupleOut, nodes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void DeclareSecondNode_Invalid_Node()
        {
            var node = Node<AstNode>.Empty;

            node.
                StubCompilerContext()
                .StubParseNode();

            Node.SetChildren(null, node);

            Node.DeclareSecondNode(new List<IdentifierNode>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTuple_Null_Scope()
        {
            Node.CreateTuple(null);
        }

        [TestMethod]
        public void CreateTuple_Generates_New_Tuple()
        {
            var expectedExprs = new List<Expression>
            {
                Expression.Constant(new object()),
                Expression.Constant(new object())
            };

            Node.Configure()
                .GetTupleArgs(Scope)
                .Returns(expectedExprs);

            var result = Node.CreateTuple(Scope) as UnaryExpression;

            Assert.AreEqual(ExpressionType.Convert, result.NodeType);
            Assert.AreEqual(typeof(object), result.Type);

            var newExpr = result.Operand as NewExpression;

            Assert.AreEqual(typeof(ScriptTuple), newExpr.Type);
            Assert.AreEqual(1, newExpr.Arguments.Count);

            var arrayArg = newExpr.Arguments.Single() as NewArrayExpression;
            Assert.IsTrue(arrayArg.Expressions.SequenceEqual(expectedExprs));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTupleArgs_Null_Scope()
        {
            Node.GetTupleArgs(null, new List<Expression>());
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetTupleArgs_Invalid_TupleOut_First_Node()
        {
            Node.AllowOut = false;

            Node.SetChildren(
                Node<TupleOut>.Empty
                    .StubParseNode()
                    .StubCompilerContext(),
                Node<AstNode>.Empty);

            Node.GetTupleArgs(Scope);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetTupleArgs_Invalid_TupleOut_Second_Node()
        {
            Node.AllowOut = false;

            Node.SetChildren(
                Node<AstNode>.Empty,
                Node<TupleOut>.Empty
                    .StubParseNode()
                    .StubCompilerContext());

            Node.GetTupleArgs(Scope);
        }

        [TestMethod]
        public void GetTupleArgs_First_Node_Is_Tuple_Names_Node()
        {
            var exprs = new List<Expression>();

            var firstNode = Node<TupleNamesNode>.Empty;
            firstNode.GetTupleArgs(Scope, exprs);

            var expectedExpr = Expression.Empty();
            var secondNode = Node<AstNode>.Empty;
            secondNode.GetExpression(Scope).Returns(expectedExpr);

            Node.SetChildren(firstNode, secondNode);

            var result = Node.GetTupleArgs(Scope, exprs);

            firstNode.Received().GetTupleArgs(Scope, exprs);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedExpr, exprs[0]);
        }

        [TestMethod]
        public void GetTupleArgs_First_Node_Is_Not_Tuple_Names_Node()
        {
            var exprs = new List<Expression>();

            var expectedFirstExpr = Expression.Empty();
            var firstNode = Node<AstNode>.Empty;
            firstNode.GetExpression(Scope).Returns(expectedFirstExpr);

            var expectedSecondExpr = Expression.Empty();
            var secondNode = Node<AstNode>.Empty;
            secondNode.GetExpression(Scope).Returns(expectedSecondExpr);

            Node.SetChildren(firstNode, secondNode);

            var result = Node.GetTupleArgs(Scope, exprs);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedFirstExpr, exprs[0]);
            Assert.AreEqual(expectedSecondExpr, exprs[1]);
        }
    }
}
