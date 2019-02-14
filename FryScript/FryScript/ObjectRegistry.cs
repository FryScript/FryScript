using System;
using System.Collections.Generic;

namespace FryScript
{
    public class ObjectRegistry : IObjectRegistry
    {
        public ObjectRegistry()
        {

        }

        public IScriptObject Import(string name, IScriptObject obj)
        {
            throw new NotImplementedException();
        }

        public IScriptObject Import(Type type, string name = null, bool autoConstruct = true)
        {
            throw new NotImplementedException();
        }

        public bool TryGetObject(string name, out IScriptObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
