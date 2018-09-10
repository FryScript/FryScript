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

        public ScriptCompiler(IScriptParser parser = null, IScriptParser expressionParser = null)
        {
            _parser = parser ?? new ScriptParser();
            _expressionParser = _expressionParser ?? new ScriptParser(FryScriptLanguageData.LanguageData.Grammar.SnippetRoots.Single(n => n.Name == NodeNames.Expression));
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
    }
}
