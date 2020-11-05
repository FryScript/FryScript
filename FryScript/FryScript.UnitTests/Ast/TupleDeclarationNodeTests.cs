using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TupleDeclarationNodeTests : AstNodeTestBase<TupleDeclarationNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Declares_But_Does_Not_Assign_Values()
        {
            var tupleNamesNode = Node<TupleNamesNode>.Empty;

            Node.SetChildren(null, tupleNamesNode);
            Node.AllowOut = true;

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.IsNull(result.Value);
            Assert.IsTrue(tupleNamesNode.AllowOut);   
            tupleNamesNode.Received().DeclareVariables(Scope);
        }

        [TestMethod]
        public void GetExpression_Declares_And_Assigns_Values()
        {
            var tupleNamesNode = Node<TupleNamesNode>.Empty;
            var valueNode = Node<AstNode>.Empty;
            var assignTupleNode = Node<AssignTupleExpressionNode>.Empty;

            Node.SetChildren(null, tupleNamesNode, valueNode);
            Node.AllowOut = true;

            Node.Configure()
                .Transform<AssignTupleExpressionNode>(
                Arg.Is<AstNode[]>(a => a[0] == tupleNamesNode
                && a[1] == valueNode))
                .Returns(assignTupleNode);
            
            var assignTupleExpr = Expression.Constant(new object());
            assignTupleNode.GetExpression(assignTupleExpr, Scope);

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.AreEqual(assignTupleExpr, result);
            Assert.IsTrue(tupleNamesNode.AllowOut);
            tupleNamesNode.Received().DeclareVariables(Scope);
        }
    }
}
