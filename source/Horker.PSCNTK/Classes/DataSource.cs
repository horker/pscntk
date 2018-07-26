using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Horker.PSCNTK
{
    public class DataSource<T>
    {
        public Shape Shape;
        public IList<T> Data;

        public DataSource(IList<T> data, int[] dimensions, bool ensureCopy = false)
        {
            Shape = new Shape(dimensions, data.Count);

            if (ensureCopy)
            {
                var buffer = new T[data.Count];
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

        public static DataSource<T> FromRows(IReadOnlyList<T>[] rows)
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

            return new DataSource<T>(data, new int[] { rows.Length, columnCount });
        }

        public static DataSource<T> FromColumns(IReadOnlyList<T>[] columns)
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

            return new DataSource<T>(data, new int[] { rowCount, columns.Length });
        }

        public static DataSource<float> FromValue(CNTK.Value value)
        {
            if (value.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (value.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var variable = CNTK.Variable.InputVariable(value.Shape, CNTK.DataType.Float);
            var result = value.GetDenseData<float>(variable);

            return new DataSource<float>(result[0], value.Shape.Dimensions.ToArray());
        }

        public static DataSource<float> FromVariable(CNTK.Variable variable)
        {
            var array = variable.GetValue();

            if (array.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (array.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var value = new CNTK.Value(array);
            var result = value.GetDenseData<float>(variable);

            return new DataSource<float>(result[0], value.Shape.Dimensions.ToArray());
        }

        public T this[params int[] indexes]
        {
            get => Data[Shape.GetSequentialIndex(indexes)];
            set { Data[Shape.GetSequentialIndex(indexes)] = value; }
        }

        public static implicit operator T[] (DataSource<T> source)
        {
            return source.Data.ToArray();
        }

        public void Reshape(params int[] dimensions)
        {
            Shape.Reshape(dimensions, Data.Count);
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

                var newChunkSize = d.Shape.GetSize(axis - 1) * newShape.Dimensions[axis];

                var chunkCount = d.Data.Count / chunkSize;

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

        public static string ConvertToString<V>(string className, IList<V> data, Shape shape, bool longFormat)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(className))
            {
                result.Append(className);
                result.Append(" ");
            }

            result.Append(shape.ToString());

            if (longFormat)
                result.AppendLine();

            var sizes = new int[shape.Rank];
            sizes[0] = shape[0];
            for (var i = 1; i < shape.Rank; ++i)
                sizes[i] = sizes[i - 1] * shape[i];

            for (var i = 0; i < data.Count; ++i)
            {
                bool open = false;
                for (var j = 0; j < shape.Rank; ++j)
                {
                    if (i % sizes[j] == 0)
                    {
                        result.Append(" [");
                        open = true;
                    }
                }

                if (!open)
                    result.Append(" ");

                result.Append(data[i]);

                var close = false;
                var newline = false;
                for (var j = 0; j < shape.Rank; ++j)
                {
                    if ((i + 1) % sizes[j] == 0)
                    {
                        if (close)
                            result.Append(" ");
                        result.Append("]");
                        close = true;

                        if (j == shape.Rank - 2)
                            newline = true;
                    }
                }

                if (i < data.Count - 1 && longFormat && newline)
                {
                    result.AppendLine();
                    result.Append("  ");
                }
            }

            return result.ToString();
        }

        public override string ToString()
        {
            return ConvertToString("DataSource", this.Data, this.Shape, false);
        }

        public string AsString()
        {
            return ConvertToString("DataSource", this.Data, this.Shape, true);
        }

        public T[] ToArray()
        {
            return Data.ToArray();
        }

        public CNTK.NDArrayView ToNDArrayView(CNTK.DeviceDescriptor device = null)
        {
            if (device == null)
                device = CNTK.DeviceDescriptor.UseDefaultDevice();

            return new CNTK.NDArrayView(Shape.Dimensions, Data.Select(x => Convert.ToSingle(x)).ToArray(), device);
        }

        public CNTK.Value ToValue(CNTK.DeviceDescriptor device = null)
        {
            return new CNTK.Value(ToNDArrayView(device));
        }
    }
}
