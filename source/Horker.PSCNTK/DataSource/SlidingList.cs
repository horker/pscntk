using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class SlidingList<T> : IList<T>
    {
        private List<IList<T>> _lists;
        private int _offset;

        public SlidingList()
        {
            _lists = new List<IList<T>>();
            _offset = 0;
        }

        public SlidingList(IList<IList<T>> lists)
        {
            _lists = new List<IList<T>>(lists);
            _offset = 0;
        }

        internal Tuple<int, int> GetListIndexes(int index)
        {
            index += _offset;
            for (var i = 0; i < _lists.Count; ++i)
            {
                if (index < _lists[i].Count)
                    return new Tuple<int, int>(i, index);
                index -= _lists[i].Count;
            }
            throw new ArgumentOutOfRangeException("index");
        }

        public IList<T> GetList(int index)
        {
            return _lists[index];
        }

        public int ListCount => _lists.Count;

        public void AddList(IList<T> list)
        {
            _lists.Add(list);
        }

        public void SkipElements(int n)
        {
            var offset = _offset + n;

            for (var i = 0; i < _lists.Count - 1; ++i)
            {
                if (offset < _lists[i].Count)
                {
                    _offset = offset;
                    if (i > 0)
                        _lists.RemoveRange(0, i);
                    return;
                }

                offset -= _lists[i].Count;
            }

            if (offset < _lists[_lists.Count - 1].Count)
            {
                _lists[0] = _lists[_lists.Count - 1];
                _lists.RemoveRange(1, _lists.Count - 1);
                _offset = offset;
                return;
            }

            if (offset == _lists[_lists.Count - 1].Count)
            {
                _lists.Clear();
                _offset = 0;
                return;
            }

            throw new ArgumentOutOfRangeException("n");
        }

        public T this[int index]
        {
            get
            {
                var indexes = GetListIndexes(index);
                return _lists[indexes.Item1][indexes.Item2];
            }

            set
            {
                var indexes = GetListIndexes(index);
                _lists[indexes.Item1][indexes.Item2] = value;
            }
        }

        public int Count => _lists.Sum(x => x.Count) - _offset;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            for (var i = 0; i < _lists.Count; ++i)
                _lists[i].Clear();
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < _lists.Count; ++i)
                if (_lists[i].Contains(item))
                    return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (Count == 0)
                return;

            foreach (var e in this)
                array[arrayIndex++] = e;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new JoinedListEnumerator<T>(this);
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            var indexes = GetListIndexes(index);
            _lists[indexes.Item1].Insert(indexes.Item2, item);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            var indexes = GetListIndexes(index);
            _lists[indexes.Item1].RemoveAt(indexes.Item2);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new JoinedListEnumerator<T>(this);
        }
    }

    public class JoinedListEnumerator<T> : IEnumerator<T>
    {
        private SlidingList<T> _joinedList;
        private int _listIndex;
        private int _index;

        public JoinedListEnumerator(SlidingList<T> joinedList)
        {
            _joinedList = joinedList;
            Reset();
        }

        public T Current => _joinedList.GetList(_listIndex)[_index];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            var l = _joinedList.GetList(_listIndex);
            ++_index;
            if (_index >= l.Count)
            {
                ++_listIndex;
                if (_listIndex >= _joinedList.ListCount)
                    return false;
                _index = 0;
            }

            return true;
        }

        public void Reset()
        {
            if (_joinedList.ListCount == 0)
            {
                _listIndex = 0;
                _index = -1;
            }
            else
            {
                var indexes = _joinedList.GetListIndexes(0);
                _listIndex = indexes.Item1;
                _index = indexes.Item2 - 1;
            }
        }
    }
}
