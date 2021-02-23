using System;
using System.Reflection;

namespace FryScript
{
    public static class RuntimeUri
    {
        private readonly static Uri _scriptObjectUri = GetRuntimeUri(typeof(ScriptObject));

        private readonly static Uri _scriptArrayUri = GetRuntimeUri(typeof(ScriptArray));

        private readonly static Uri _scriptErrorUri = GetRuntimeUri(typeof(ScriptError));

        private readonly static Uri _scriptFibreUri = GetRuntimeUri(typeof(ScriptFibre));
        
        private readonly  static Uri _scriptFibreContextUri = GetRuntimeUri(typeof(ScriptFibreContext));

        private readonly static Uri _scriptFunctionUri = GetRuntimeUri(typeof(ScriptFunction));

        public static Uri ScriptObjectUri => _scriptObjectUri;

        public static Uri ScriptArrayUri => _scriptArrayUri;

        public static Uri ScriptErrorUri => _scriptErrorUri;

        public static Uri ScriptFibreUri => _scriptFibreUri;

        public static Uri ScriptFibreContextUri => _scriptFibreContextUri;

        public static Uri ScriptFunctionUri => _scriptFunctionUri;

        public static Uri GetRuntimeUri(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            var name = type.GetCustomAttribute<ScriptableTypeAttribute>()?.Name
                ?? throw new ArgumentException("Type musr ");

            return GetRuntimeUri(name);
        }

        public static Uri GetRuntimeUri(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
                
            return new Uri($"runtime://{name}.fry");
        }
    }
}