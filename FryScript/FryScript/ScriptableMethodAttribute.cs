using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptableMethodAttribute : ScriptableBaseAttribute
    {
        public ScriptableMethodAttribute(string name)
            : base(name)
        {
        }
    }
}
