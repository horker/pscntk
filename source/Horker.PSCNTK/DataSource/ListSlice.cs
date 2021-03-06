﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class ListSlice<T> : IList<T>
    {
        private IList<T> _list;
        private int _offset;
        private int _count;

        public ListSlice(IList<T> source, int offset, int count)
        {
            if (offset >= source.Count)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + count > source.Count)
                throw new ArgumentOutOfRangeException("count");

            _list = source;
            _offset = offset;
            _count = count;
        }

        public T this[int index] { get => _list[index + _offset]; set { _list[index + _offset] = value; } }

        public int Count => _count;

        public bool IsReadOnly => true;

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < _count; ++i)
                array[arrayIndex + i] = _list[_offset + i];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < _count; ++i)
                if (_list[_offset + i].Equals(item))
                    return i;

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
           
        private class Enumerator : IEnumerator<T>
        {
            private ListSlice<T> _slice;
            private int _index;

            public Enumerator(ListSlice<T> slice)
            {
                _slice = slice;
                _index = -1;
            }

            public T Current => _slice[_index];

            object IEnumerator.Current => _slice[_index];

            public void Dispose()
            {
                // Do nothing
            }

            public bool MoveNext()
            {
                ++_index;
                return _index < _slice.Count;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
