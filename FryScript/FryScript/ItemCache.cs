using System.Collections.Generic;

namespace FryScript
{
    internal class ItemCache<TKey, TValue>
    {
        private struct CacheItem
        {
            public TValue Value;
            public LinkedListNode<TKey> ItemNode;
        }

        private readonly LinkedList<TKey> _aliveItems = new LinkedList<TKey>();
        private readonly Dictionary<TKey, CacheItem> _items = new Dictionary<TKey, CacheItem>();
        private readonly int _limit;

        public int Count
        {
            get { return _items.Count; }
        }

        public ItemCache(int limit)
        {
            _limit = limit;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            CacheItem cacheItem;
            if (!_items.TryGetValue(key, out cacheItem))
            {
                value = default(TValue);
                return false;
            }

            var itemNode = cacheItem.ItemNode;
            _aliveItems.Remove(itemNode);
            _aliveItems.AddFirst(itemNode);

            value = cacheItem.Value;
            return true;
        }

        public void Add(TKey key, TValue value)
        {
            var itemNode = new LinkedListNode<TKey>(key);
            var cacheItem = new CacheItem
            {
                ItemNode = itemNode,
                Value = value
            };

            _aliveItems.AddFirst(itemNode);
            _items.Add(key, cacheItem);

            if (_items.Count <= _limit)
                return;

            _items.Remove(_aliveItems.Last.Value);
            _aliveItems.RemoveLast();
        }
    }
}
