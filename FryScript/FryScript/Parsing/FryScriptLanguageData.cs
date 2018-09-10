using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript.Parsing
{
    public static class FryScriptLanguageData
    {
        public static readonly LanguageData LanguageData = new LanguageData(new FryScriptGrammar());
    }
}
