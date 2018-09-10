using System;
using System.Collections.Generic;

namespace FryScript.Compilation
{
    public class CircularDependencyException : Exception
    {
        internal string CurrentKey;
        internal IEnumerable<string> KeyNames;
 
        internal CircularDependencyException(string currentKey, IEnumerable<string> keyNames)
        {
            CurrentKey = currentKey;
            KeyNames = keyNames;
        }
    }
}
