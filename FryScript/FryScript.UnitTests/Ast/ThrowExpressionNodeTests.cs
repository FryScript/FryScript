using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ThrowExpressionNodeTests : AstNodeTestBase<ThrowExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Rethrows_Exception()
        {
            var throwNode = Node<AstNode>.Empty;

            var expectedExEpr = Expression.Constant(new Exception());
            Node.Configure()
                .HandleRethrow(Scope)
                .Returns(expectedExEpr);

            var expectedWrappedExpr = Expression.Constant(new Exception());
            Node.Configure()
                .WrapException(Scope, expectedExEpr, true)
                .Returns(expectedWrappedExpr);

            Node.SetChildren(throwNode);

            var result = Node.GetExpression(Scope) as UnaryExpression;

            Assert.AreEqual(ExpressionType.Throw, result.NodeType);
            Assert.AreEqual(expectedWrappedExpr, result.Operand);
        }

        [TestMethod]
        public void GetExpression_Throws_Exception()
        {
            var throwNode = Node<AstNode>.Empty;
            var exceptionNode = Node<AstNode>.Empty;

            var expectedExEpr = Expression.Constant(new Exception());
            Node.Configure()
                .HandleThrow(Scope)
                .Returns(expectedExEpr);

            var expectedWrappedExpr = Expression.Constant(new Exception());
            Node.Configure()
                .WrapException(Scope, expectedExEpr, false)
                .Returns(expectedWrappedExpr);

            Node.SetChildren(throwNode, exceptionNode);

            var result = Node.GetExpression(Scope) as UnaryExpression;

            Assert.AreEqual(ExpressionType.Throw, result.NodeType);
            Assert.AreEqual(expectedWrappedExpr, result.Operand);
        }

        [TestMethod]
        public void WrapException_Calls_Into_FryScriptException_Passing_Exception()
        {
            Node.StubParseNode(line: 10, column: 20);
            Node.StubCompilerContext();

            var toThrowExpr = Expression.Constant(new object());

            var currentExExpr = Expression.Parameter(typeof(Exception));
            Scope.SetData(ScopeData.CurrentException, currentExExpr);

            var expectedRethrow = false;
            var result = Node.WrapException(Scope, toThrowExpr, expectedRethrow) as MethodCallExpression;

            Assert.AreEqual(nameof(FryScriptException.Throw), result.Method.Name);
            Assert.AreEqual(typeof(FryScriptException), result.Method.DeclaringType);

            Assert.AreEqual(6, result.Arguments.Count);
            Assert.AreEqual(toThrowExpr, result.Arguments[0]);
            Assert.AreEqual(currentExExpr, result.Arguments[1]);
            Assert.AreEqual(Node.CompilerContext.Name, (result.Arguments[2] as ConstantExpression).Value);
            Assert.AreEqual(10, (result.Arguments[3] as ConstantExpression).Value);
            Assert.AreEqual(20, (result.Arguments[4] as ConstantExpression).Value);
            Assert.AreEqual(expectedRethrow, (result.Arguments[5] as ConstantExpression).Value);
        }

        [TestMethod]
        public void WrapException_Calls_Into_FryScriptException_Passing_Null()
        {
            Node.StubParseNode(line: 10, column: 20);
            Node.StubCompilerContext();

            var toThrowExpr = Expression.Constant(new object());

            var expectedRethrow = false;
            var result = Node.WrapException(Scope, toThrowExpr, expectedRethrow) as MethodCallExpression;

            Assert.AreEqual(nameof(FryScriptException.Throw), result.Method.Name);
            Assert.AreEqual(typeof(FryScriptException), result.Method.DeclaringType);

            Assert.AreEqual(6, result.Arguments.Count);
            Assert.AreEqual(toThrowExpr, result.Arguments[0]);
            Assert.AreEqual(null, (result.Arguments[1] as ConstantExpression).Value);
            Assert.AreEqual(Node.CompilerContext.Name, (result.Arguments[2] as ConstantExpression).Value);
            Assert.AreEqual(10, (result.Arguments[3] as ConstantExpression).Value);
            Assert.AreEqual(20, (result.Arguments[4] as ConstantExpression).Value);
            Assert.AreEqual(expectedRethrow, (result.Arguments[5] as ConstantExpression).Value);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void HandleThrow_Handles_Throwing_Null()
        {
            Node
                .StubCompilerContext()
                .StubParseNode();

            var throwTarget = Node<AstNode>.Empty;
            var nullNode = Node<NullNode>.Empty;
            throwTarget.SetChildren(nullNode);

            Node.SetChildren(null, throwTarget);

            Node.HandleThrow(Scope);
        }

        [TestMethod]
        public void HandleThrow_Gets_Throw_Target_Expressio()
        {
            var throwTarget = Node<AstNode>.Empty;

            var expectedTargetExpr = Expression.Constant(new object());
            throwTarget.GetExpression(expectedTargetExpr, Scope);

            Node.SetChildren(null, throwTarget);

            var result = Node.HandleThrow(Scope);

            Assert.AreEqual(expectedTargetExpr, result);
        }

        [TestMethod]
        public void HandleRethrow_Gets_Scope_Data_Exception()
        {
            var expectedExpr = Expression.Parameter(typeof(Exception));
            Scope.SetData<Expression>(ScopeData.CurrentException, expectedExpr);

            var result = Node.HandleRethrow(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void HandleRethrow_Throws_Compiler_Exception_If_Scope_Data_Is_Missing()
        {
            Node
                .StubCompilerContext()
                .StubParseNode();

            Node.HandleRethrow(Scope);
        }
    }
}
