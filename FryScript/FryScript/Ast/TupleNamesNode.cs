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

        public IEnumerable<AstNode> GetIdentifiers(Scope scope, List<AstNode> nodes = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            nodes = nodes ?? new List<AstNode>();

            if (ChildNodes.Length == 2)
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

            var secondNode = ChildNodes.Skip(1).First();
            if(secondNode.FindChild<IdentifierExpressionNode>() is IdentifierExpressionNode secondIdentifier)
                nodes.Add(secondNode.FindChild<IdentifierExpressionNode>());
            else if (secondNode is TupleOut tupleOut)
                nodes.Add(tupleOut);
            else
                ExceptionHelper.InvalidContext(secondNode.ParseNode.Term.Name, secondNode);

            return nodes;
        }

        public IEnumerable<ParameterExpression> DeclareVariables(Scope scope, List<ParameterExpression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            if (ChildNodes.Length == 2)
            {
                var firstNode = ChildNodes.First();
                if (firstNode is TupleNamesNode tuplesNames)
                    tuplesNames.DeclareVariables(scope);
                else if (firstNode.FindChild<IdentifierNode>() is IdentifierNode firstIdentifier)
                    exprs.Add(firstIdentifier.CreateIdentifier(scope));
                else if (firstNode is TupleOut tupleOut)
                    exprs.Add(tupleOut.CreateOut(scope));
            }

            var secondNode = ChildNodes.Skip(1).First();
            if (secondNode.FindChild<IdentifierNode>() is IdentifierNode secondIdentifier)
                exprs.Add(secondIdentifier.CreateIdentifier(scope));
            else if (secondNode is TupleOut tupleOut)
                exprs.Add(tupleOut.CreateOut(scope));

            return exprs; 
        }

        public Expression CreateTuple(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var tupleItemExprs = GetTupleArgs(scope);

            var tupleItemsArrayExpr = Expression.NewArrayInit(typeof(object), tupleItemExprs);
            var newTupleExpr = Expression.New(ScriptTuple_ObjectArrayCtor, tupleItemsArrayExpr);
            var convertTupleExpr = Expression.Convert(newTupleExpr, typeof(object));

            return convertTupleExpr;
        }

        public List<Expression> GetTupleArgs(Scope scope, List<Expression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<Expression>();

            var firstNode = ChildNodes.First();
            if (firstNode is TupleNamesNode tupleNames)
                tupleNames.GetTupleArgs(scope, exprs);
            else if (firstNode is ExpressionNode expr)
                exprs.Add(expr.GetExpression(scope));


            var secondNode = ChildNodes.Skip(1).First();
            exprs.Add(secondNode.GetExpression(scope));

            return exprs;
        }
    }
}
