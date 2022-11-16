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

        public static T New<T>(this IScriptRuntime source, string name, params object[] args)
            where T : class
        {
            return (T)(source ?? throw new ArgumentNullException(nameof(source))).New(name, args);
        }

        public static IScriptObject ImportEnum<TEnum>(this IScriptRuntime source, string name)
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException($"Type {typeof(TEnum).FullName} must be an enum");

            source = source ?? throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var enumObj = new ScriptObject();

            foreach(var value in Enum.GetValues(typeof(TEnum)))
            {
                var memberName = Enum.GetName(typeof(TEnum), value);

                enumObj[memberName] = value;
            }

            source.Import(name, enumObj);

            return enumObj;
        }
    }
}
