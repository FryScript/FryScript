﻿namespace FryScript.Parsing
{
    public class ParserException : FryScriptException
    {
        public ParserException(string message, string name, int line, int column, int tokenLength, object internalData = null) 
            : base(message, null, name, line, column)
        {
            TokenLength = tokenLength;
            InternalData = internalData;
        }
    }
}
