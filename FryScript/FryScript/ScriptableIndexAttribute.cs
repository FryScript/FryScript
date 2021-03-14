using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ScriptableIndexAttribute : ScriptableBaseAttribute
    {
        private const string IndexName = "Index";

        public ScriptableIndexAttribute()
            : base(IndexName)
        {
        }
    }
}
