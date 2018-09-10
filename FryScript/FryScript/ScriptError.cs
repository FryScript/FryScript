namespace FryScript
{
    [ScriptableType("[error]")]
    public class ScriptError : IScriptable
    {
        public dynamic Script { get; set; }

        [ScriptableProperty("message")]
        public string Message { get; set; }

        [ScriptableProperty("innerObject")]
        public dynamic InnerObject { get; set; }

        [ScriptableMethod("ctor")]
        public void Ctor(string message, object innerObject)
        {
            Message = message;
            InnerObject = innerObject;
        }
    }
}
