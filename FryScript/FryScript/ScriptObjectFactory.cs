using FryScript.HostInterop;
using System;

namespace FryScript
{
    public class ScriptObjectFactory : IScriptObjectFactory
    {
        private readonly Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder, IScriptObjectBuilder> _factory;
        private readonly ITypeFactory _typeFactory;

        public ScriptObjectFactory()
            : this((t, c, u, p) => Activator.CreateInstance(t, c, u, p) as IScriptObjectBuilder,
                  TypeFactory.Current)
        {
        }

        public ScriptObjectFactory(Func<Type, Func<IScriptObject, object>, Uri, IScriptObjectBuilder, IScriptObjectBuilder> factory, ITypeFactory typeFactory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _typeFactory = typeFactory ?? throw new ArgumentNullException(nameof(typeFactory));
        }

        public IScriptObject Create(Type type, Func<IScriptObject, object> ctor, Uri uri, IScriptObjectBuilder parent)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            uri = uri ?? throw new ArgumentNullException(nameof(uri));
            parent = parent ?? throw new ArgumentNullException(nameof(parent));

            var scriptableType = _typeFactory.CreateScriptableType(type);
            var builderType = typeof(ScriptObjectBuilder<>).MakeGenericType(scriptableType);
            var builder = _factory(builderType, ctor, uri, parent);
            var instance = builder.Build();

            return instance;
        }

        public IScriptObject CreatePrimitive(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            var scriptPrimitiveType = typeof(ScriptPrimitive<>).MakeGenericType(type);

            var obj = Activator.CreateInstance(scriptPrimitiveType);

            return obj as IScriptObject;
        }
    }
}
