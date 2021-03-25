using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class InvokeExpressionTests : AstNodeTestBase<InvokeExpressionNode>
    {
        public override void OnTestInitialize()
        {
            Node.StubCompilerContext();
            Node.StubParseNode();
        }

        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Generates_Invoke()
        {
            var expectedTargetExpr = Expression.Constant(new object());
            var targetNode = Node<AstNode>.Empty;
            targetNode.GetExpression(expectedTargetExpr, Scope);

            var argsNode = Node<AstNode>.Empty;

            var expectedArg1Expr = Expression.Constant(new object());
            var arg1Node = Node<AstNode>.Empty;
            arg1Node.GetExpression(expectedArg1Expr, Scope);

            var expectedArg2Expr = Expression.Constant(new object());
            var arg2Node = Node<AstNode>.Empty;
            arg2Node.GetExpression(expectedArg2Expr, Scope);

            argsNode.SetChildren(arg1Node, arg2Node);

            Node.SetChildren(targetNode, argsNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);

            Assert.AreEqual(3, result.Arguments.Count);
            Assert.AreEqual(expectedTargetExpr, result.Arguments[0]);
            Assert.AreEqual(expectedArg1Expr, result.Arguments[1]);
            Assert.AreEqual(expectedArg2Expr, result.Arguments[2]);

            var binder = result.Binder as ScriptInvokeBinder;
            Assert.AreEqual(2, binder.CallInfo.ArgumentCount);
        }

        [TestMethod]
        public void GetExpression_Wraps_Debug_Stack()
        {
            Node.CompilerContext.ScriptRuntime.DebugHook = o => { };

            var expectedTargetExpr = Expression.Constant(new object());
            var targetNode = Node<AstNode>.Empty;
            targetNode.GetExpression(expectedTargetExpr, Scope);

            var argsNode = Node<AstNode>.Empty;

            Node.SetChildren(targetNode, argsNode);

            var expectedDebugExpr = Expression.Constant("debug", typeof(object));

            Node.Configure()
                .WrapDebugStack(
                    Arg.Is<Scope>(s => s.Parent.Parent == Scope),
                    Arg.Any<Func<Scope, Expression>>())
                .Returns(expectedDebugExpr);

            Expression expectedReturnExpr = null;
            Node.Configure()
                .When(n => n.WrapDebugStack(Arg.Any<Scope>(), Arg.Any<Func<Scope, Expression>>()))
                .Do(c =>
                {
                    var func = c[1] as Func<Scope, Expression>;
                    expectedReturnExpr = func(Scope);
                });

            Node.GetExpression(Scope);

            var result = expectedReturnExpr as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);

            var binder = result.Binder as ScriptInvokeBinder;
            Assert.AreEqual(0, binder.CallInfo.ArgumentCount);
        }
    }
}
