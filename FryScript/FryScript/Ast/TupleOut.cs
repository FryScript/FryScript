using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TupleOut : IdentifierNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public override Expression SetIdentifier(Scope scope, Expression value)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));
            value = value ?? throw new ArgumentNullException(nameof(value));

            if (!scope.TryGetData(ScopeData.TupleOut, out ParameterExpression tupleOut))
                throw new InvalidOperationException("Missing tuple out scope data");

            return Expression.Assign(tupleOut, value);
        }

        public override void CreateIdentifier(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (scope.HasData(ScopeData.TupleOut))
                ExceptionHelper.MultipleOutVars(this);

            var tupleOutExpr = scope.AddTempMember(TempPrefix.TupleOut, this);
            scope.SetData(ScopeData.TupleOut, tupleOutExpr);
        }
    }
}
