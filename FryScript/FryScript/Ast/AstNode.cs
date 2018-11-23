using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public abstract class AstNode : IAstNodeInit
    {
        private class PlaceHolder : AstNode
        {
            public override Expression GetExpression(Scope scope)
            {
                throw new NotImplementedException();
            }
        }

        private class ExpressionWrapper : AstNode
        {
            private readonly Expression _expression;

            public ExpressionWrapper(Expression expression)
            {
                _expression = expression;
            }

            public override Expression GetExpression(Scope scope)
            {
                return _expression;
            }
        }

        public ParseTreeNode ParseNode;

        public AstNode[] ChildNodes;

        public CompilerContext CompilerContext;

        public virtual string ValueString { get { return ParseNode.Token.ValueString; } }

        public virtual object Value { get { return ParseNode.Token.Value; } }

        public abstract Expression GetExpression(Scope scope);

        public virtual Expression GetIdentifier(Scope scope)
        {
            throw new NotImplementedException();
        }

        public virtual Expression SetIdentifier(Scope scope, Expression value)
        {
            throw new NotImplementedException();
        }

        public virtual ParameterExpression CreateIdentifier(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Init(AstContext context, ParseTreeNode parseNode)
        {
            CompilerContext = context as CompilerContext ?? throw new ArgumentNullException(nameof(context));
            ParseNode = parseNode ?? throw new ArgumentNullException(nameof(parseNode));

            ChildNodes = (from c in ParseNode.ChildNodes
                          select c.AstNode as AstNode).ToArray();
        }

        public bool TryGetChild<T>(out T child)
            where T : AstNode
        {
            child = null;
            var current = this;

            while (current != null)
            {
                if (current is T match && match.ChildNodes.Length != 1)
                {
                    child = match;
                    return true;
                }

                if (current.ChildNodes.Length == 1)
                    current = current.ChildNodes.FirstOrDefault();
                else
                    return false;
            }

            return false;
        }

        public T AsType<T>()
            where T : AstNode
        {
            AstNode curNode = this;

            while (curNode != null)
            {
                curNode = curNode.ChildNodes.FirstOrDefault();

                if (!(curNode is T))
                    continue;

                return (T)curNode;
            }

            return default(T);
        }

        protected Expression GetChildExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1 && ChildNodes[0] == null)
                return ExpressionHelper.Null();

            if (ChildNodes.Length == 1)
                return ChildNodes.Single().GetExpression(scope);

            var childExprs = ChildNodes.Select(c => c.GetExpression(scope)).Where(c => c != null);

            return Expression.Block(typeof(object), childExprs);
        }

        protected Expression WrapDebugExpression(DebugEvent debugEvent, Scope scope, Func<Scope, Expression> func)
        {
            var span = ParseNode.Span;
            var location = span.Location;

            return DebugExpressionHelper.GetDebugEventExpression(
                debugEvent,
                scope,
                func,
                CompilerContext.Name,
                location.Line,
                location.Position,
                span.Length,
                CompilerContext.DebugHook);
        }

        protected Expression WrapDebugStack(Scope scope, Func<Scope, Expression> func, DebugEvent pushEvent = DebugEvent.PushStackFrame, DebugEvent popEvent = DebugEvent.PopStackFrame)
        {
            var span = ParseNode.Span;
            var location = span.Location;

            return DebugExpressionHelper.GetCallStackExpression(
                scope,
                func,
                CompilerContext.Name,
                location.Line,
                location.Column,
                span.Length,
                CompilerContext.DebugHook,
                pushEvent,
                popEvent);
        }

        protected AstNode Transform<T>(params AstNode[] childNodes)
            where T : AstNode, new()
        {
            return new T
            {
                ParseNode = ParseNode,
                CompilerContext = CompilerContext,
                ChildNodes = childNodes
            };
        }
    }
}
