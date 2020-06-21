using FryScript.Parsing;
using System;
using System.Linq;

namespace FryScript.Compilation
{
    public class ScriptCompiler : IScriptCompiler
    {
        private readonly IScriptParser _parser;
        private readonly IScriptParser _expressionParser;

        public IScriptParser Parser => _parser;

        public IScriptParser ExpressionParser => _expressionParser;

        public ScriptCompiler()
            : this(new ScriptParser(),
                  new ScriptParser(FryScriptLanguageData.LanguageData.Grammar.SnippetRoots.Single(n => n.Name == NodeNames.Expression))
                  )
        {
        }

        public ScriptCompiler(IScriptParser parser, IScriptParser expressionParser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _expressionParser = expressionParser ?? throw new ArgumentNullException(nameof(expressionParser));
        }

        public Func<IScriptObject, object> Compile(string source, string name, CompilerContext compilerContext)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            compilerContext = compilerContext ?? throw new ArgumentNullException(nameof(compilerContext));
            compilerContext.ExpressionParser = _expressionParser;

            var rootNode = _parser.Parse(source, name, compilerContext);

            var func = rootNode.Compile(compilerContext.Scope);

            return func;
        }
    }
}
