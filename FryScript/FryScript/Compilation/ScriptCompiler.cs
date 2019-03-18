using FryScript.Ast;
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

        public Func<ScriptObject, object> Compile(string script, string fileName, CompilerContext compilerContext)
        {
            compilerContext = compilerContext ?? throw new ArgumentNullException(nameof(compilerContext));
      
            if (string.IsNullOrWhiteSpace(script))
                throw new ArgumentNullException(nameof(script));

            var rootNode = (ScriptNode)_parser.Parse(script, fileName, compilerContext);

            var func = rootNode.Compile(new Scope());

            return func;
        }

        public Func<IScriptObject, object> Compile2(string source, string name, CompilerContext compilerContext)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            compilerContext = compilerContext ?? throw new ArgumentNullException(nameof(compilerContext));

            var rootNode = _parser.Parse2(source, name, compilerContext);

            var func = rootNode.Compile2(new Scope());

            return func;
        }
    }
}
