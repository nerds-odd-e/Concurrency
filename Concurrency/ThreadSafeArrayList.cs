using System;
using System.Collections;
using System.Collections.Generic;

namespace Concurrency
{
    public class ThreadSafeArrayList : ArrayList
    {
        private ArrayList _list;
        private object _root;

        public ThreadSafeArrayList(ArrayList list)
        {
            _list = list;
            _root = list.SyncRoot;
        }

        public override int Capacity
        {
            get
            {
                lock (_root)
                {
                    return _list.Capacity;
                }
            }
            set
            {
                lock (_root)
                {
                    _list.Capacity = value;
                }
            }
        }

        public override int Count
        {
            get { lock (_root) { return _list.Count; } }
        }

        public override bool IsReadOnly
        {
            get { return _list.IsReadOnly; }
        }

        public override bool IsFixedSize
        {
            get { return _list.IsFixedSize; }
        }


        public override bool IsSynchronized
        {
            get { return true; }
        }

        public override Object this[int index]
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

        public override Object SyncRoot
        {
            get { return _root; }
        }

        public override int Add(Object value)
        {
            lock (_root)
            {
                return _list.Add(value);
            }
        }

        public override void AddRange(ICollection c)
        {
            lock (_root)
            {
                _list.AddRange(c);
            }
        }

        public override int BinarySearch(Object value)
        {
            lock (_root)
            {
                return _list.BinarySearch(value);
            }
        }

        public override int BinarySearch(Object value, IComparer comparer)
        {
            lock (_root)
            {
                return _list.BinarySearch(value, comparer);
            }
        }

        public override int BinarySearch(int index, int count, Object value, IComparer comparer)
        {
            lock (_root)
            {
                return _list.BinarySearch(index, count, value, comparer);
            }
        }

        public override void Clear()
        {
            lock (_root)
            {
                _list.Clear();
            }
        }

        public override Object Clone()
        {
            lock (_root)
            {
                return new ThreadSafeArrayList((ArrayList)_list.Clone());
            }
        }

        public override bool Contains(Object item)
        {
            lock (_root)
            {
                return _list.Contains(item);
            }
        }

        public override void CopyTo(Array array)
        {
            lock (_root)
            {
                _list.CopyTo(array);
            }
        }

        public override void CopyTo(Array array, int index)
        {
            lock (_root)
            {
                _list.CopyTo(array, index);
            }
        }

        public override void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            lock (_root)
            {
                _list.CopyTo(index, array, arrayIndex, count);
            }
        }

        public override IEnumerator GetEnumerator()
        {
            lock (_root)
            {
                ArrayList copy = (ArrayList)_list.Clone();
                return copy.GetEnumerator();
            }
        }

        public override IEnumerator GetEnumerator(int index, int count)
        {
            lock (_root)
            {
                ArrayList copy = (ArrayList)_list.Clone();
                return copy.GetEnumerator(index, count);
            }
        }

        public override int IndexOf(Object value)
        {
            lock (_root)
            {
                return _list.IndexOf(value);
            }
        }

        public override int IndexOf(Object value, int startIndex)
        {
            lock (_root)
            {
                return _list.IndexOf(value, startIndex);
            }
        }

        public override int IndexOf(Object value, int startIndex, int count)
        {
            lock (_root)
            {
                return _list.IndexOf(value, startIndex, count);
            }
        }

        public override void Insert(int index, Object value)
        {
            lock (_root)
            {
                _list.Insert(index, value);
            }
        }

        public override void InsertRange(int index, ICollection c)
        {
            lock (_root)
            {
                _list.InsertRange(index, c);
            }
        }

        public override int LastIndexOf(Object value)
        {
            lock (_root)
            {
                return _list.LastIndexOf(value);
            }
        }

        public override int LastIndexOf(Object value, int startIndex)
        {
            lock (_root)
            {
                return _list.LastIndexOf(value, startIndex);
            }
        }

        public override int LastIndexOf(Object value, int startIndex, int count)
        {
            lock (_root)
            {
                return _list.LastIndexOf(value, startIndex, count);
            }
        }

        public override void Remove(Object value)
        {
            lock (_root)
            {
                _list.Remove(value);
            }
        }

        public override void RemoveAt(int index)
        {
            lock (_root)
            {
                _list.RemoveAt(index);
            }
        }

        public override void RemoveRange(int index, int count)
        {
            lock (_root)
            {
                _list.RemoveRange(index, count);
            }
        }

        public override void Reverse(int index, int count)
        {
            lock (_root)
            {
                _list.Reverse(index, count);
            }
        }

        public override void SetRange(int index, ICollection c)
        {
            lock (_root)
            {
                _list.SetRange(index, c);
            }
        }

        public override ArrayList GetRange(int index, int count)
        {
            lock (_root)
            {
                return _list.GetRange(index, count);
            }
        }

        public override void Sort()
        {
            lock (_root)
            {
                _list.Sort();
            }
        }

        public override void Sort(IComparer comparer)
        {
            lock (_root)
            {
                _list.Sort(comparer);
            }
        }

        public override void Sort(int index, int count, IComparer comparer)
        {
            lock (_root)
            {
                _list.Sort(index, count, comparer);
            }
        }

        public override Object[] ToArray()
        {
            lock (_root)
            {
                return _list.ToArray();
            }
        }

        public override Array ToArray(Type type)
        {
            lock (_root)
            {
                return _list.ToArray(type);
            }
        }

        public override void TrimToSize()
        {
            lock (_root)
            {
                _list.TrimToSize();
            }
        }

        public ThreadSafeArrayList()
        {
            _list = new ArrayList();
            _root = _list.SyncRoot;
        }

        public ThreadSafeArrayList(int capacity)
        {
            _list = new ArrayList(capacity);
            _root = _list.SyncRoot;
        }

    }
}
