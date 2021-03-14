using System;

namespace FryScript
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class ScriptableBaseAttribute : Attribute
    {
        private readonly string _name;

        public string Name { get { return _name; }}

        protected ScriptableBaseAttribute(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }
    }
}
