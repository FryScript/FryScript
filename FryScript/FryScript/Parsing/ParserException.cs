using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript.Parsing
{
    public class ParserException : FryScriptException
    {
        public ParserException(string message, string name, int line, int column) 
            : base(message, null, name, line, column)
        {
        }

        public static ParserException SyntaxError(string parserMessage, string name, int line, int column)
        {
            throw new ParserException(parserMessage, name, line, column);
        }
    }
}
