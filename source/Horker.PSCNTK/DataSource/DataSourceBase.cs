using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class DataSourceBase<T, C> : IDataSource<T>
        where C: IList<T>
    {
        private Shape _shape;
        private C _data;

        public Shape Shape => _shape;
        public IList<T> Data => _data;
        public C TypedData => _data;

        #region Constructors and factories

        public DataSourceBase(C data, int[] dimensions = null)
        {
            if (dimensions == null || dimensions.Length == 0)
                dimensions = new int[] { data.Count };

            _shape = new Shape(dimensions, data.Count);
            _data = data;
        }

        #endregion

        #region Accessors

        public T this[params int[] indexes]
        {
            get => _data[_shape.GetSequentialIndex(indexes)];
            set { _data[_shape.GetSequentialIndex(indexes)] = value; }
        }

        public CNTK.StreamConfiguration GetStreamConfiguration(string name, string alias = "")
        {
            if (_shape.Rank < 3)
                throw new NotSupportedException("Shape should contain sequence and batch axes as the last two");

            return new CNTK.StreamConfiguration(name, _shape.GetSize(_shape.Rank - 3), false, alias, false);
        }

        #endregion

        #region Converters

        public override string ToString()
        {
            return Converter.ArrayToString("DataSource", this._data, this._shape, false);
        }

        public string AsString()
        {
            return Converter.ArrayToString("DataSource", this._data, this._shape, true);
        }

        public T[] ToArray()
        {
            return _data.ToArray();
        }

        public static implicit operator T[](DataSourceBase<T, C> source)
        {
            return source.Data.ToArray();
        }

        public CNTK.NDArrayView ToNDArrayView(CNTK.DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            return Converter.ArrayToNDArrayView(_data.Select(x => Convert.ToSingle(x)).ToArray(), _shape.Dimensions, device);
        }

        public CNTK.Value ToValue(CNTK.DeviceDescriptor device = null)
        {
            return new CNTK.Value(ToNDArrayView(device));
        }

        public CNTK.Variable ToVariable(CNTK.DeviceDescriptor device = null)
        {
            return new CNTK.Constant(ToNDArrayView(device));
        }

        public CNTK.MinibatchData ToMinibatchData(bool sweepEnd = false)
        {
            if (_shape.Rank < 3)
                throw new ArgumentException("To make a minibatch data, sequence and batch axis is necessary");

            return new CNTK.MinibatchData(ToValue(), (uint)_shape[-1], (uint)(_shape[-1] * _shape[-2]), sweepEnd);
        }

        public DataSourceBase<float, float[]> ToDataSourceFloat()
        {
            var data = _data.Select(x => Convert.ToSingle(x)).ToArray();
            return new DataSourceBase<float, float[]>(data, _shape);
        }

        public Bitmap ToBitmap(ImageFormat imageFormat, bool scale)
        {
            return DataSourceToBitmap<T>.Do(this, imageFormat, scale);
        }

        #endregion

        #region Serializer

        public byte[] Serialize(bool compress = true)
        {
            return Serializer.Serialize(this, compress);
        }

        public void Save(string path, bool compress = true)
        {
            Serializer.Serialize(this, path, compress);
        }

        #endregion

        #region Manipulators

        public void Reshape(params int[] dimensions)
        {
            _shape.Reshape(dimensions, _data.Count);
        }

        public void ApplyInPlace(Func<int, T, T> func)
        {
            for (var i = 0; i < _data.Count; ++i)
                _data[i] = func.Invoke(i, _data[i]);
        }

        internal static void Copy(IList<T> from, int fromOffset, IList<T> to, int toOffset, int size)
        {
            for (var i = 0; i < size; ++i)
            {
                to[toOffset + i] = from[fromOffset + i];
            }
        }

        public DataSourceBase<T, T[]> GetSubsequences(int subseqLength, int step = 1, int sequenceAxis = -1)
        {
            if (sequenceAxis == -1)
                sequenceAxis = _shape.Rank - 2;

            var seqDim = _shape.Dimensions[sequenceAxis];
            var sampleAxis = _shape.Rank - 1;
            var sampleDim = _shape.Dimensions[sampleAxis];

            var repeatLength = (seqDim - subseqLength) / step + 1;
            if (repeatLength < 1)
                throw new ArgumentException("Sequence too short");

            var valueSize = _shape.GetSize(sequenceAxis - 1);
            var sampleSize = _shape.GetSize(sequenceAxis);

            var newSampleSize = valueSize * subseqLength * repeatLength;

            var newDims = _shape.Dimensions.Clone() as int[];
            newDims[sampleAxis] = sampleDim * repeatLength;
            newDims[sequenceAxis] = subseqLength;

            var newData = new T[new Shape(newDims).TotalSize];

            for (var sampleCount = 0; sampleCount < sampleDim; ++sampleCount)
            {
                for (var seqCount = 0; seqCount < repeatLength; ++seqCount)
                {
                    for (var offset = 0; offset < subseqLength; ++offset)
                    {
                        Copy(
                            _data,
                            sampleCount * sampleSize + (seqCount * step + offset) * valueSize,
                            newData,
                            sampleCount * newSampleSize + (seqCount * subseqLength + offset) * valueSize,
                            valueSize
                        );
                    }
                }
            }

            return new DataSourceBase<T, T[]>(newData, newDims);
        }

        IDataSource<T> IDataSource<T>.GetSubsequences(int subseqLength, int step, int sequenceAxis)
        {
            return GetSubsequences(subseqLength, step, sequenceAxis);
        }

        public void Shuffle(int? seed = null)
        {
            int count = _shape[_shape.Rank - 1];
            int chunkSize = _shape.GetSize(_shape.Rank - 2);

            System.Random random;
            if (seed.HasValue)
                random = Random.GetInstance(seed.Value);
            else
                random = Random.GetInstance();

            var temp = new T[chunkSize];
            for (var i = 0; i < count; ++i)
            {
                var j = random.Next(count);
                Copy(_data, i * chunkSize, temp, 0, chunkSize);
                Copy(_data, j * chunkSize, _data, i * chunkSize, chunkSize);
                Copy(temp, 0, _data, j * chunkSize, chunkSize);
            }
        }

        public DataSourceBase<T, ListSlice<T>> Slice(int offset, int count, int axis = -1)
        {
            if (axis < 0)
                axis = _shape.Rank + axis;

            for (var i = axis + 1; i < _shape.Rank; ++i)
                if (_shape[i] != 1)
                    throw new ArgumentException("All axes greater than the argument 'axis' shold be 1");

            int chunkSize = _shape.GetSize(axis - 1);
            var slice = new ListSlice<T>(Data, chunkSize * offset, chunkSize * count);

            var shape = _shape.Clone();
            shape[axis] = count;

            return new DataSourceBase<T, ListSlice<T>>(slice, shape);
        }

        public DataSourceBase<T, T[]>[] Split(params double[] rates)
        {
            var results = new List<DataSourceBase<T, T[]>>();

            int total = _shape[_shape.Rank - 1];
            int chunkSize = _shape.GetSize(_shape.Rank - 2);

            int start = 0;
            foreach (var r in rates)
            {
                var size = (int)System.Math.Round(r >= 1 ? r : total * r);
                if (size > total - start)
                    size = total - start;

                var data = new T[size * chunkSize];
                Copy(_data, start * chunkSize, data, 0, size * chunkSize);

                var shape = _shape.Clone();
                shape.Dimensions[shape.Rank - 1] = size;

                results.Add(new DataSourceBase<T, T[]>(data, shape));

                start += size;
                if (start >= total)
                    break;
            }

            if (start < total)
            {
                var size = total - start;

                var data = new T[size * chunkSize];
                Copy(_data, start * chunkSize, data, 0, size * chunkSize);

                var shape = _shape.Clone();
                shape.Dimensions[shape.Rank - 1] = size;

                results.Add(new DataSourceBase<T, T[]>(data, shape));
            }

            return results.ToArray();
        }

        public DataSourceBase<T, T[]> Transpose(params int[] order)
        {
            if (order.Length != _shape.Rank)
                throw new ArgumentException("Specify the order for all axes");

            var newShape = new Shape(order.Select(x => _shape[x]).ToArray());

            var newData = new T[_shape.TotalSize];

            var reordered = new int[_shape.Rank];
            for (var i = 0; i < newData.Length; ++i)
            {
                var dims = _shape.GetDimensionalIndexes(i);

                for (var j = 0; j < order.Length; ++j)
                    reordered[j] = dims[order[j]];

                var index = newShape.GetSequentialIndex(reordered);

                newData[index] = _data[i];
            }

            return new DataSourceBase<T, T[]>(newData, newShape);
        }

        #endregion

        #region Explicit interface implementation

        Shape IDataSource<T>.Shape => Shape;
        IList<T> IDataSource<T>.Data => Data;

        T IDataSource<T>.this[params int[] indexes] { get => this[indexes]; set { this[indexes] = value; } }

        IDataSource<T> IDataSource<T>.Slice(int offset, int count, int axis)
        {
            return Slice(offset, count, axis);
        }

        IDataSource<T>[] IDataSource<T>.Split(params double[] rates)
        {
            return Split(rates);
        }

        IDataSource<float> IDataSource<T>.ToDataSourceFloat()
        {
            return ToDataSourceFloat();
        }

        IDataSource<T> IDataSource<T>.Transpose(params int[] order)
        {
            return Transpose(order);
        }

        #endregion
    }
}
