using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class RingBuffer<T>
    {
        private T[] _data;
        private int _nextRead;
        private int _nextWrite;

        public RingBuffer()
            : this(256)
        { }

        public RingBuffer(int capacity)
        {
            _data = new T[capacity];
            _nextRead = 0;
            _nextWrite = 0;
        }

        public int Capacity => _data.Length;

        public T ReadNext()
        {
            var value = _data[_nextRead];

            ++_nextRead;
            if (_nextRead >= _data.Length)
                _nextRead = 0;

            return value;
        }

        public T Peek()
        {
            return _data[_nextRead];
        }

        public void WriteNext(T value)
        {
            _data[_nextWrite] = value;

            ++_nextWrite;
            if (_nextWrite >= _data.Length)
                _nextWrite = 0;
        }
    }
}