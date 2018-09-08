using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Horker.PSCNTK
{
    [Serializable]
    public class DataSource<T>
    {
        public Shape Shape;
        public T[] Data;

        #region Constructors and factories

        public DataSource(T[] data, int[] dimensions = null, bool ensureCopy = false)
        {
            if (dimensions == null || dimensions.Length == 0)
                dimensions = new int[] { data.Length };

            Shape = new Shape(dimensions, data.Length);

            if (ensureCopy)
            {
                var buffer = new T[data.Length];
                int i = 0;
                foreach (var value in data)
                {
                    buffer[i] = value;
                    ++i;
                }
                Data = buffer;
            }
            else
            {
                Data = data;
            }
        }

        public static DataSource<T> Create(IReadOnlyList<T>[] dataSet, int[] dimensions)
        {
            var count = dataSet.Sum(x => x.Count);
            var data = new T[count];

            int index = 0;
            foreach (var d in dataSet)
            {
                for (var i = 0; i < d.Count; ++i)
                {
                    data[index] = d[i];
                    ++index;
                }
            }

            return new DataSource<T>(data, dimensions);
        }

        public static DataSource<T> FromRows(IReadOnlyList<T>[] rows, int[] dimensions = null)
        {
            int columnCount = rows.Max(x => x.Count);

            var data = new T[rows.Length * columnCount];

            for (var row = 0; row < rows.Length; ++row)
            {
                var r = rows[row];

                for (var column = 0; column < r.Count; ++column)
                    data[column * rows.Length + row] = r[column];

                for (var column = r.Count; column < columnCount; ++column)
                    data[column * rows.Length + row] = default(T);
            }

            if (dimensions == null)
                dimensions = new int[] { rows.Length, columnCount };

            return new DataSource<T>(data, dimensions);
        }

        public static DataSource<T> FromColumns(IReadOnlyList<T>[] columns, int[] dimensions = null)
        {
            int rowCount = columns.Max(x => x.Count);

            var data = new T[rowCount * columns.Length];

            for (var column = 0; column < columns.Length; ++column)
            {
                var c = columns[column];

                for (var row = 0; row < c.Count; ++row)
                    data[column * rowCount + row] = c[row];

                for (var row = c.Count; row < rowCount; ++row)
                    data[column * rowCount + row] = default(T);
            }

            if (dimensions == null)
                dimensions = new int[] { rowCount, columns.Length };

            return new DataSource<T>(data, dimensions);
        }

        public static DataSource<float> FromValue(CNTK.Value value)
        {
            return Converter.ValueToDataSource(value);
        }

        public static DataSource<float> FromVariable(CNTK.Variable variable)
        {
            return Converter.VariableToDataSource(variable);
        }

        public static DataSource<T> Load(byte[] data, bool decompress = true)
        {
            return Serializer.Deserialize<DataSource<T>>(data, decompress);
        }

        public static DataSource<T> Load(string path, bool decompress = true)
        {
            return Serializer.Deserialize<DataSource<T>>(path, decompress);
        }

        #endregion

        #region Accessors

        public T this[params int[] indexes]
        {
            get => Data[Shape.GetSequentialIndex(indexes)];
            set { Data[Shape.GetSequentialIndex(indexes)] = value; }
        }

        public CNTK.StreamConfiguration GetStreamConfiguration(string name, string alias = "")
        {
            if (Shape.Rank < 3)
                throw new NotSupportedException("Shape should contain sequence and batch axes as the last two");

            return new CNTK.StreamConfiguration(name, Shape.GetSize(Shape.Rank - 3), false, alias, false);
        }

        #endregion

        #region Converters

        public override string ToString()
        {
            return Converter.ArrayToString("DataSource", this.Data, this.Shape, false);
        }

        public string AsString()
        {
            return Converter.ArrayToString("DataSource", this.Data, this.Shape, true);
        }

        public T[] ToArray()
        {
            return Data.ToArray();
        }

        public static implicit operator T[] (DataSource<T> source)
        {
            return source.Data.ToArray();
        }

        public CNTK.NDArrayView ToNDArrayView(CNTK.DeviceDescriptor device = null)
        {
            if (device == null)
                device = CNTK.DeviceDescriptor.UseDefaultDevice();

            return Converter.ArrayToNDArrayView(Data.Select(x => Convert.ToSingle(x)).ToArray(), Shape.Dimensions, device);
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
            if (Shape.Rank < 3)
                throw new ArgumentException("To make a minibatch data, sequence and batch axis is necessary");

            return new CNTK.MinibatchData(ToValue(), (uint)Shape[-1], (uint)(Shape[-1] * Shape[-2]), sweepEnd);
        }

        public DataSource<float> ToDataSourceFloat()
        {
            var data = Data.Select(x => Convert.ToSingle(x)).ToArray();
            return new DataSource<float>(data, Shape);
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
            Shape.Reshape(dimensions, Data.Length);
        }

        public void ApplyInPlace(Func<int, T, T> func)
        {
            for (var i = 0; i < Data.Length; ++i)
                Data[i] = func.Invoke(i, Data[i]);
        }

        private static void Copy(IList<T> from, int fromOffset, IList<T> to, int toOffset, int size)
        {
            for (var i = 0; i < size; ++i)
            {
                to[toOffset + i] = from[fromOffset + i];
            }
        }

        public static DataSource<T> Combine(DataSource<T>[] dataSources, int axis)
        {
            // Validate arguments

            for (var i = 1; i < dataSources.Length; ++i)
            {
                if (dataSources[0].Shape.Rank != dataSources[i].Shape.Rank)
                {
                    throw new ArgumentException("rank of data sources should be equal");
                }
            }

            int rank = dataSources[0].Shape.Rank;
            for (var i = 0; i < rank; ++i)
            {
                if (i == axis)
                {
                    continue;
                }
                for (var j = 1; j < dataSources.Length; ++j)
                {
                    if (dataSources[0].Shape.Dimensions[i] != dataSources[j].Shape.Dimensions[i])
                    {
                        throw new ArgumentException(String.Format("dimensions of rank {0} are not equal in all data sources", i));
                    }
                }
            }

            // Obtain the shape of resultant data

            var dim = 0;
            foreach (var d in dataSources)
            {
                dim += d.Shape.Dimensions[axis];
            }

            var newShape = (Shape)(dataSources[0].Shape.Dimensions);
            newShape.Dimensions[axis] = dim;

            // Copy data to the new shape

            var newData = new T[newShape.TotalSize];

            var offset = 0;
            var chunkSize = 0;
            foreach (var d in dataSources)
            {
                offset += chunkSize;

                chunkSize = d.Shape.GetSize(axis);

                var size = 1;
                if (axis > 0)
                    size = d.Shape.GetSize(axis - 1);

                var newChunkSize = size * newShape.Dimensions[axis];

                var chunkCount = d.Data.Length / chunkSize;

                for (var i = 0; i < chunkCount; ++i)
                {
                    var from = i * chunkSize;
                    var to = i * newChunkSize + offset;
                    Copy(d.Data, from, newData, to, chunkSize);
                }
            }

            return new DataSource<T>(newData, newShape);
        }

        public DataSource<T> GetSubsequences(int subseqLength, int step = 1, int sequenceAxis = -1)
        {
            if (sequenceAxis == -1)
                sequenceAxis = Shape.Rank - 2;

            var seqDim = Shape.Dimensions[sequenceAxis];
            var sampleAxis = Shape.Rank - 1;
            var sampleDim = Shape.Dimensions[sampleAxis];

            var repeatLength = (seqDim - subseqLength) / step + 1;
            if (repeatLength < 1)
                throw new ArgumentException("Sequence too short");

            var valueSize = Shape.GetSize(sequenceAxis - 1);
            var sampleSize = Shape.GetSize(sequenceAxis);

            var newSampleSize = valueSize * subseqLength * repeatLength;

            var newDims = Shape.Dimensions.Clone() as int[];
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
                            Data,
                            sampleCount * sampleSize + (seqCount * step + offset) * valueSize,
                            newData,
                            sampleCount * newSampleSize + (seqCount * subseqLength + offset) * valueSize,
                            valueSize
                        );
                    }
                }
            }

            return new DataSource<T>(newData, newDims);
        }

        public void Shuffle(int? seed = null)
        {
            int count = Shape[Shape.Rank - 1];
            int chunkSize = Shape.GetSize(Shape.Rank - 2);

            System.Random random;
            if (seed.HasValue)
                random = Random.GetInstance(seed.Value);
            else
                random = Random.GetInstance();

            var temp = new T[chunkSize];
            for (var i = 0; i < count; ++i)
            {
                var j = random.Next(count);
                Copy(Data, i * chunkSize, temp, 0, chunkSize);
                Copy(Data, j * chunkSize, Data, i * chunkSize, chunkSize);
                Copy(temp, 0, Data, j * chunkSize, chunkSize);
            }
        }

        public DataSource<T> Slice(int from, int to)
        {
            if (to - from <= 0)
                throw new ArgumentException("Range must not be zero or negative");

            int chunkSize = Shape.GetSize(Shape.Rank - 2);

            var data = new T[chunkSize * (to - from)];
            Array.Copy(Data, chunkSize * from, data, 0, data.Length);

            var shape = Shape.Clone();
            shape[-1] = to - from;

            return new DataSource<T>(data, shape);
        }

        public DataSource<T>[] Split(params double[] rates)
        {
            var results = new List<DataSource<T>>();

            int total = Shape[Shape.Rank - 1];
            int chunkSize = Shape.GetSize(Shape.Rank - 2);

            int start = 0;
            foreach (var r in rates)
            {
                var size = (int)System.Math.Round(r >= 1 ? r : total * r);
                if (size > total - start)
                    size = total - start;

                var data = new T[size * chunkSize];
                Copy(Data, start * chunkSize, data, 0, size * chunkSize);

                var shape = Shape.Clone();
                shape.Dimensions[shape.Rank - 1] = size;

                results.Add(new DataSource<T>(data, shape));

                start += size;
                if (start >= total)
                    break;
            }

            if (start < total)
            {
                var size = total - start;

                var data = new T[size * chunkSize];
                Copy(Data, start * chunkSize, data, 0, size * chunkSize);

                var shape = Shape.Clone();
                shape.Dimensions[shape.Rank - 1] = size;

                results.Add(new DataSource<T>(data, shape));
            }

            return results.ToArray();
        }

        public DataSource<T> Transpose(params int[] order)
        {
            if (order.Length != Shape.Rank)
                throw new ArgumentException("Specify the order for all axes");

            var newShape = new Shape(order.Select(x => Shape[x]).ToArray());

            var newData = new T[Shape.TotalSize];

            var reordered = new int[Shape.Rank];
            for (var i = 0; i < newData.Length; ++i)
            {
                var dims = Shape.GetDimensionalIndexes(i);

                for (var j = 0; j < order.Length; ++j)
                    reordered[j] = dims[order[j]];

                var index = newShape.GetSequentialIndex(reordered);

                newData[index] = Data[i];
            }

            return new DataSource<T>(newData, newShape);
        }

        #endregion
    }
}
