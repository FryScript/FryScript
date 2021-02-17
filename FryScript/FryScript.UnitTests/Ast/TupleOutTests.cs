using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TupleOutTests : AstNodeTestBase<TupleOut>
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetExpression_Throws_Not_Implemented_Exception()
        {
            Node.GetExpression(Scope);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Scope()
        {
            Node.SetIdentifier(null, Expression.Empty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Value()
        {
            Node.SetIdentifier(Scope, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetIdentifier_No_Tuple_Out_Scope_Data()
        {
            Scope.RemoveData(ScopeData.TupleOut);

            Node.SetIdentifier(Scope, Expression.Empty());
        }

        [TestMethod]
        public void SetIdentifier_Assigns_Tuple_Out()
        {
            var outExpr = Scope.AddMember(Keywords.Out, Node);

            Scope.SetData(ScopeData.TupleOut, outExpr);

            var expectedExpr = Expression.Constant(new object());

            var result = Node.SetIdentifier(Scope, expectedExpr) as BinaryExpression;

            Assert.AreEqual(ExpressionType.Assign, result.NodeType);
            Assert.AreEqual(outExpr, result.Left);
            Assert.AreEqual(expectedExpr, result.Right);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateIdentifier_Null_Scope()
        {
            Node.CreateIdentifier(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void CreateIdentifier_Out_Already_Defined()
        {
            Scope.SetData(ScopeData.TupleOut, new object());

            Node
                .StubCompilerContext()
                .StubParseNode();

            Node.CreateIdentifier(Scope);
        }

        [TestMethod]
        public void CreateIdentifier_Sets_Scope_Data()
        {
            Node.CreateIdentifier(Scope);

            var expectedExpr = Scope.GetAllowedMembers(true).First().Parameter;
            Scope.TryGetData(ScopeData.TupleOut, out ParameterExpression actualExpr);

            Assert.AreEqual(expectedExpr, actualExpr);
        }
    }
}
