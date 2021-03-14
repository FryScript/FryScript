using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ScriptablePropertyAttribute : ScriptableBaseAttribute
    {
        public ScriptablePropertyAttribute(string name)
            : base(name)
        {
        }
    }
}
