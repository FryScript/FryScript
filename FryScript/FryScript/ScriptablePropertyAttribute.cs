using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
