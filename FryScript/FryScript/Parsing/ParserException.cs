using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript.Parsing
{
    public class ParserException : FryScriptException
    {
        public int TokenLength { get; }

        public ParserException(string message, string name, int line, int column, int tokenLength) 
            : base(message, null, name, line, column)
        {
            TokenLength = tokenLength;
        }

        public static ParserException SyntaxError(string parserMessage, string name, int line, int column, int tokenLength)
        {
            throw new ParserException(parserMessage, name, line, column, tokenLength);
        }
    }
}
