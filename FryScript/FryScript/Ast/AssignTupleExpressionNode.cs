using FryScript.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{
    public class AssignTupleExpressionNode : AstNode
    {
        private static PropertyInfo IndexProperty = typeof(ScriptTuple).GetTypeInfo().DeclaredProperties.Single(p => p.GetIndexParameters().Length == 1);

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
            {
                var transformExpr = ChildNodes.First() as TupleNamesNode;
                return transformExpr.CreateTuple(scope);
            }

            scope = scope.New();
            List<ParameterExpression> identifiers = new List<ParameterExpression>();

            var identifiersExprs = (ChildNodes.First() as TupleNamesNode).GetIdentifiers(scope, identifiers);

            var right = ChildNodes.Skip(2).First();

            var rightExpr = right.GetExpression(scope);
            var tupleExpr = scope.AddTempMember(TempPrefix.Tuple, this, typeof(ScriptTuple));

            var wrapTupleExpr = Expression.Call(typeof(ScriptTuple), nameof(ScriptTuple.WrapTuple), null, rightExpr);

            var assignTupleExpr = Expression.Assign(tupleExpr, wrapTupleExpr);


            var assignIdentifiers = identifiers.Select((e, i) =>
            {
                var indexExpr = Expression.MakeIndex(tupleExpr, IndexProperty, new[] { Expression.Constant(i) });
                var assignIdentifierExpr = Expression.Assign(e, indexExpr);

                return assignIdentifierExpr;
            });

            var converTupleExpr = Expression.Convert(tupleExpr, typeof(object));

            var exprs = new List<Expression>
            {
                assignTupleExpr
            };
            exprs.AddRange(assignIdentifiers);
            exprs.Add(converTupleExpr);

            var blockExpr = scope.ScopeBlock(exprs.ToArray());

            return blockExpr;
        }
    }
}
