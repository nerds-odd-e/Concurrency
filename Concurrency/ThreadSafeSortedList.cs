using System;
using System.Collections.Generic;

namespace Concurrency.Chess
{
    public class ThreadSafeSortedList<TKey, TValue> : SortedList<TKey, TValue>
    {
        private readonly SortedList<TKey, TValue> _sortedList;
        private readonly object _root = null;

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

        public new void Add(TKey key, TValue value)
        {
            lock (_root)
            {
                _sortedList.Add(key, value);
            }
        }

        public new TValue this[TKey key]
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

        //public new IList<TValue> Values
        //{
        //    get
        //    {
        //        return new ThreadSafeList<TValue>(_sortedList.Values);
        //    }
        //}
    }
}
