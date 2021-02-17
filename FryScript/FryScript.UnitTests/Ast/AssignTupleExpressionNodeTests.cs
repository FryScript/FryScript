using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AssignTupleExpressionNodeTests : AstNodeTestBase<AssignTupleExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Single_Child_Returns_Tuple()
        {
            var createTupleExpr = Expression.Constant(true);
            var tupleNamesNode = Node<TupleNamesNode>.Empty;
            
            tupleNamesNode.CreateTuple(Scope).Returns(createTupleExpr);

            Node.SetChildren(tupleNamesNode);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(createTupleExpr, result);
        }

        [TestMethod]
        public void GetExpression_No_Out_Parameter()
        {
            var identifierNodes = new[]
            {
                Node<IdentifierNode>.Empty
            };

            var setIdentifierExpr = Expression.Parameter(typeof(object));
            identifierNodes[0]
                .SetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope), Arg.Any<Expression>())
                .Returns(setIdentifierExpr);

            var tupleNamesNode = Node<TupleNamesNode>.Empty;
            tupleNamesNode
                .GetIdentifiers(Arg.Is<Scope>(s => s.Parent == Scope), Arg.Any<List<AstNode>>())
                .Returns(identifierNodes);

            var right = Node<AstNode>.WithValue(new object());

            Node.SetChildren(tupleNamesNode, null, right);

            var result = Node.GetExpression(Scope);

            Assert.IsInstanceOfType(result, typeof(BlockExpression));
        }

        [TestMethod]
        public void GetExpression_With_Out_Parameter()
        {
            var tupleOutParamExpr = Expression.Parameter(typeof(object), "test");
            Scope.SetData<ParameterExpression>(ScopeData.TupleOut, tupleOutParamExpr);

            var identifierNodes = new[]
            {
                Node<IdentifierNode>.Empty
            };
            
            var setIdentifierExpr = Expression.Parameter(typeof(object));
            identifierNodes[0]
                .SetIdentifier(Arg.Is<Scope>(s => s.Parent == Scope), Arg.Any<Expression>())
                .Returns(setIdentifierExpr);

            var tupleNamesNode = Node<TupleNamesNode>.Empty;
            tupleNamesNode
                .GetIdentifiers(Arg.Is<Scope>(s => s.Parent == Scope), Arg.Any<List<AstNode>>())
                .Returns(identifierNodes);

            var right = Node<AstNode>.WithValue(new object());

            Node.SetChildren(tupleNamesNode, null, right);
            Node.AllowOut = true;

            var result = Node.GetExpression(Scope);

            Assert.IsInstanceOfType(result, typeof(BlockExpression));
            Assert.IsFalse(Scope.TryGetData<ParameterExpression>(ScopeData.TupleOut, out tupleOutParamExpr));
        }
    }
}
