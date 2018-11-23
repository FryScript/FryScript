using FryScript.Compilation;
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

        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParameterExpression> GetIdentifiers(Scope scope, List<ParameterExpression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            if (ChildNodes.Length == 2)
            {
                if (ChildNodes.First() is TupleNamesNode tuplesNames)
                    (ChildNodes.First() as TupleNamesNode).GetIdentifiers(scope, exprs);
                else
                    exprs.Add(ChildNodes.First().GetExpression(scope) as ParameterExpression);
            }

            exprs.Add(ChildNodes.Skip(1).First().GetExpression(scope) as ParameterExpression);

            return exprs;
        }

        public IEnumerable<ParameterExpression> DeclareVariables(Scope scope, List<ParameterExpression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            var firstNode = ChildNodes.First();
            if (firstNode is IdentifierNode identifier)
                exprs.Add(identifier.CreateIdentifier(scope));

            if (firstNode is TupleNamesNode tupleNames)
                tupleNames.DeclareVariables(scope, exprs);

            var secondNode = ChildNodes.Skip(1).First() as IdentifierNode;
            exprs.Add(secondNode.CreateIdentifier(scope));

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
