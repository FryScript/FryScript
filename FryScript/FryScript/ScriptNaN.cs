namespace FryScript
{
    using System.Collections.Generic;
    using Helpers;

    public class ScriptNaN : IScriptType
    {
        private const string NaNName = "[NaN]";

        public static ScriptNaN Value = new ScriptNaN();

        private ScriptNaN()
        {
        }

        public string GetScriptType()
        {
            return NaNName;
        }

        public bool IsScriptType(string scriptType)
        {
            return ScriptTypeHelper.NormalizeTypeName(NaNName) == ScriptTypeHelper.NormalizeTypeName(scriptType);
        }

        public bool ExtendsScriptType(string scriptType)
        {
            return IsScriptType(scriptType);
        }

        public bool HasMember(string name)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetMembers()
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator float(ScriptNaN value)
        {
            return float.NaN;
        }
    }
}
