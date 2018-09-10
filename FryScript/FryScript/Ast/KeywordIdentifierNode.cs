using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public abstract class KeywordIdentifierNode : TokenNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return GetIdentifier(scope);
        }

        public override Expression GetIdentifier(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!scope.HasMember(ValueString))
                throw ExceptionHelper.InvalidContext(ValueString, this);

            return scope.GetMemberExpression(ValueString);
        }
    }
}
