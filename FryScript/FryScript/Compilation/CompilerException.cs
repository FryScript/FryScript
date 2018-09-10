using FryScript.Ast;
using System;

namespace FryScript.Compilation
{
    public class CompilerException : FryScriptException
    {
        private readonly AstNode _astNode;
        public CompilerException(string message, AstNode astNode)
            : base(GetCompilerMessage(message, astNode))
        {
            _astNode = astNode ?? throw new ArgumentNullException(nameof(astNode));
        }

        public CompilerException(string message)
            : base(message)
        {
        }

        private static string GetCompilerMessage(string message, AstNode astNode)
        {
            var parseNode = astNode.ParseNode;
            return string.Format("Line {0}, position {1}: {2}",
                parseNode.Span.Location.Line,
                parseNode.Span.Location.Column,
                message);
        }
    }
}
