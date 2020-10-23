using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FryScript.Ast;
using FryScript.Compilation;
using FryScript.Debugging;
using Irony.Parsing;
using NSubstitute;

namespace FryScript.UnitTests.Ast
{
    public static class Node
    {
        public static T SetChildren<T>(this T node, params AstNode[] children) where T : AstNode
        {
            node.ChildNodes = children;

            return node;
        }

        public static T WithDummyChildren<T>(this T node, int count) where T : AstNode
        {
            var children = new List<AstNode>();
            
            for(var i = 0; i < count; i++)
            {
                children.Add(Node<AstNode>.WithValue(new object()));
            }

            node.ChildNodes = children.ToArray();

            return node;
        }

        public static T WithValue<T>(this T node, object value) where T : AstNode
        {
            node.GetExpression(Arg.Any<Scope>()).Returns(Expression.Constant(value));

            return node;
        }

        public static T GetExpression<T>(this T node, Expression exprToReturn) where T : AstNode
        {
            node.GetExpression(Arg.Any<Scope>()).Returns(exprToReturn);

            return node;
        }

        public static T GetExpression<T>(this T node, Expression exprToReturn, Scope scope) where T : AstNode
        {
            node.GetExpression(scope).Returns(exprToReturn);

            return node;
        }

        public static AstNode StubParseNode(this AstNode node, string valueString = "", object value = null, int position = 0, int line = 0, int column = 0)
        {
            node.ParseNode = new ParseTreeNode(
                new Token(
                    new Terminal("test"),
                    new SourceLocation(position, line, column),
                    valueString,
                    value
                    ));

            return node;
        }

        public static AstNode StubCompilerContext(this AstNode node, DebugHook debugHook = null, bool detailedExceptions = false, bool isEvalMode = false)
        {
            var runtime = Substitute.For<IScriptRuntime>();
            runtime.DebugHook = debugHook;
            runtime.DetailedExceptions = detailedExceptions;

            node.CompilerContext = new CompilerContext(
                runtime,
                new Uri("test://test"),
                isEvalMode);
            
            return node;
        }
    }

    public static class Node<T> where T : AstNode
    {
        public static T Empty => Node<T>.WithValue(new object());
        
        public static T WithValue(object value)
        {
            var node = Substitute.For<T>();
            node.GetExpression(Arg.Any<Scope>()).Returns(Expression.Constant(value));

            return node;
        }

        public static T WithValueString(string value)
        {
            var node = Substitute.For<T>();
            node.ValueString.Returns(value);

            return node;
        }

        public static T WithChildren(params AstNode[] children)
        {
            var node = Substitute.For<T>();
            node.SetChildren(children);

            return node;
        }
    }
}