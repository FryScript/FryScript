using System;

namespace FryScript
{
    public struct ScriptTuple
    {
        private readonly object[] _items;

        public object this[int index]
        {
            get
            {
                if (_items != null && index < _items.Length)
                    return _items[index];

                return null;
            }
        }

        public ScriptTuple(params object[] items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public static ScriptTuple WrapTuple(object obj)
        {
            if (obj is ScriptTuple tuple)
                return tuple;

            return new ScriptTuple(obj);
        }
    }
}
