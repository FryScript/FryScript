using FryScript.HostInterop;
using System;
using System.Reflection;

namespace FryScript.Helpers
{

    public static class ScriptTypeHelper
    {
        public static string NormalizeTypeName(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            return name.Replace('\\', '/').ToLower();
        }

        public static string GetScriptType(object target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            if (target is IScriptType scriptType)
                return scriptType.GetScriptType();

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

        public static bool IsScriptType(object target, object value)
        {
            var targetType = target?.GetType();
            var valueType = value?.GetType();

            //if (targetType == null || valueType == null)
                //return false;

            return targetType == valueType;
           /* target = target ?? throw new ArgumentNullException(nameof(target));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var compareType = GetScriptType(value);

            if (target is IScriptType scriptTypeTarget)
                return scriptTypeTarget.IsScriptType(compareType);

            var isScriptType = GetScriptType(target) == compareType;

            return isScriptType;*/
        }

        public static bool ExtendsScriptType(object target, object value)
        {
            var subType = target.GetType();
            var superType = value.GetType();

            if (superType.IsAssignableFrom(subType))
                return true;

            if (target is IScriptObject && value is ScriptObject)
                return true;
            
            throw new NotImplementedException();
            //target = target ?? throw new ArgumentNullException(nameof(target));
            //value = value ?? throw new ArgumentNullException(nameof(value));

            //var compareType = GetScriptType(value);

            //if (target is IScriptType scriptTypeTarget)
            //    return scriptTypeTarget.ExtendsScriptType(compareType);

            //return GetScriptType(target) == compareType;
        }

        public static string GetPesudoScriptType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return NormalizeTypeName(string.Format("[{0}]", type.FullName));
        }
    }
}
