namespace FryScript
{
    public static class Builder
    {
        public static IScriptObjectBuilder ScriptObjectBuilder = new ScriptObjectBuilder<ScriptObject>(o => o, RuntimeUri.ScriptObjectUri, null);  

        public static IScriptObjectBuilder ScriptArrayBuilder = new ScriptObjectBuilder<ScriptArray>(o => o, RuntimeUri.ScriptArrayUri, ScriptObjectBuilder);  

        public static IScriptObjectBuilder ScriptErrorBuilder = new ScriptObjectBuilder<ScriptError>(o => o, RuntimeUri.ScriptErrorUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFibreBuilder = new ScriptObjectBuilder<ScriptFibre>(o => o, RuntimeUri.ScriptFibreUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFibreContextBuilder = new ScriptObjectBuilder<ScriptFibreContext>(o => o, RuntimeUri.ScriptFibreUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFunctionBuilder = new ScriptObjectBuilder<ScriptFunction>(o => o, RuntimeUri.ScriptFunctionUri, ScriptObjectBuilder);
    }
}