using System;
using System.Collections.Generic;

namespace FryScript
{
    public class ObjectRegistry : IObjectRegistry
    {
        private readonly Dictionary<string, IScriptObject> _scriptObjects = new Dictionary<string, IScriptObject>(StringComparer.OrdinalIgnoreCase);

        public void Import(string name, IScriptObject obj)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            obj = obj ?? throw new ArgumentNullException(nameof(obj));

            if (_scriptObjects.ContainsKey(name)) throw new ArgumentException($"A script object named {name} has already been registered", nameof(name));

            _scriptObjects[name] = obj;
        }

        public void Import(Type type, string name = null, bool autoConstruct = true)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            throw new NotImplementedException();
        }

        public bool TryGetObject(string name, out IScriptObject obj)
        {
            return _scriptObjects.TryGetValue(name, out obj);
        }
    }
}
