using System.Reflection;

namespace FryScript.HostInterop
{
    public class ScriptableMethodInfo
    {
        public MethodInfo Method { get; internal set; }
        public string Name { get; internal set; }
    }
}
