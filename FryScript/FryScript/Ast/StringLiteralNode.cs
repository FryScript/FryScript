using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FryScript.Ast
{
    public class StringLiteralNode : AstNode
    {
        public struct Interpolation
        {
            public readonly int
                Start,
                Length;

            public readonly string Value;

            public Interpolation(int start, int length, string value) 
                => (Start, Length, Value) = (start, length, value);
        }

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ValueString.Contains(Operators.Format))
                return GetFormatExpression(scope);

            return Expression.Constant(ValueString);
        }

        protected internal virtual Expression GetFormatExpression(Scope scope)
        {
            var interpolations = GetInterpolations(ValueString).ToArray();

            if (interpolations.Length == 0)
                return Expression.Constant(ValueString);

            var subExprs = interpolations.Select(m => GetInterpolatedExpression(m, scope));

            var formatStr = ValueString;

            for (var i = 0; i < interpolations.Length; i++)
            {
                var firstIndex = formatStr.IndexOf(interpolations[i].Value);
                formatStr = formatStr.Remove(firstIndex, interpolations[i].Length);
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

        protected internal virtual IEnumerable<Interpolation> GetInterpolations(string str)
        {
            var curPos = 0;
            var endPos = str.Length;

            var braces = 0;
            var capturing = false;
            var capturePos = 0;

            while(curPos < endPos)
            {
                var curChar = str[curPos];

                if(capturing && curPos - 1 == capturePos && curChar != '{')
                {
                    capturing = false;
                }

                if(capturing)
                {
                    if (curChar == '{')
                        braces++;

                    if (curChar == '}')
                        braces--;

                    if(braces == 0)
                    {
                        var length = curPos - capturePos + 1;
                        var exprString = str.Substring(capturePos, length);
                        yield return new Interpolation(capturePos, length, exprString);

                        capturing = false;
                    }
                }

                if (curChar == '@' && !capturing)
                {
                    capturePos = curPos;
                    braces = 0;
                    capturing = true;
                }

                curPos++;
            }
        }

        protected internal virtual Expression GetInterpolatedExpression(Interpolation interpolation, Scope scope)
        {
            var subString = interpolation.Value;

            subString = subString.Substring(2, subString.Length - 3);

            if (string.IsNullOrWhiteSpace(subString))
                throw ExceptionHelper.EmptyInterpolation(this, interpolation.Start, interpolation.Length);

            AstNode expressionNode;
            try
            {
                expressionNode = CompilerContext.ExpressionParser.ParseExpression(
                         subString,
                         CompilerContext.Uri?.AbsoluteUri ?? nameof(StringLiteralNode),
                         CompilerContext);

                AdjustNode(expressionNode, interpolation.Length, interpolation.Start);

                var subExpr = expressionNode.GetExpression(scope);

                return subExpr;
            }
            catch (FryScriptException ex)
            {
                var location = ParseNode.Token.Location;
                ex.Line = location.Line;
                ex.Column = location.Column + ex.Column + interpolation.Start + Operators.Format.Length + 1;
                throw ex;
            }
        } 

        protected internal virtual void AdjustNode(AstNode node, int length, int startIndex)
        {
            var parentLocation = ParseNode.Span.Location;
            var location = new SourceLocation(parentLocation.Position + startIndex + 3, parentLocation.Line, parentLocation.Column);

            var span = new SourceSpan(location, length - 3);

            node.ParseNode.Span = span;

            if (node is AwaitExpressionNode)
                throw ExceptionHelper.InvalidContext(Keywords.Await, node);
        }
    }
}
