using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
