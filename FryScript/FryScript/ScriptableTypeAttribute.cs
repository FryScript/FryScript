using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptableTypeAttribute : ScriptableBaseAttribute
    {
        public bool IgnoreTypeFactory { get; set; }

        public ScriptableTypeAttribute(string name)
            : base(name)
        {
        }
    }
}
