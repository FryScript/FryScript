using System;
using System.Linq.Expressions;
using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AsExpressionNodeTests : AstNodeTestBase<AsExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Single_Child_Gets_Child_Expression()
        {
            Node.SetChildren(Node<AstNode>.Empty);

            var scope = new Scope();
            var expr = Expression.Empty();

            ChildExpressionVisitor.GetExpression(Node, scope).Returns(expr);

            var result = Node.GetExpression(scope);

            Assert.AreEqual(expr, result);
        }

        [TestMethod]
        public void GetExpression_As_Variable()
        {
            var left = Node<AstNode>.WithValue(new object());
            var op = Node<AstNode>.WithValueString(Keywords.As);
            var right = Node<IdentifierNode>
                            .WithValueString("test")
                            .WithValue(new object());

            var scope = new Scope();

            Node.SetChildren(left, op, right);

            Node.GetExpression(scope);

            right.Received().SetIdentifier(scope, Arg.Any<Expression>());
            left.Received().GetExpression(scope);
        }

        [TestMethod]
        public void GetExpression_As_Tuple()
        {
            var identifiers = Node<AstNode>.WithValue(new object());
            var op = Node<AstNode>.WithValueString(Keywords.As);
            var values = Node<TupleNamesNode>.WithValue(new object());

            Node.SetChildren(values, op, identifiers);

            var tupleNameValues = Node<AssignTupleExpressionNode>.Empty;

            NodeTransformer.Transform<AssignTupleExpressionNode>(Arg.Any<ParseTreeNode>(), Arg.Any<CompilerContext>(), Arg.Is<AstNode[]>(a => a[0] == values)).Returns(tupleNameValues);

            var declareTupleExpr = Expression.Constant(true);
            var declareTuple = Node<TupleDeclarationNode>.Empty.GetExpression(declareTupleExpr);
            NodeTransformer.Transform<TupleDeclarationNode>(Arg.Any<ParseTreeNode>(), Arg.Any<CompilerContext>(), Arg.Is<AstNode[]>(a => 
                a[0] == null
                && a[1] == identifiers
                && a[2] == null
                && a[3] == tupleNameValues
            )).Returns(declareTuple);

            var scope = new Scope();

            var result = Node.GetExpression(scope);

            Assert.AreEqual(declareTupleExpr, result);
            declareTuple.Received().GetExpression(scope);
        }
    }
}