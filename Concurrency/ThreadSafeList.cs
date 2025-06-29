using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrency
{
    public class ThreadSafeList<T> : IList<T>
    {
        private List<T> _list;
        private object _root;

        public ThreadSafeList(IList<T> list)
        {
            _list = new List<T>(list);
            _root = ((ICollection)_list).SyncRoot;
        }

        public int Count
        {
            get
            {
                lock (_root)
                {
                    return _list.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)_list).IsReadOnly;
            }
        }

        public void Add(T item)
        {
            lock (_root)
            {
                _list.Add(item);
            }
        }

        public void Clear()
        {
            lock (_root)
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_root)
            {
                return _list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_root)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (_root)
            {
                return _list.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_root)
            {
                var copy = new List<T>(_list);
                return copy.GetEnumerator();
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_root)
                {
                    return _list[index];
                }
            }
            set
            {
                lock (_root)
                {
                    _list[index] = value;
                }
            }
        }

        public int IndexOf(T item)
        {
            lock (_root)
            {
                return _list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_root)
            {
                _list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_root)
            {
                _list.RemoveAt(index);
            }
        }

        public int Capacity
        {
            get { lock (_root) { return _list.Capacity; } }
            set { lock (_root) { _list.Capacity = value; } }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            lock (_root)
            {
                _list.AddRange(collection);
            }
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (_root)
            {
                _list.InsertRange(index, collection);
            }
        }

        public void RemoveRange(int index, int count)
        {
            lock (_root)
            {
                _list.RemoveRange(index, count);
            }
        }

        public int RemoveAll(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.RemoveAll(match);
            }
        }

        public int IndexOf(T item, int index)
        {
            lock (_root)
            {
                return _list.IndexOf(item, index);
            }
        }

        public int IndexOf(T item, int index, int count)
        {
            lock (_root)
            {
                return _list.IndexOf(item, index, count);
            }
        }

        public int LastIndexOf(T item)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item);
            }
        }

        public int LastIndexOf(T item, int index)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item, index);
            }
        }

        public int LastIndexOf(T item, int index, int count)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item, index, count);
            }
        }

        public void Reverse()
        {
            lock (_root)
            {
                _list.Reverse();
            }
        }

        public void Reverse(int index, int count)
        {
            lock (_root)
            {
                _list.Reverse(index, count);
            }
        }

        public void Sort()
        {
            lock (_root)
            {
                _list.Sort();
            }
        }

        public void Sort(IComparer<T> comparer)
        {
            lock (_root)
            {
                _list.Sort(comparer);
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            lock (_root)
            {
                _list.Sort(comparison);
            }
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (_root)
            {
                _list.Sort(index, count, comparer);
            }
        }

        public void TrimExcess()
        {
            lock (_root)
            {
                _list.TrimExcess();
            }
        }

        public int BinarySearch(T item)
        {
            lock (_root)
            {
                return _list.BinarySearch(item);
            }
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            lock (_root)
            {
                return _list.BinarySearch(item, comparer);
            }
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            lock (_root)
            {
                return _list.BinarySearch(index, count, item, comparer);
            }
        }

        public T Find(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.Find(match);
            }
        }

        public ThreadSafeList<T> FindAll(Predicate<T> match)
        {
            lock (_root)
            {
                return new ThreadSafeList<T>(_list.FindAll(match));
            }
        }

        public int FindIndex(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindIndex(match);
            }
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindIndex(startIndex, match);
            }
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindIndex(startIndex, count, match);
            }
        }

        public T FindLast(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindLast(match);
            }
        }

        public int FindLastIndex(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindLastIndex(match);
            }
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindLastIndex(startIndex, match);
            }
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            lock (_root)
            {
                return _list.FindLastIndex(startIndex, count, match);
            }
        }

        public bool Exists(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.Exists(match);
            }
        }

        public bool TrueForAll(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.TrueForAll(match);
            }
        }

        public void ForEach(Action<T> action)
        {
            List<T> snapshot;
            lock (_root)
            {
                snapshot = new List<T>(_list);
            }
            snapshot.ForEach(action);
        }

        public ThreadSafeList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            lock (_root)
            {
                return new ThreadSafeList<TOutput>(_list.ConvertAll(converter));
            }
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            lock (_root)
            {
                return new ReadOnlyCollection<T>(this);
            }
        }

        public ThreadSafeList()
        {
            _list = new List<T>();
            _root = ((ICollection)_list).SyncRoot;
        }

        public ThreadSafeList(int capacity)
        {
            _list = new List<T>(capacity);
            _root = ((ICollection)_list).SyncRoot;
        }

        public void CopyTo(T[] array)
        {
            lock (_root)
            {
                _list.CopyTo(array);
            }
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            lock (_root)
            {
                _list.CopyTo(index, array, arrayIndex, count);
            }
        }

        public ThreadSafeList<T> GetRange(int index, int count)
        {
            lock (_root)
            {
                return new ThreadSafeList<T>(_list.GetRange(index, count));
            }
        }

        public T[] ToArray()
        {
            lock (_root)
            {
                return _list.ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object SyncRoot
        {
            get { return _root; }
        }

    }
}
