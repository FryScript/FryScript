using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptHeadersNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 0)
                return ExpressionHelper.Null();

            ChildNodes = ChildNodes.OrderBy(n => n is ScriptExtendNode ? 0 : 1)
                .ThenBy(n => n is ScriptImportNode ? 0 : 1)
                .ThenBy(n => n is ScriptImportFromNode ? 0 : 1).ToArray();

            var extendNodes = ChildNodes.Where(c => c is ScriptExtendNode).ToList();

            if (extendNodes.Count > 1)
                throw CompilerException.FromAst("Headers can only include one extend statement", extendNodes.Last());

            return GetChildExpression(scope);
        }
    }
}
