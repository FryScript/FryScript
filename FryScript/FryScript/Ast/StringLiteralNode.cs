using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using Irony.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FryScript.Ast
{
    public class StringLiteralNode : AstNode
    {
        private static readonly Regex FormatRegex = new Regex("@{(.+?)}", RegexOptions.Compiled);

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ValueString.Contains(Operators.Format))
                return GetFormatExpression(scope);

            return Expression.Constant(ValueString);
        }

        private Expression GetFormatExpression(Scope scope)
        {
            var matches = FormatRegex.Matches(ValueString).Cast<Match>().Select(m => m).ToArray();

            var subExprs = matches.Select(m => GetMatchExpression(m, scope));

            var formatStr = ValueString;

            for(var i = 0; i < matches.Length; i++)
            {
                var firstIndex = formatStr.IndexOf(matches[i].Value);
                formatStr = formatStr.Remove(firstIndex, matches[i].Length);
                formatStr = formatStr.Insert(firstIndex, string.Format("{{{0}}}", i));
            }

            var formatArgExprs = new Expression[] { Expression.Constant(formatStr) }
                .Concat(subExprs.Select(e => ExpressionHelper.DynamicInvokeMember(e, "toString")))
                .ToArray();

            var formatExpr = Expression.Call(
                typeof(string),
                "Format",
                null,
                formatArgExprs);

            return formatExpr;
        }

        private Expression GetMatchExpression(Match match, Scope scope)
        {
            var subString = match.Value;

            subString = subString.Trim('@', '{', '}');
            var expressionNode = CompilerContext.ExpressionParser.Parse(
                    subString, 
                    CompilerContext.Uri?.AbsoluteUri ?? nameof(StringLiteralNode), 
                    CompilerContext);

            AdjustNode(expressionNode, match.Length, match.Index);

            var subExpr = expressionNode.GetExpression(scope);

            return subExpr;
        }

        private void AdjustNode(AstNode node, int length, int startIndex)
        {
            var parentLocation = ParseNode.Span.Location;
            var location = new SourceLocation(parentLocation.Position + startIndex + 3, parentLocation.Line, parentLocation.Column);

            var parentSpan = ParseNode.Span;
            var span = new SourceSpan(location, length - 3);

            node.ParseNode.Span = span;
        }
    }
}
