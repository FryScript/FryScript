using FryScript.Compilation;
using FryScript.Parsing;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    using Helpers;

    public class IdentifierNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return GetIdentifier(scope);
        }

        public override Expression GetIdentifier(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var identifierName = ValueString;
            if (scope.HasMember(identifierName))
            {
                return scope.GetMemberExpression(identifierName);
            }

            var getExpr = ExpressionHelper.DynamicGetMember(ValueString, scope.GetMemberExpression(Keywords.This));

            return getExpr;
        }

        public override Expression SetIdentifier(Scope scope, Expression value)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var identifierName = ValueString;
            if (scope.HasMember(identifierName))
            {
                return Expression.Assign(scope.GetMemberExpression(identifierName), value);
            }

            var setExpr = ExpressionHelper.DynamicSetMember(ValueString, scope.GetMemberExpression(Keywords.This), value);

            return setExpr;
        }

        public override void CreateIdentifier(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (CompilerContext.IsEvalMode && scope.IsRoot)
                return;

            scope.AddMember(ValueString, this);
        }
    }
}
