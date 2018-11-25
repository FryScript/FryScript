using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TupleDeclarationNode : AstNode
    {
        public bool AllowOut { get; set; }

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var tupleNames = ChildNodes.Skip(1).First() as TupleNamesNode;
            tupleNames.AllowOut = AllowOut;

            tupleNames.DeclareVariables(scope);

            if (ChildNodes.Length == 2)
                return ExpressionHelper.Null();

            var assignTuple = Transform<AssignTupleExpressionNode>(ChildNodes.Skip(1).ToArray()) as AssignTupleExpressionNode;
            assignTuple.AllowOut = AllowOut;

            var assignTupleExp = assignTuple.GetExpression(scope);

            return assignTupleExp;
        }
    }
}
