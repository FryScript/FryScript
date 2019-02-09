namespace FryScript.ScriptProviders
{
    public interface IScriptLoader
    {
        string Load(string path, string relativeTo);
    }
}
