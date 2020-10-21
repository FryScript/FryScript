using FryScript.Compilation;
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
        private readonly static AstNode[] _emptyNodes = new AstNode[0];
        public ParseTreeNode ParseNode;

        public AstNode[] ChildNodes = _emptyNodes;

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

            return default;
        }

        protected internal virtual Expression GetChildExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1 && ChildNodes[0] == null)
                return ExpressionHelper.Null();

            if (ChildNodes.Length == 1)
                return ChildNodes.Single().GetExpression(scope);

            var childExprs = ChildNodes.Select(c => c.GetExpression(scope)).Where(c => c != null);

            return Expression.Block(typeof(object), childExprs);
        }

        protected internal virtual AstNode Transform<T>(params AstNode[] childNodes)
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
