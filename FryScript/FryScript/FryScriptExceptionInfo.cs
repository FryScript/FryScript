using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript
{
    public class FryScriptExceptionInfo
    {
        private readonly string _name;
        private readonly int _line;
        private readonly int _position;

        public string Name { get { return _name; } }

        public int Line { get { return _line; } }

        public int Position { get { return _position; } }

        public FryScriptExceptionInfo(string name, int line, int position)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
            _line = line;
            _position = position;
        }
    }
}
