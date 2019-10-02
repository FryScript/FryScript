namespace FryScript
{
    [ScriptableType("error")]
    public class ScriptError : ScriptObject
    {
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
