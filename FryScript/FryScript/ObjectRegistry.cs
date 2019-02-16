using FryScript.HostInterop;
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

        public void Import(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            var name = TypeProvider.Current.GetTypeName(type);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Type {type.FullName} does not define a scriptable name", nameof(type));

            if (_scriptObjects.ContainsKey(name))
                throw new ArgumentException($"An object with the name \"{name}\" has already been registered");

            var proxyType = TypeProvider.Current.GetProxy(type);

            var obj = Activator.CreateInstance(proxyType) as IScriptObject;

            _scriptObjects[name] = obj;
        }

        public bool TryGetObject(string name, out IScriptObject obj)
        {
            return _scriptObjects.TryGetValue(name, out obj);
        }
    }
}
