using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ImportAliasNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public Expression GetExpression(Scope scope, IScriptObject obj)
        {
            var identifier = ChildNodes.First();

            dynamic dynamicObj = obj;
            var member = dynamicObj[identifier.ValueString];
            var memberExpr = Expression.Constant(member);

            identifier.CreateIdentifier(scope);
            var setIdentifierExpr = identifier.SetIdentifier(scope, memberExpr);

            return setIdentifierExpr;
        }
    }
}
