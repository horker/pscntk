using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class JoinedList<T> : IList<T>
    {
        private List<IList<T>> _lists;
        private int _offset;

        public JoinedList()
        {
            _lists = new List<IList<T>>();
            _offset = 0;
        }

        public JoinedList(IList<IList<T>> lists)
        {
            _lists = new List<IList<T>>(lists);
            _offset = 0;
        }

        internal Tuple<int, int> GetListIndex(int index)
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

        public void SkipElements(int n)
        {
            var offset = _offset += n;

            for (var i = 0; i < _lists.Count; ++i)
            {
                if (offset < _lists[i].Count)
                {
                    _offset = offset;
                    for (var j = 0; j < i; ++j)
                        _lists.RemoveAt(0);
                    return;
                }

                offset -= _lists[i].Count;
            }
        }

        public T this[int index]
        {
            get
            {
                var indexes = GetListIndex(index);
                return _lists[indexes.Item1][indexes.Item2];
            }

            set
            {
                var indexes = GetListIndex(index);
                _lists[indexes.Item1][indexes.Item2] = value;
            }
        }

        public int Count => _lists.Sum(x => x.Count) - _offset;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            _lists.Last().Add(item);
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
            var indexes = GetListIndex(index);
            _lists[indexes.Item1].Insert(indexes.Item2, item);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            var indexes = GetListIndex(index);
            _lists[indexes.Item1].RemoveAt(indexes.Item2);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new JoinedListEnumerator<T>(this);
        }
    }

    public class JoinedListEnumerator<T> : IEnumerator<T>
    {
        private JoinedList<T> _joinedList;
        private int _listIndex;
        private int _index;

        public JoinedListEnumerator(JoinedList<T> joinedList)
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
            var indexes = _joinedList.GetListIndex(0);
            _listIndex = indexes.Item1;
            _index = indexes.Item2 - 1;
        }
    }
}
