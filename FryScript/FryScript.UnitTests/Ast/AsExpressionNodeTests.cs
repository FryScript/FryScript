using System;
using System.Linq.Expressions;
using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;

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
        public override void GetExpression_Single_Child_Gets_Child_Expression()
        {
            base.GetExpression_Single_Child_Gets_Child_Expression();
        }

        [TestMethod]
        public void GetExpression_As_Variable()
        {
            var left = Node<AstNode>.WithValue(new object());
            var op = Node<AstNode>.WithValueString(Keywords.As);
            var right = Node<IdentifierNode>
                            .WithValueString("test")
                            .WithValue(new object());

            Node.SetChildren(left, op, right);

            Node.GetExpression(Scope);

            right.Received().SetIdentifier(Scope, Arg.Any<Expression>());
            left.Received().GetExpression(Scope);
        }

        [TestMethod]
        public void GetExpression_As_Tuple()
        {
            var identifiers = Node<AstNode>.WithValue(new object());
            var op = Node<AstNode>.WithValueString(Keywords.As);
            var values = Node<TupleNamesNode>.WithValue(new object());

            Node.SetChildren(values, op, identifiers);

            var tupleNameValues = Node<AssignTupleExpressionNode>.Empty;

            Node.Configure()
                .Transform<AssignTupleExpressionNode>(Arg.Is<AstNode[]>(a => a[0] == values))
                .Returns(tupleNameValues);

            var declareTupleExpr = Expression.Constant(true);
            var declareTuple = Node<TupleDeclarationNode>.Empty.GetExpression(declareTupleExpr);
            Node.Configure()
                .Transform<TupleDeclarationNode>(Arg.Is<AstNode[]>(a => 
                a[0] == null
                && a[1] == identifiers
                && a[2] == null
                && a[3] == tupleNameValues
            )).Returns(declareTuple);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(declareTupleExpr, result);
            declareTuple.Received().GetExpression(Scope);
        }
    }
}