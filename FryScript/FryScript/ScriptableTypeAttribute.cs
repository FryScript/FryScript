using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptableTypeAttribute : ScriptableBaseAttribute
    {
        public ScriptableTypeAttribute(string name)
            : base(name)
        {
        }
    }
}
