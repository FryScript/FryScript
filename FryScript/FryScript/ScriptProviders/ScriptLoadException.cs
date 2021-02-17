namespace FryScript.ScriptProviders
{
    public class ScriptLoadException : FryScriptException
    {
        public ScriptLoadException(string name, string relativeTo = null)
            : base(FormatMessage(name, relativeTo))
        {
        }

        private static string FormatMessage(string name, string relativeTo = null)
        {
            return relativeTo == null
                ? $"Unable to load script \"{name}\""
                : $"Unable to load script \"{name}\" relative to \"{relativeTo}\"";
        }
    }
}
