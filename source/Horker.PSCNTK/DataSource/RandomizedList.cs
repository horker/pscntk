using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class RandomizedList<T> : IList<T>
    {
        public static int[] GetRandomizedIndexes(int length)
        {
            var random = Random.GetInstance();

            var result = new int[length];
            for (var i = 0; i < length; ++i)
            {
                var j = random.Next(i + 1);
                if (j != i)
                    result[i] = result[j];
                result[j] = i;
            }

            return result;
        }

        private IList<T> _list;
        private int[] _order;
        private int _stride;

        public RandomizedList(IList<T> source, int[] randomizedIndexes, int stride)
        {
            if (source.Count % stride != 0)
                throw new ArgumentException("source length must be a multiplication of stride");

            if (source.Count != randomizedIndexes.Length * stride)
                throw new ArgumentException("randomizedIndexes times stride must be equal to souce length");

            _list = source;
            _order = randomizedIndexes;
            _stride = stride;
        }

        private int GetRandomizedIndex(int index)
        {
            return _order[index / _stride] * _stride + index % _stride;
        }

        public T this[int index]
        {
            get => _list[GetRandomizedIndex(index)];
            set
            {
                _list[GetRandomizedIndex(index)] = value;
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < _list.Count; ++i)
                array[arrayIndex + i] = this[i];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < _list.Count; ++i)
                if (this[i].Equals(item))
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
            private RandomizedList<T> _list;
            private int _index;

            public Enumerator(RandomizedList<T> list)
            {
                _list = list;
                _index = -1;
            }

            public T Current => _list[_index];

            object IEnumerator.Current => _list[_index];

            public void Dispose()
            {
                // Do nothing
            }

            public bool MoveNext()
            {
                ++_index;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
