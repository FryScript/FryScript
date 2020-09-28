using System;
using System.Reflection;

namespace FryScript
{
    public static class RuntimeUri
    {
        public static Uri ScriptObjectUri = new Uri(GetRuntimeUri(typeof(ScriptObject)));

        public static Uri ScriptArrayUri = new Uri(GetRuntimeUri(typeof(ScriptArray)));

        public static Uri ScriptErrorUri = new Uri(GetRuntimeUri(typeof(ScriptError)));

        public static Uri ScriptFibreUri = new Uri(GetRuntimeUri(typeof(ScriptFibre)));
        
        public static Uri ScriptFibreContextUri = new Uri(GetRuntimeUri(typeof(ScriptFibreContext)));

        public static Uri ScriptFunctionUri = new Uri(GetRuntimeUri(typeof(ScriptFunction)));

        private static string GetRuntimeUri(Type type)
        {
            var scriptableType = type.GetTypeInfo().GetCustomAttribute<ScriptableTypeAttribute>();

            return $"runtime://{scriptableType.Name}.fry";
        }
    }
}