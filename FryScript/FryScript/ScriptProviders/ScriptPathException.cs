using System;
using System.Collections.Generic;
using System.Text;

namespace FryScript.ScriptProviders
{
    internal class ScriptPathException : FryScriptException
    {
        public ScriptPathException(string message) : base(message)
        {
        }

        public ScriptPathException(string message, Exception innerException, string name, int line, int column) : base(message, innerException, name, line, column)
        {
        }
    }
}
