﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace FryScript
{

    [ScriptableType("params")]
    public sealed class ScriptParams : ScriptObject, IEnumerable<object>
    {
        private readonly object[] _items;

        [ScriptableProperty("count")]
        public int Count { get { return _items.Length; } }

        [ScriptableIndex]
        public object this[int index]
        {
            get
            {
                if (index >= _items.Length)
                    throw new FryScriptException(string.Format("Index value {0} is out of bounds", index));

                return _items[index];
            }
        }

        public ScriptParams(params object[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            _items = items;
        }

        public T Get<T>(int index)
        {
            var value = this[index];

            if (value is T)
                return (T)value;

            throw new InvalidCastException(string.Format("Cannot cast value to type {0}", typeof(T).FullName));
        }

        public override IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
