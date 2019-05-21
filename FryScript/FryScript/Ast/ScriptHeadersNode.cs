using FryScript.Compilation;
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
                return null;

            ChildNodes = ChildNodes.OrderBy(n => n is ScriptExtendNode ? 0 : 1)
                .ThenBy(n => n is ScriptImportNode ? 0 : 1)
                .ThenBy(n => n is ScriptImportFromNode ? 0 : 1).ToArray();
                //.ThenBy(n => n is ScriptProtoNode ? 0 : 1).ToArray();

            var extendNodes = ChildNodes.Where(c => c.GetType() == typeof(ScriptExtendNode)).ToList();

            if (extendNodes.Count > 1)
                throw CompilerException.FromAst("Headers can only include one extend statement", extendNodes.Last());

            //var protoNodes = ChildNodes.Where(n => n.GetType() == typeof (ScriptProtoNode)).ToList();

            //if (protoNodes.Count > 1)
            //    throw CompilerException.FromAst("Headers can only include one proto statement", protoNodes.Last());

            return GetChildExpression(scope);
        }
    }
}
