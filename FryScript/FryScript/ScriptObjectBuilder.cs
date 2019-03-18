using System;

namespace FryScript
{
    public class ScriptObjectBuilder<T> : IScriptObjectBuilder
        where T : IScriptObject, new()
    {
        private readonly Func<IScriptObject, object> _ctor;

        public Uri Uri { get; }

        public ScriptObjectBuilder(Func<IScriptObject, object> ctor, Uri uri)
        {
            _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public IScriptObject Build()
        {
            var instance = new T();

            _ctor(instance);

            if(instance.ObjectCore != null)
                instance.ObjectCore.Builder = this;

            return instance;
        }
    }
}
