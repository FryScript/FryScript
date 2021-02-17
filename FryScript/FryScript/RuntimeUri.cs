using System;
using System.Reflection;

namespace FryScript
{
    public static class RuntimeUri
    {
        public static Uri ScriptObjectUri = GetRuntimeUri(typeof(ScriptObject));

        public static Uri ScriptArrayUri = GetRuntimeUri(typeof(ScriptArray));

        public static Uri ScriptErrorUri = GetRuntimeUri(typeof(ScriptError));

        public static Uri ScriptFibreUri = GetRuntimeUri(typeof(ScriptFibre));
        
        public static Uri ScriptFibreContextUri = GetRuntimeUri(typeof(ScriptFibreContext));

        public static Uri ScriptFunctionUri = GetRuntimeUri(typeof(ScriptFunction));

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