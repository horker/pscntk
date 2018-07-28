using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Horker.PSCNTK
{
    public class DataSource<T>
    {
        public Shape Shape;
        public T[] Data;

        #region Constructors and factories

        public DataSource(T[] data, int[] dimensions, bool ensureCopy = false)
        {
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

        #endregion

        #region Accessors

        public T this[params int[] indexes]
        {
            get => Data[Shape.GetSequentialIndex(indexes)];
            set { Data[Shape.GetSequentialIndex(indexes)] = value; }
        }

        public CNTK.StreamConfiguration GetStreamConfiguration(string name)
        {
            if (Shape.Rank < 3)
                throw new NotSupportedException("Shape should contain sequence and batch axes as the last two");

            return new CNTK.StreamConfiguration(name, Shape.GetSize(Shape.Rank - 3), false, name, false);
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

        #endregion

        #region Manipulators

        public void Reshape(params int[] dimensions)
        {
            Shape.Reshape(dimensions, Data.Length);
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

        public static DataSource<T> GetSubsequences(DataSource<T> dataSource, int subseqLength, int sequenceAxis = -1)
        {
            var seqAxis = sequenceAxis;
            if (sequenceAxis == -1)
            {
                seqAxis = dataSource.Shape.Rank - 2;
            }

            var seqDim = dataSource.Shape.Dimensions[seqAxis];
            var sampleAxis = dataSource.Shape.Rank - 1;
            var sampleDim = dataSource.Shape.Dimensions[sampleAxis];

            var valueSize = dataSource.Shape.GetSize(seqAxis - 1);
            var sampleSize = dataSource.Shape.GetSize(seqAxis);

            var repeatLength = seqDim - subseqLength + 1;
            var newSampleSize = valueSize * subseqLength * repeatLength;

            var newDims = dataSource.Shape.Dimensions.Clone() as int[];
            newDims[sampleAxis] = sampleDim * repeatLength;
            newDims[seqAxis] = subseqLength;

            var newData = new T[new Shape(newDims).TotalSize];

            for (var sampleCount = 0; sampleCount < sampleDim; ++sampleCount)
            {
                for (var seqCount = 0; seqCount < repeatLength; ++seqCount)
                {
                    for (var offset = 0; offset < subseqLength; ++offset)
                    {
                        Copy(
                            dataSource.Data,
                            sampleCount * sampleSize + (seqCount + offset) * valueSize,
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

            Random random;
            if (seed.HasValue)
                random = new Random(seed.Value);
            else
                random = new Random();

            var temp = new T[chunkSize];
            for (var i = 0; i < count; ++i)
            {
                var j = random.Next(count);
                Copy(Data, i * chunkSize, temp, 0, chunkSize);
                Copy(Data, j * chunkSize, Data, i * chunkSize, chunkSize);
                Copy(temp, 0, Data, j * chunkSize, chunkSize);
            }
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

        #endregion
    }
}
