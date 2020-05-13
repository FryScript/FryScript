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

        public object InternalData {get;}

        public ParserException(string message, string name, int line, int column, int tokenLength, object internalData = null) 
            : base(message, null, name, line, column)
        {
            TokenLength = tokenLength;
            InternalData = internalData;
        }
    }
}
