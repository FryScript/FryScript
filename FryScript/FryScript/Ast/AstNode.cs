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
        public class AstNodeTransformer
        {
            public virtual AstNode Transform<T>(ParseTreeNode parseNode, CompilerContext compilerContext, params AstNode[] childNodes)
            where T : AstNode, new()
            {
                return new T
                {
                    ParseNode = parseNode,
                    CompilerContext = compilerContext,
                    ChildNodes = childNodes
                };
            }
        }

        public class GetChildExpressionVisitor
        {
            public virtual Expression GetExpression(AstNode node, Scope scope)
            {
                scope = scope ?? throw new ArgumentNullException(nameof(scope));

                if (node.ChildNodes.Length == 1 && node.ChildNodes[0] == null)
                    return ExpressionHelper.Null();

                if (node.ChildNodes.Length == 1)
                    return node.ChildNodes.Single().GetExpression(scope);

                var childExprs = node.ChildNodes.Select(c => c.GetExpression(scope)).Where(c => c != null);

                return Expression.Block(typeof(object), childExprs);
            }
        }

        private static AstNodeTransformer _astNodeTransformer = new AstNodeTransformer();
        private static GetChildExpressionVisitor _getChildExpressionVisitor = new GetChildExpressionVisitor();
        private readonly static AstNode[] _emptyNodes = new AstNode[0];
        public ParseTreeNode ParseNode;

        public AstNode[] ChildNodes = _emptyNodes;

        public CompilerContext CompilerContext;

        public virtual string ValueString { get { return ParseNode.Token.ValueString; } }

        public virtual object Value { get { return ParseNode.Token.Value; } }

        public AstNodeTransformer NodeTransformer { get; set; } = _astNodeTransformer;

        public GetChildExpressionVisitor ChildExpressionVisitor {get;set;} = _getChildExpressionVisitor;

        public abstract Expression GetExpression(Scope scope);

        public virtual Expression GetIdentifier(Scope scope)
        {
            throw new NotImplementedException();
        }

        public virtual Expression SetIdentifier(Scope scope, Expression value)
        {
            throw new NotImplementedException();
        }

        public virtual void CreateIdentifier(Scope scope)
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

        public T FindChild<T>()
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

        public Expression GetChildExpression(Scope scope)
        {
            return ChildExpressionVisitor.GetExpression(this, scope);
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
            return NodeTransformer.Transform<T>(ParseNode, CompilerContext, childNodes);
        }
    }
}
