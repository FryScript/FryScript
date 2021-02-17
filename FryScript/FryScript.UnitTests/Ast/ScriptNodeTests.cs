using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class ScriptNodeTests : AstNodeTestBase<ScriptNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Generates_Lambda_Expression()
        {
            Node.StubCompilerContext();

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope) as LambdaExpression;

            Assert.AreEqual(typeof(Func<IScriptObject, object>), result.Type);

            var expectedParamExpr = Scope.GetMemberExpression(Keywords.This);
            Assert.AreEqual(1, result.Parameters.Count);
            Assert.AreEqual(expectedParamExpr, result.Parameters.Single());

            var bodyExpr = result.Body as BlockExpression;
            Assert.AreEqual(1, bodyExpr.Expressions.Count);
            Assert.AreEqual(expectedExpr, bodyExpr.Expressions.Single());
        }

        [TestMethod]
        public void GetExpression_Wraps_Debug_Stack()
        {
            Node.StubCompilerContext(debugHook: h => { });
            Node.StubParseNode();

            var expectedExpr = Expression.Constant("wrappedExpr", typeof(object));
            Node.Configure()
                .GetChildExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedExpr);

            Func<Scope, Expression> expectedFunc = null;
            var debugExpr = Expression.Constant("debugExpr", typeof(object));
            Node.Configure()
                .WrapDebugStack(
                Arg.Is<Scope>(s => s.Parent == Scope),
                Arg.Do<Func<Scope, Expression>>(a => expectedFunc = a),
                DebugEvent.ScriptInitializing,
                DebugEvent.ScriptInitialized)
                .Returns(debugExpr);

            var result = Node.GetExpression(Scope) as LambdaExpression;

            Assert.AreEqual(typeof(Func<IScriptObject, object>), result.Type);

            var expectedParamExpr = Scope.GetMemberExpression(Keywords.This);
            Assert.AreEqual(1, result.Parameters.Count);
            Assert.AreEqual(expectedParamExpr, result.Parameters.Single());

            var bodyExpr = result.Body as ConstantExpression;
            Assert.AreEqual(debugExpr, bodyExpr);

            var wrappedExpr = expectedFunc(Scope) as BlockExpression;
            Assert.AreEqual(1, wrappedExpr.Expressions.Count);
            Assert.AreEqual(expectedExpr, wrappedExpr.Expressions.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Null_Scope()
        {
            Node.Compile(null);
        }

        [TestMethod]
        public void Compile_Generates_Func()
        {
            var expectedResult = new object();
            var expectedExpr = Expression.Lambda<Func<IScriptObject, object>>(
                Expression.Constant(expectedResult),
                Expression.Parameter(typeof(IScriptObject)));

            Node.Configure()
                .GetExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.Compile(Scope) as Func<IScriptObject, object>;
            var funcResult = result(null);

            Assert.AreEqual(expectedResult, funcResult);
        }
    }
}
