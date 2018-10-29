using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;

namespace Horker.PSCNTK
{
    public class DataSourceFactory
    {
        public static DataSourceBase<T, T[]> Create<T>(T[] data, int[] dimensions = null)
        {
            return new DataSourceBase<T, T[]>(data, dimensions);
        }

        public static DataSourceBase<T, T[]> Copy<T, C>(C data, int[] dimensions = null)
            where C: IList<T>
        {
            var copy = new T[data.Count];
            data.CopyTo(copy, 0);

            return new DataSourceBase<T, T[]>(copy, dimensions);
        }

        public static DataSourceBase<T, T[]> CreateFromSet<T>(IList<T>[] dataSet, int[] dimensions)
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

            return new DataSourceBase<T, T[]>(data, dimensions);
        }

        public static DataSourceBase<T, T[]> FromRows<T>(IList<T[]> rows, int[] dimensions = null)
        {
            int columnCount = rows.Max(x => x.Length);

            var data = new T[rows.Count * columnCount];

            for (var row = 0; row < rows.Count; ++row)
            {
                var r = rows[row];

                for (var column = 0; column < r.Length; ++column)
                    data[column * rows.Count + row] = r[column];

                for (var column = r.Length; column < columnCount; ++column)
                    data[column * rows.Count + row] = default(T);
            }

            if (dimensions == null)
                dimensions = new int[] { rows.Count, columnCount };

            return new DataSourceBase<T, T[]>(data, dimensions);
        }

        public static DataSourceBase<T, T[]> FromColumns<T>(IList<T[]> columns, int[] dimensions = null)
        {
            int rowCount = columns.Max(x => x.Length);

            var data = new T[rowCount * columns.Count];

            for (var column = 0; column < columns.Count; ++column)
            {
                var c = columns[column];

                for (var row = 0; row < c.Length; ++row)
                    data[column * rowCount + row] = c[row];

                for (var row = c.Length; row < rowCount; ++row)
                    data[column * rowCount + row] = default(T);
            }

            if (dimensions == null)
                dimensions = new int[] { rowCount, columns.Count };

            return new DataSourceBase<T, T[]>(data, dimensions);
        }

        public static DataSourceBase<T, T[]> FromPSObjects<T>(IList<PSObject> objects, Func<object, T> converter)
        {
            var obj = objects[0];
            int rowCount = obj.Properties.Count();
            int columnCount = objects.Count;

            var data = new T[rowCount * columnCount];

            int row = 0;
            foreach (var prop in obj.Properties)
            {
                var name = prop.Name;
                for (var column = 0; column < columnCount; ++column)
                {
                    var value = objects[column].Properties[name].Value;
                    if (value is PSObject psobj)
                        value = psobj.BaseObject;
                    data[column * rowCount + row] = converter.Invoke(value);
                }
                ++row;
            }

            return new DataSourceBase<T, T[]>(data, new int[] { rowCount, columnCount });
        }

        public static DataSourceBase<T, T[]> FromDataTable<T>(DataTable table, Func<object, T> converter)
        {
            int rowCount = table.Columns.Count;
            int columnCount = table.Rows.Count;

            var data = new T[rowCount * columnCount];

            int column = 0;
            foreach (DataRow r in table.Rows)
            {
                for (var row = 0; row < rowCount; ++row)
                {
                    var value = r[row];
                    if (value is PSObject psobj)
                        value = psobj.BaseObject;
                    data[column * rowCount + row] = converter.Invoke(value);
                }
                ++column;
            }

            return new DataSourceBase<T, T[]>(data, new int[] { rowCount, columnCount });
        }

        public static DataSourceBase<float, float[]> FromValue(CNTK.Value value)
        {
            return new DataSourceBase<float, float[]>(Converter.ValueToArray(value), value.Shape.Dimensions.ToArray());
        }

        public static DataSourceBase<float, IList<float>> FromVariable(CNTK.Variable variable)
        {
            var array = variable.GetValue();

            if (array.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (array.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var value = new CNTK.Value(array);

            var result = value.GetDenseData<float>(variable);

            return new DataSourceBase<float, IList<float>>(result[0], value.Shape.Dimensions.ToArray());
        }

        public static DataSourceBase<T, T[]> Load<T>(byte[] data, bool decompress = true)
        {
            return Serializer.Deserialize<DataSourceBase<T, T[]>>(data, decompress);
        }

        public static DataSourceBase<T, T[]> Load<T>(string path, bool decompress = true)
        {
            return Serializer.Deserialize<DataSourceBase<T, T[]>>(path, decompress);
        }

        public static DataSourceBase<T, T[]> Combine<T>(IDataSource<T>[] dataSources, int axis)
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

                var chunkCount = d.Data.Count / chunkSize;

                for (var i = 0; i < chunkCount; ++i)
                {
                    var from = i * chunkSize;
                    var to = i * newChunkSize + offset;
                    DataSourceBase<T, T[]>.Copy(d.Data, from, newData, to, chunkSize);
                }
            }

            return new DataSourceBase<T, T[]>(newData, newShape);
        }

        public static JoinedDataSource<T> FromLists<T>(IList<IList<T>> lists, int[] dimensions = null)
        {
            var joined = new JoinedList<T>(lists);
            return new JoinedDataSource<T>(joined, dimensions);
        }
    }
}
