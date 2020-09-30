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

            var parent = instance?.ObjectCore?.Builder?.Parent;

            var builder = method.Invoke(null, new object[] { name, instance, parent }) as IScriptObjectBuilder;

            return builder;
        }

        private static ScriptObjectBuilder<T> InternalGetInstanceBuilder<T>(string name, T instance, IScriptObjectBuilder parent)
            where T : IScriptObject, new()
        {
            var uri = RuntimeUri.GetRuntimeUri(name);

            return new ScriptObjectBuilder<T>(o => ExceptionHelper.InvalidCreateInstance(uri.AbsoluteUri), uri, parent);
        }
    }

    public class ScriptObjectBuilder<T> : IScriptObjectBuilder
        where T : IScriptObject, new()
    {
        private readonly Func<T> _factory;
        private readonly Func<IScriptObject, object> _ctor;
        private readonly IScriptObjectBuilder _parent;

        public Uri Uri { get; }

        public IScriptObjectBuilder Parent => _parent;

        public ScriptObjectBuilder(Func<IScriptObject, object> ctor, Uri uri, IScriptObjectBuilder parent)
            : this(() => new T(),
                  ctor,
                  uri,
                  parent)
        {
        }

        public ScriptObjectBuilder(Func<T> factory, Func<IScriptObject, object> ctor, Uri uri, IScriptObjectBuilder parent)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            _parent = parent;
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

        public bool Extends(IScriptObjectBuilder target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            if(this == target)
                return false;

            var curParent = _parent;

            while(curParent != null)
            {
                if(curParent == target)
                    return true;

                curParent = curParent.Parent;
            }

            return false;
        }
    }
}
