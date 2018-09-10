using System;
using System.Collections.Generic;

namespace FryScript
{
    using System.Collections;
    using Helpers;
    using HostInterop;

    [ScriptableType("[array]")]
    public class ScriptArray : ScriptObject, IEnumerable<object>
    {
        private static readonly string ScriptTypeName = TypeProvider.Current.GetTypeName(typeof(ScriptArray));

        private readonly List<object> _items;

        [ScriptableProperty("count")]
        public int Count { get { return _items.Count; } }

        [ScriptableIndexAttribute]
        public object this[int index]
        {
            get
            {
                try
                {
                    return _items[index];
                }
                catch (Exception)
                {
                    throw ExceptionHelper.IndexOutOfBounds(index);
                }
            }
            set
            {
                try
                {
                    _items[index] = value;
                }
                catch (Exception)
                {
                    throw ExceptionHelper.IndexOutOfBounds(index);
                }
            }
        }

        public ScriptArray(params object[] items)
            : base(scriptType: ScriptTypeName)
        {
            _items = items == null
                ? new List<object>()
                : new List<object>(items);
        }

        public ScriptArray()
            : this(null)
        {
        }

        [ScriptableMethod("add")]
        public void Add(object item)
        {
            _items.Add(item);
        }

        [ScriptableMethod("remove")]
        public void Remove(int index)
        {
            try
            {
                _items.RemoveAt(index);
            }
            catch (Exception)
            {
                throw ExceptionHelper.IndexOutOfBounds(index);
            }
        }

        [ScriptableMethod("indexOf")]
        public void IndexOf(object item)
        {
            _items.IndexOf(item);
        }

        [ScriptableMethod("removeLast")]
        public void RemoveLast()
        {
            try
            {
                _items.RemoveAt(_items.Count - 1);
            }
            catch (Exception)
            {
                throw ExceptionHelper.IndexOutOfBounds(_items.Count - 1);
            }
        }

        [ScriptableMethod("removeFirst")]
        public void RemoveFirst()
        {
            try
            {
                _items.RemoveAt(0);
            }
            catch (Exception)
            {
                throw ExceptionHelper.IndexOutOfBounds(0);
            }
        }

        [ScriptableMethod("insert")]
        public void Insert(int index, object item)
        {
            try
            {
                _items.Insert(index, item);
            }
            catch (Exception)
            {
                throw ExceptionHelper.IndexOutOfBounds(index);
            }
        }

        public override IEnumerator<object> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public static List<object> GetItems(ScriptArray scriptArray)
        {
            if (scriptArray == null) 
                throw new ArgumentNullException("scriptArray");

            return scriptArray._items;
        }

        public static explicit operator List<object>(ScriptArray array)
        {
            return array._items;
        }

        public static explicit operator object[](ScriptArray array)
        {
            return array._items.ToArray();
        }
    }
}
