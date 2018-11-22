using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TupleDeclarationNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var tupleNames = ChildNodes.Skip(1).First() as TupleNamesNode;

            tupleNames.DeclareVariables(scope);

            if (ChildNodes.Length == 2)
                return ExpressionHelper.Null();

            var assignTuple = Transform<AssignTupleExpressionNode>(ChildNodes.Skip(1).ToArray());

            var assignTupleExp = assignTuple.GetExpression(scope);

            return assignTupleExp;
        }
    }
}
