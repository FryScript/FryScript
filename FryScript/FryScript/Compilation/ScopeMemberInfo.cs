using FryScript.Ast;
using System.Linq.Expressions;

namespace FryScript.Compilation
{
    public struct ScopeMemberInfo
    {
        public readonly string Name;

        public readonly ParameterExpression Parameter;

        public readonly AstNode AstNode;

        public ScopeMemberInfo(string name, ParameterExpression parameterExpression, AstNode astNode)
            => (Name, Parameter, AstNode) = (name, parameterExpression, astNode);
    }
}
