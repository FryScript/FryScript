namespace FryScript
{
    public static class Builder
    {
        public static IScriptObjectBuilder ScriptObjectBuilder { get; } = new ScriptObjectBuilder<ScriptObject>(o => o, RuntimeUri.ScriptObjectUri, null);  

        public static IScriptObjectBuilder ScriptArrayBuilder { get; } = new ScriptObjectBuilder<ScriptArray>(o => o, RuntimeUri.ScriptArrayUri, ScriptObjectBuilder);  

        public static IScriptObjectBuilder ScriptErrorBuilder { get; } = new ScriptObjectBuilder<ScriptError>(o => o, RuntimeUri.ScriptErrorUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFibreBuilder { get; } = new ScriptObjectBuilder<ScriptFibre>(o => o, RuntimeUri.ScriptFibreUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFibreContextBuilder { get; } = new ScriptObjectBuilder<ScriptFibreContext>(o => o, RuntimeUri.ScriptFibreContextUri, ScriptObjectBuilder);

        public static IScriptObjectBuilder ScriptFunctionBuilder { get; } = new ScriptObjectBuilder<ScriptFunction>(o => o, RuntimeUri.ScriptFunctionUri, ScriptObjectBuilder);
    }
}