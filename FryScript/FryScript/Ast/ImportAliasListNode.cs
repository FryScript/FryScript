using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ImportAliasListNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public virtual Expression GetExpression(Scope scope, IScriptObject obj)
        {
            return Expression.Block(typeof(object),
                ChildNodes.Cast<ImportAliasNode>().Select(c => c.GetExpression(scope, obj)));
        }
    }
}
