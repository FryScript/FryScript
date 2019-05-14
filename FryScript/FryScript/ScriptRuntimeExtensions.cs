using System;

namespace FryScript
{
    public static class ScriptRuntimeExtensions
    {
        public static IScriptObject Import<T>(this IScriptRuntime source)
            where T: class
        {
            return (source ?? throw new ArgumentNullException(nameof(source))).Import(typeof(T));
        }
    }
}
