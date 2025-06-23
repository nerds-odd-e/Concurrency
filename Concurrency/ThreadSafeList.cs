using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrency
{
    public class ThreadSafeList<T> : List<T>
    {
        private List<T> _list;
        private object _root;

        public ThreadSafeList(List<T> list)
        {
            _list = list;
            _root = ((ICollection)list).SyncRoot;
        }

        public new int Count
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

        public new void Add(T item)
        {
            lock (_root)
            {
                _list.Add(item);
            }
        }

        public new void Clear()
        {
            lock (_root)
            {
                _list.Clear();
            }
        }

        public new bool Contains(T item)
        {
            lock (_root)
            {
                return _list.Contains(item);
            }
        }

        public new void CopyTo(T[] array, int arrayIndex)
        {
            lock (_root)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        public new bool Remove(T item)
        {
            lock (_root)
            {
                return _list.Remove(item);
            }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            lock (_root)
            {
                var copy = new List<T>(_list);
                return copy.GetEnumerator();
            }
        }

        public new T this[int index]
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

        public new int IndexOf(T item)
        {
            lock (_root)
            {
                return _list.IndexOf(item);
            }
        }

        public new void Insert(int index, T item)
        {
            lock (_root)
            {
                _list.Insert(index, item);
            }
        }

        public new void RemoveAt(int index)
        {
            lock (_root)
            {
                _list.RemoveAt(index);
            }
        }

        public new int Capacity
        {
            get { lock (_root) { return _list.Capacity; } }
            set { lock (_root) { _list.Capacity = value; } }
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            lock (_root)
            {
                _list.AddRange(collection);
            }
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (_root)
            {
                _list.InsertRange(index, collection);
            }
        }

        public new void RemoveRange(int index, int count)
        {
            lock (_root)
            {
                _list.RemoveRange(index, count);
            }
        }

        public new int RemoveAll(Predicate<T> match)
        {
            lock (_root)
            {
                return _list.RemoveAll(match);
            }
        }

        public new int IndexOf(T item, int index)
        {
            lock (_root)
            {
                return _list.IndexOf(item, index);
            }
        }

        public new int IndexOf(T item, int index, int count)
        {
            lock (_root)
            {
                return _list.IndexOf(item, index, count);
            }
        }

        public new int LastIndexOf(T item)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item);
            }
        }

        public new int LastIndexOf(T item, int index)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item, index);
            }
        }

        public new int LastIndexOf(T item, int index, int count)
        {
            lock (_root)
            {
                return _list.LastIndexOf(item, index, count);
            }
        }

        public new void Reverse()
        {
            lock (_root)
            {
                _list.Reverse();
            }
        }

        public new void Reverse(int index, int count)
        {
            lock (_root)
            {
                _list.Reverse(index, count);
            }
        }

        public new void Sort()
        {
            lock (_root)
            {
                _list.Sort();
            }
        }

        public new void Sort(IComparer<T> comparer)
        {
            lock (_root)
            {
                _list.Sort(comparer);
            }
        }
    }
}
