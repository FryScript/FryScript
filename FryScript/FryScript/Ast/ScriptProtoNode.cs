//using FryScript.Compilation;
//using FryScript.Parsing;
//using System;
//using System.Linq;
//using System.Linq.Expressions;

//namespace FryScript.Ast
//{
//    public class ScriptProtoNode : AstNode
//    {
//        public override Expression GetExpression(Scope scope)
//        {
//            scope = scope ?? throw new ArgumentNullException(nameof(scope));

//            var oldScope = scope;

//            // We want an empty scope for the proto definition
//            scope = new Scope();

//            var statementsNode = ChildNodes.Skip(1).First();

//            var paramExpr = scope.AddKeywordMember<ScriptObject>(Keywords.This, this);
//            scope = scope.New();

//            // We want to import any declared import scripts
//            var importExprs = (from i in CompilerContext.ImportInfos
//                let param = scope.AddMember(i.Alias, this)
//                select new
//                {
//                    Param = param,
//                    Assign = Expression.Assign(param, Expression.Constant(i.Object))
//                }).ToArray();

//            var bodyExpr = scope.ScopeBlock(importExprs.Select(i => i.Assign)
//                .Concat(new[] {statementsNode.GetExpression(scope)}).ToArray());

//            var lambda = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, paramExpr);

//            CompilerContext.ProtoCtor = lambda.Compile();
//            CompilerContext.ProtoReference = new ScriptObjectReference();

//            var protoReferenceExpr = Expression.Constant(CompilerContext.ProtoReference);
//            var protoExpr = oldScope.AddKeywordMember<ScriptObjectReference>(Keywords.Proto, this);
//            var assignProtoExpr = Expression.Assign(protoExpr, protoReferenceExpr);

//            return assignProtoExpr;
//        }
//    }
//}
