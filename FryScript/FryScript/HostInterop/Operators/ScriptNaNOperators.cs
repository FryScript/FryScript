//namespace FryScript.HostInterop.Operators
//{
//    using Helpers;

//    public class ScriptNaNOperators
//    {
//        private const string NaNName = "[NaN]";

//        public static string ToString(ScriptNaN value)
//        {
//            return NaNName;
//        }

//        public static float ToSingle(ScriptNaN value)
//        {
//            return float.NaN;
//        }

//        public static int ToInt32(ScriptNaN value)
//        {
//            return 0;
//        }

//        public static bool ToBoolean(ScriptNaN value)
//        {
//            return false;
//        }

//        public static ScriptNaN ToScriptNaN(ScriptNaN value)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object ToObject(ScriptNaN value)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object Add(ScriptNaN value1, ScriptNaN value2)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object Subtract(ScriptNaN value1, ScriptNaN value2)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object Multiply(ScriptNaN value1, ScriptNaN value2)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object Divide(ScriptNaN value1, ScriptNaN value2)
//        {
//            return ScriptNaN.Value;
//        }

//        public static object Equal(ScriptNaN value1, ScriptNaN value2)
//        {
//            return false;
//        }

//        public static object NotEqual(ScriptNaN value1, ScriptNaN value2)
//        {
//            return true;
//        }

//        public static object GreaterThan(ScriptNaN value1, ScriptNaN value2)
//        {
//            return false;
//        }

//        public static object LessThan(ScriptNaN value1, ScriptNaN value2)
//        {
//            return false;
//        }

//        public static object GreaterThanOrEqual(ScriptNaN value1, ScriptNaN value2)
//        {
//            return false;
//        }

//        public static object LessThanOrEqual(ScriptNaN value1, ScriptNaN value2)
//        {
//            return false;
//        }

//        public static bool IsScriptType(string scriptType)
//        {
//            return ScriptNameHelper.Normalize(NaNName) == ScriptNameHelper.Normalize(scriptType);
//        }

//        public static bool ExtendsScriptType(string scriptType)
//        {
//            return IsScriptType(scriptType);
//        }

//        public static string GetScriptType()
//        {
//            return ScriptNameHelper.Normalize(NaNName);
//        }
//    }
//}
