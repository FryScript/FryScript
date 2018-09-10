using System.Linq.Expressions;

namespace FryScript
{
    public enum ScriptableBinaryOperater
    {
        Equal = ExpressionType.Equal,
        NotEqual = ExpressionType.NotEqual,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThanOrEqual,

        Add = ExpressionType.Add,
        Subtract = ExpressionType.Subtract,
        Multiply = ExpressionType.Multiply,
        Divide = ExpressionType.Divide,
        Modulo = ExpressionType.Modulo
    }
}
