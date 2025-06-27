using System;
using System.Collections;
using System.Collections.Generic;

namespace Concurrency.Chess
{
    public class ThreadSafeSortedList<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly SortedList<TKey, TValue> _sortedList;
        private readonly object _root = null;

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_root)
                {
                    return new ThreadSafeList<TKey>(_sortedList.Keys);
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_root)
                {
                    return new ThreadSafeList<TValue>(_sortedList.Values);
                }
            }
        }

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).IsReadOnly;

        public ThreadSafeSortedList()
        {
            _sortedList = new SortedList<TKey, TValue>();
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public ThreadSafeSortedList(IDictionary<TKey, TValue> dictionary)
        {
            _sortedList = new SortedList<TKey, TValue>(dictionary);
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public void Add(TKey key, TValue value)
        {
            lock (_root)
            {
                _sortedList.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_root)
            {
                return _sortedList.ContainsKey(key);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_root)
            {
                return _sortedList.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_root)
            {
                return _sortedList.TryGetValue(key, out value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)_sortedList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_sortedList).GetEnumerator();
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_root)
                {
                    return _sortedList[key];
                }
            }
            set
            {
                lock (_root)
                {
                    _sortedList[key] = value;
                }
            }
        }

    }
}
