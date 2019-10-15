using System;

namespace FryScript
{
    [ScriptableType("error")]
    public class ScriptError : ScriptObject
    {
        private static readonly ScriptObjectBuilder<ScriptError> Builder = new ScriptObjectBuilder<ScriptError>(o => o, new Uri("runtime://error.fry"));

        public ScriptError()
        {
            ObjectCore.Builder = Builder;
        }

        [ScriptableProperty("message")]
        public string Message { get; set; }

        [ScriptableProperty("innerObject")]
        public object InnerObject { get; set; }

        [ScriptableMethod("ctor")]
        public void Constructor(string message, object innerObject)
        {
            Message = message;
            InnerObject = innerObject;
        }
    }
}
