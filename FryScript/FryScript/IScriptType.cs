using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript
{
    public interface IScriptType
    {
        string GetScriptType();
        bool IsScriptType(string scriptType);
        bool ExtendsScriptType(string scriptType);
        bool HasMember(string name);
        IEnumerable<string> GetMembers();
    }
}
