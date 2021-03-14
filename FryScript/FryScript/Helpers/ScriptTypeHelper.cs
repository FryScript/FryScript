using FryScript.HostInterop;
using System;
using System.Reflection;

namespace FryScript.Helpers
{
    public static class ScriptTypeHelper
    {
        public static string GetScriptType(object target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            if(!TypeProvider.Current.TryGetTypeOperator(target.GetType(), ScriptableTypeOperator.TypeOf, out MethodInfo opInfo))
                return GetPesudoScriptType(target.GetType());

            if (!(opInfo.Invoke(null, new[] { target }) is string scriptTypeName))
                throw new InvalidOperationException(
                    string.Format("Method {0} for operator {1} must return a {2}",
                        opInfo.Name,
                        Enum.GetName(typeof(ScriptableTypeOperator), ScriptableTypeOperator.TypeOf),
                        typeof(string).FullName
                    ));

            return scriptTypeName;
        }

        private static string GetPesudoScriptType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return (type.GetCustomAttribute(typeof(ScriptableTypeAttribute)) as ScriptableTypeAttribute)?.Name ?? type.FullName;
        }
    }
}
