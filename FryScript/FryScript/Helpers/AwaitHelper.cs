namespace FryScript.Helpers
{
    public static class AwaitHelper
    {
        public static ScriptFibreContext EnsureAwait(object obj)
        {
            var fibreContext = obj as ScriptFibreContext;

            if (fibreContext == null)
                throw ExceptionHelper.NonAwaitable(obj?.GetType() ?? typeof(object));

            return fibreContext;
        }
    }
}
