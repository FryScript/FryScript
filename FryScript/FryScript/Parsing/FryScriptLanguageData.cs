using Irony.Parsing;

namespace FryScript.Parsing
{
    public static class FryScriptLanguageData
    {
        public static readonly LanguageData LanguageData = new LanguageData(new FryScriptGrammar());
    }
}
