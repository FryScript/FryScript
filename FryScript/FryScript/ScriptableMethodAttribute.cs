using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
