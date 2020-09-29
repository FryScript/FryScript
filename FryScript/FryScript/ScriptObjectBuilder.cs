using System;
using System.Linq;
using System.Reflection;
using FryScript.Helpers;

namespace FryScript
{
    public static class ScriptObjectBuilder
    {
        public static IScriptObjectBuilder GetInstanceBuilder(string name, IScriptObject instance)
        {
            var method = (from m in typeof(ScriptObjectBuilder).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                         where m.IsGenericMethod && m.Name == nameof(ScriptObjectBuilder.InternalGetInstanceBuilder)
                         select m)
                         .Single();

            method = method.MakeGenericMethod(instance.GetType());

            var builder = method.Invoke(null, new object[] { name, instance }) as IScriptObjectBuilder;

            return builder;
        }

        private static ScriptObjectBuilder<T> InternalGetInstanceBuilder<T>(string name, T instance)
            where T : IScriptObject, new()
        {
            var uri = RuntimeUri.GetRuntimeUri(name);

            return new ScriptObjectBuilder<T>(o => ExceptionHelper.InvalidCreateInstance(uri.AbsoluteUri), uri);
        }
    }

    public class ScriptObjectBuilder<T> : IScriptObjectBuilder
        where T : IScriptObject, new()
    {
        private readonly Func<T> _factory;
        private readonly Func<IScriptObject, object> _ctor;

        public Uri Uri { get; }

        public ScriptObjectBuilder(Func<IScriptObject, object> ctor, Uri uri)
            : this(() => new T(),
                  ctor,
                  uri)
        {
        }

        public ScriptObjectBuilder(Func<T> factory, Func<IScriptObject, object> ctor, Uri uri)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public IScriptObject Build()
        {
            var instance = _factory();

            _ctor(instance);

            if (instance.ObjectCore != null)
                instance.ObjectCore.Builder = this;

            return instance;
        }

        public IScriptObject Extend(IScriptObject obj)
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));

            _ctor(obj);

            return obj;
        }
    }
}
