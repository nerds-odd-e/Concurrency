using System;
using System.Collections;
using System.Collections.Generic;

namespace Concurrency.Chess
{
    public class ThreadSafeSortedList<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly SortedList<TKey, TValue> _sortedList;
        private readonly object _root = null;

        public IList<TKey> Keys
        {
            get
            {
                lock (_root)
                {
                    return new ThreadSafeList<TKey>(_sortedList.Keys);
                }
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        public IList<TValue> Values
        {
            get
            {
                lock (_root)
                {
                    return new ThreadSafeList<TValue>(_sortedList.Values);
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_root)
                {
                    return _sortedList.Count;
                }
            }
        }

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).IsReadOnly;

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

        public ThreadSafeSortedList()
        {
            _sortedList = new SortedList<TKey, TValue>();
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public ThreadSafeSortedList(int capacity)
        {
            _sortedList = new SortedList<TKey, TValue>(capacity);
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public ThreadSafeSortedList(IComparer<TKey> comparer)
        {
            _sortedList = new SortedList<TKey, TValue>(comparer);
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public ThreadSafeSortedList(int capacity, IComparer<TKey> comparer)
        {
            _sortedList = new SortedList<TKey, TValue>(capacity, comparer);
            System.Threading.Interlocked.CompareExchange(ref _root, new object(), null);
        }

        public ThreadSafeSortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            _sortedList = new SortedList<TKey, TValue>(dictionary, comparer);
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

        public int IndexOfKey(TKey key)
        {
            lock (_root)
            {
                return _sortedList.IndexOfKey(key);
            }
        }

        public bool ContainsValue(TValue value)
        {
            lock (_root)
            {
                return _sortedList.ContainsValue(value);
            }
        }

        public int IndexOfValue(TValue value)
        {
            lock (_root)
            {
                return _sortedList.IndexOfValue(value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_root)
            {
                return _sortedList.Remove(key);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_root)
            {
                _sortedList.RemoveAt(index);
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
            lock (_root)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Add(item);
            }
        }

        public void Clear()
        {
            lock (_root)
            {
                _sortedList.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_root)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_root)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_root)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_sortedList).Remove(item);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_root)
            {
                var copy = new SortedList<TKey, TValue>(_sortedList);
                return copy.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        public int Capacity
        {
            get
            {
                lock (_root)
                {
                    return _sortedList.Capacity;
                }
            }
            set
            {
                lock (_root)
                {
                    _sortedList.Capacity = value;
                }
            }
        }

        public IComparer<TKey> Comparer
        {
            get
            {
                return _sortedList.Comparer;
            }
        }

        public void TrimExcess()
        {
            lock (_root)
            {
                _sortedList.TrimExcess();
            }
        }
    }
}
