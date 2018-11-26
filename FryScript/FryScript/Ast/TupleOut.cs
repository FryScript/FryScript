using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FryScript.Compilation;
using FryScript.Helpers;

namespace FryScript.Ast
{
    public class TupleOut : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return CreateOut(scope);
        }


        public override Expression SetIdentifier(Scope scope, Expression value)
        {
            scope.TryGetData(ScopeData.TupleOut, out ParameterExpression tupleOut);
            return Expression.Assign(tupleOut, value);
        }

        public ParameterExpression CreateOut(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (scope.HasData(ScopeData.TupleOut))
                ExceptionHelper.MultipleOutVars(this);

            var tupleOutExpr = scope.AddTempMember(TempPrefix.TupleOut, this);
            scope.SetData(ScopeData.TupleOut, tupleOutExpr);

            return tupleOutExpr;
        }
    }
}
