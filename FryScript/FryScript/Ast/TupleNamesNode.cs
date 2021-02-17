using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{
    public class TupleNamesNode : AstNode
    {
        private static ConstructorInfo ScriptTuple_ObjectArrayCtor =
            typeof(ScriptTuple).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new[] { typeof(object[]) },
                null);

        public bool AllowOut { get; set; }

        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<AstNode> GetIdentifiers(Scope scope, List<AstNode> nodes = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            nodes = nodes ?? new List<AstNode>();

            HandleFirstNode(scope, nodes);

            HandleSecondNode(nodes);

            return nodes;
        }

        protected internal virtual void HandleFirstNode(Scope scope, List<AstNode> nodes)
        {
            var firstNode = ChildNodes.First();
            if (firstNode is TupleNamesNode tuplesNames)
                tuplesNames.GetIdentifiers(scope, nodes);
            else if (firstNode.FindChild<IdentifierExpressionNode>() is IdentifierExpressionNode firstIdentifier)
                nodes.Add(firstIdentifier);
            else if (firstNode is TupleOut tupleOut)
                nodes.Add(tupleOut);
            else
                ExceptionHelper.InvalidContext(firstNode.ParseNode.Term.Name, firstNode);
        }

        protected internal virtual void HandleSecondNode(List<AstNode> nodes)
        {
            var secondNode = ChildNodes.Skip(1).First();
            if (secondNode.FindChild<IdentifierExpressionNode>() is IdentifierExpressionNode identifierExpressionNode)
                nodes.Add(identifierExpressionNode);
            else if (secondNode is TupleOut tupleOut)
                nodes.Add(tupleOut);
            else
                ExceptionHelper.InvalidContext(secondNode.ParseNode.Term.Name, secondNode);
        }

        public virtual void DeclareVariables(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            DeclareVariables(scope, new List<IdentifierNode>())
                .ForEach(i => i.CreateIdentifier(scope));
        }

        protected internal virtual List<IdentifierNode> DeclareVariables(Scope scope, List<IdentifierNode> nodes)
        {
            DeclareFirstNode(scope, nodes);
            
            DeclareSecondNode(nodes);

            return nodes;
        }

        protected internal virtual void DeclareFirstNode(Scope scope, List<IdentifierNode> nodes)
        {
            var firstNode = ChildNodes.First();
            if (firstNode is TupleNamesNode tuplesNames)
                tuplesNames.DeclareVariables(scope, nodes);
            else if (firstNode.FindChild<IdentifierNode>() is IdentifierNode firstIdentifier)
                nodes.Add(firstIdentifier);
            else if (firstNode is TupleOut tupleOut)
                nodes.Add(tupleOut);
            else
                throw ExceptionHelper.InvalidContext(firstNode.ParseNode.Term.Name, firstNode);
        }

        protected internal virtual void DeclareSecondNode(List<IdentifierNode> nodes)
        {
            var secondNode = ChildNodes.Skip(1).First();
            if (secondNode.FindChild<IdentifierNode>() is IdentifierNode identifierNode)
                nodes.Add(identifierNode);
            else if (secondNode is TupleOut tupleOut)
                nodes.Add(tupleOut);
            else
                throw ExceptionHelper.InvalidContext(secondNode.ParseNode.Term.Name, secondNode);
        }

        public virtual Expression CreateTuple(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var tupleItemExprs = GetTupleArgs(scope);

            var tupleItemsArrayExpr = Expression.NewArrayInit(typeof(object), tupleItemExprs);
            var newTupleExpr = Expression.New(ScriptTuple_ObjectArrayCtor, tupleItemsArrayExpr);
            var convertTupleExpr = Expression.Convert(newTupleExpr, typeof(object));

            return convertTupleExpr;
        }

        protected internal virtual List<Expression> GetTupleArgs(Scope scope, List<Expression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<Expression>();

            var firstNode = ChildNodes.First();
            var secondNode = ChildNodes.Skip(1).First();

            if (!AllowOut && firstNode is TupleOut)
                ExceptionHelper.UnexpectedOut(firstNode);

            if (!AllowOut && secondNode is TupleOut)
                ExceptionHelper.UnexpectedOut(secondNode);

            if (firstNode is TupleNamesNode tupleNames)
                tupleNames.GetTupleArgs(scope, exprs);
            else
                exprs.Add(firstNode.GetExpression(scope));

            exprs.Add(secondNode.GetExpression(scope));

            return exprs;
        }
    }
}
