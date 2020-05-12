using FryScript.Ast;
using FryScript.Compilation;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Linq;

namespace FryScript.Parsing
{
    public class ScriptParser : IScriptParser
    {
        private readonly Parser _parser;

        public ScriptParser(NonTerminal root = null)
        {
            if (root == null)
                _parser = new Parser(FryScriptLanguageData.LanguageData);
            else
                _parser = new Parser(FryScriptLanguageData.LanguageData, root);
        }

        public IRootNode Parse(string source, string name, CompilerContext compilerContext)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("fileName");

            if (string.IsNullOrWhiteSpace("script"))
                throw new ArgumentNullException("script");

            if (compilerContext == null)
                throw new ArgumentNullException("conpilerContext");

            var parseTree = _parser.Parse(source, name);

            if (parseTree.HasErrors())
            {
                var parseError = parseTree.ParserMessages.First();
                var token = parseTree.Tokens.Last();
                var errorMessage = $"Unexpected {token.ValueString}";

                throw ParserException.SyntaxError(errorMessage, compilerContext.Name, parseError.Location.Line, parseError.Location.Column, token.Length);
            }

            var astBuilder = new AstBuilder(compilerContext);
            astBuilder.BuildAst(parseTree);

            return parseTree.Root.AstNode as IRootNode;
        }

        public AstNode ParseExpression(string source, string name, CompilerContext compilerContext)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("fileName");

            if (string.IsNullOrWhiteSpace("script"))
                throw new ArgumentNullException("script");

            if (compilerContext == null)
                throw new ArgumentNullException("conpilerContext");

            var parseTree = _parser.Parse(source, name);

            if (parseTree.HasErrors())
            {
                var parseError = parseTree.ParserMessages.First();
                throw ParserException.SyntaxError(parseError.Message, compilerContext.Name, parseError.Location.Line, parseError.Location.Column, 0);
            }

            var astBuilder = new AstBuilder(compilerContext);
            astBuilder.BuildAst(parseTree);

            return parseTree.Root.AstNode as AstNode;
        }
    }
}
