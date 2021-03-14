using FryScript.Ast;

namespace FryScript.Compilation
{
    public class CompilerException : FryScriptException
    {
        public CompilerException(string message, string name, int line, int column)
            : base(message, null, name, line, column)
        {

        }

        public static CompilerException FromAst(string message, AstNode node)
        {
            if (node == null)
                return new CompilerException(message, string.Empty, 0, 0);

            return new CompilerException(message, node.CompilerContext.Name, node.ParseNode.Span.Location.Line, node.ParseNode.Span.Location.Column)
            {
                TokenLength = node?.ParseNode?.Span.Length
            };
        }
    }
}
