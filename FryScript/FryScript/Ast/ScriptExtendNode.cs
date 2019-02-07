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
        private static readonly MethodInfo ScriptObject_Extend = typeof (ScriptObject).GetMethod(nameof(ScriptObject.Extend), BindingFlags.NonPublic | BindingFlags.Instance);

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var name = ChildNodes.Skip(1).First();
            var nameStr = ScriptTypeHelper.NormalizeTypeName(name.ValueString);
            
            try
            {
                var extendScript = CompilerContext.ScriptEngine.Get(nameStr, CompilerContext.Name);

                //CompilerContext.Extend(extendScript);

                var extendScriptExpr = Expression.Constant(extendScript);
                var thisExpr = scope.GetMemberExpression(Keywords.This);
                var inheritExpr = Expression.Call(thisExpr, ScriptObject_Extend, extendScriptExpr);

                return inheritExpr;
            }
            catch (CircularDependencyException ex)
            {
                throw ExceptionHelper.CircularDependency(ex, name);
            }
        }
    }
}
