using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{
    public class ScriptExtendNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (CompilerContext.IsEvalMode)
                ExceptionHelper.ExtendUnavailable(this);

            var name = ChildNodes.Skip(1).First();

            var nameStr = name.ValueString;

            var extendScript = CompilerContext.ScriptRuntime.Get(nameStr, CompilerContext.Uri);

            var builder = extendScript.ObjectCore.Builder;
            var builderExpr = Expression.Constant(builder);
            var thisExpr = scope.GetMemberExpression(Keywords.This);
            var extendExpr = Expression.Call(builderExpr, nameof(builder.Extend), null, thisExpr);

            CompilerContext.ScriptType = extendScript.GetType();
            CompilerContext.ScriptObjectBuilder = builder;

            return extendExpr;
        }
    }
}
