using System;

namespace FryScript
{
    public class ScriptObjectBuilderFactory : IScriptObjectBuilderFactory
    {
        public IScriptObjectBuilder Create(Type type, Func<IScriptObject, object> ctor, Uri uri)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            uri = uri ?? throw new ArgumentNullException(nameof(uri));

            var builderType = typeof(ScriptObjectBuilder<>).MakeGenericType(type);

            var builder = Activator.CreateInstance(builderType, ctor, uri) as IScriptObjectBuilder;

            return builder;
        }
    }
}
