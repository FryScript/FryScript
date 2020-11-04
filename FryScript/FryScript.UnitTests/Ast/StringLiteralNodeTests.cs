using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
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
    public class StringLiteralNodeTests : AstNodeTestBase<StringLiteralNode>
    {
        private IScriptParser _parser;

        public override void OnTestInitialize()
        {
            _parser = Substitute.For<IScriptParser>();

            Node.StubCompilerContext();
            Node.StubParseNode();

            Node.CompilerContext.ExpressionParser = _parser;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Returns_Constant_String()
        {
            Node.Configure()
                .ValueString.Returns("const");

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.AreEqual("const", result.Value);
        }

        [TestMethod]
        public void GetExpression_With_Interpolated_String()
        {
            Node.Configure()
                .ValueString.Returns("@{interpolated}");

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetFormatExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetFormatExpression_No_Interpolations_Returns_String()
        {
            Node.Configure()
                .ValueString.Returns("@{unclosed interpolation");

            Node.Configure()
                .GetInterpolations("@{unclosed interpolation")
                .Returns(new StringLiteralNode.Interpolation[0]);

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.AreEqual("@{unclosed interpolation", result.Value);
        }

        [TestMethod]
        public void GetFormatExpression_Inserts_Interpolation_Values()
        {
            Node.Configure()
                .ValueString.Returns("@{a}@{b}");

            var interpolations = new[]
            {
                new StringLiteralNode.Interpolation(0, 4, "@{a}"),
                new StringLiteralNode.Interpolation(4, 4, "@{b}"),
            };

            Node.Configure()
                .GetInterpolations("@{a}@{b}")
                .Returns(interpolations);

            var expectedExpr1 = Expression.Constant(new object());
            Node.Configure()
                .GetInterpolatedExpression(interpolations[0], Scope)
                .Returns(expectedExpr1);

            var expectedExpr2 = Expression.Constant(new object());
            Node.Configure()
                .GetInterpolatedExpression(interpolations[1], Scope)
                .Returns(expectedExpr2);

            var result = Node.GetFormatExpression(Scope) as MethodCallExpression;

            Assert.AreEqual(nameof(string.Format), result.Method.Name);
            Assert.AreEqual(typeof(string), result.Method.DeclaringType);

            Assert.AreEqual(3, result.Arguments.Count);
            var strExpr = result.Arguments[0] as ConstantExpression;
            Assert.AreEqual("{0}{1}", strExpr.Value);

            var arg1 = result.Arguments[1] as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, arg1.NodeType);
            Assert.AreEqual(1, arg1.Arguments.Count);

            var binder1 = arg1.Binder as ScriptInvokeMemberBinder;
            Assert.AreEqual("toString", binder1.Name);
            Assert.AreEqual(expectedExpr1, arg1.Arguments[0]);

            var arg2 = result.Arguments[2] as DynamicExpression;
            Assert.AreEqual(ExpressionType.Dynamic, arg2.NodeType);
            Assert.AreEqual(1, arg2.Arguments.Count);

            var binder2 = arg2.Binder as ScriptInvokeMemberBinder;
            Assert.AreEqual("toString", binder2.Name);
            Assert.AreEqual(expectedExpr2, arg2.Arguments[0]);
        }

        [TestMethod]
        public void GetInterpolations_Gets_Interpolations()
        {
            var result = Node.GetInterpolations("@{a}@{b}").ToArray();

            Assert.AreEqual(new StringLiteralNode.Interpolation(0, 4, "@{a}"), result[0]);
            Assert.AreEqual(new StringLiteralNode.Interpolation(4, 4, "@{b}"), result[1]);
        }

        [TestMethod]
        public void GetInterpolations_With_Starting_At_With_No_Brackets()
        {
            var result = Node.GetInterpolations("@unclosed").ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void GetInterpolatedExpression_Empty_Interpolation_Throws_Compiler_Exception()
        {
            var interpolation = new StringLiteralNode.Interpolation(0, 4, "@{}");

            Node.GetInterpolatedExpression(interpolation, Scope);
        }

        [TestMethod]
        public void GetInterpolatedExpression_Parses_Interpolated_Expression()
        {
            var interpolation = new StringLiteralNode.Interpolation(0, 4, "@{a}");

            var expectedNode = Node<AstNode>.Empty;
            _parser.ParseExpression(
                "a",
                Node.CompilerContext.Uri.AbsoluteUri,
                Node.CompilerContext)
                .Returns(expectedNode);

            var expectedExpr = Expression.Constant(new object());
            expectedNode.GetExpression(Scope)
                .Returns(expectedExpr);

            Node.WhenForAnyArgs(n => n.AdjustNode(default, default, default))
                .DoNotCallBase();

            var result = Node.GetInterpolatedExpression(interpolation, Scope);

            Assert.AreEqual(expectedExpr, result);

            Node.Received().AdjustNode(expectedNode, interpolation.Length, interpolation.Start);
        }

        [TestMethod]
        public void GetInterpolatedExpression_Modifies_Node_Location_On_Caught_FryScript_Exception()
        {
            var interpolation = new StringLiteralNode.Interpolation(4, 4, "@{a}");

            var ex = new FryScriptException("error")
            { 
                Line = 0,
                Column = 0
            };

            _parser.WhenForAnyArgs(p => p.ParseExpression(default, default, default))
                .Throw(ex);

            Node.StubParseNode(line: 10, column: 20);

            try
            {
                Node.GetInterpolatedExpression(interpolation, Scope);
            }
            catch(FryScriptException expectedEx)
            {
                var expectedLine = Node.ParseNode.Token.Location.Line;
                var expectedColumn = Node.ParseNode.Token.Location.Column + interpolation.Start + Operators.Format.Length + 1;
                Assert.AreEqual(expectedLine, expectedEx.Line);
                Assert.AreEqual(expectedColumn, expectedEx.Column);
            }
        }

        [TestMethod]
        public void AdjustNode_Modifies_Expression_Position_By_Adding_Padding_For_Format_Chars()
        {
            Node.StubParseNode(line: 10, column: 20);

            var length = 5;
            int startIndex = 5;
            var expectedPosition = Node.ParseNode.Span.Location.Position + startIndex + 3;
            var expectedLength = length - 3;
           
            Node.AdjustNode(Node, length, startIndex);
            
            var span = Node.ParseNode.Span;
            var location = span.Location;
            
            Assert.AreEqual(10, location.Line);
            Assert.AreEqual(20, location.Column);
            Assert.AreEqual(expectedPosition, location.Position);
            Assert.AreEqual(expectedLength, span.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void AdjustNode_Throws_Compiler_Exception_If_Node_Is_Await_Expression()
        {
            Node.StubParseNode();

            var awaitNode = Node<AwaitExpressionNode>.Empty;
            awaitNode.StubParseNode();
            awaitNode.StubCompilerContext();

            Node.AdjustNode(awaitNode, 0, 0);
        }
    }
}
