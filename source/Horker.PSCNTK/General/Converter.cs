using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class Converter
    {
        public static string ArrayToString<V>(string className, IList<V> data, Shape shape, bool longFormat)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(className))
            {
                result.Append(className);
                result.Append(" ");
            }

            result.Append(shape.ToString());

            const int MAX_ELEMENT_COUNT = 5;
            if (!longFormat && data.Count > MAX_ELEMENT_COUNT)
            {
                result.Append(" [");
                for (var i = 0; i < MAX_ELEMENT_COUNT - 1; ++i)
                {
                    result.Append(data[i]);
                    result.Append(' ');
                }
                result.Append(data[MAX_ELEMENT_COUNT - 1]);
                result.Append("...]");
                return result.ToString();
            }

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

                result.AppendFormat("{0:0.#####}", data[i]);

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

        public static CNTK.NDArrayView ArrayToNDArrayView(float[] data, int[] dimensions, CNTK.DeviceDescriptor device = null)
        {
            if (device == null)
                device = CNTK.DeviceDescriptor.UseDefaultDevice();

            return new CNTK.NDArrayView(dimensions, data, device);
        }

        public static CNTK.Value ArrayToValue(float[] data, int[] dimensions, CNTK.DeviceDescriptor device = null)
        {
            return new CNTK.Value(ArrayToNDArrayView(data, dimensions, device));
        }

        public static float[] ValueToArray(CNTK.Value value)
        {
            if (value.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (value.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var variable = CNTK.Variable.InputVariable(value.Shape, CNTK.DataType.Float);

            var result = value.GetDenseData<float>(variable);

            return result[0].ToArray();
        }

        public static DataSource<float> ValueToDataSource(CNTK.Value value)
        {
            return new DataSource<float>(ValueToArray(value), value.Shape.Dimensions.ToArray());
        }

        public static DataSource<float> VariableToDataSource(CNTK.Variable variable)
        {
            var array = variable.GetValue();

            if (array.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (array.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var value = new CNTK.Value(array);

            var result = value.GetDenseData<float>(variable);

            return new DataSource<float>(result[0].ToArray(), value.Shape.Dimensions.ToArray());
        }

        public static CNTK.Value DataSourceToValue(DataSource<float> ds)
        {
            return ds.ToValue();
        }

        public static CNTK.Value ToValue(object value, int[] dimensions)
        {
            if (value is PSObject)
                value = (value as PSObject).BaseObject;

            if (value is CNTK.Value)
                return value as CNTK.Value;

            if (value is DataSource<float>)
            {
                var ds = value as DataSource<float>;
                ds.Reshape(dimensions);
                return ds.ToValue();
            }

            if (value is float[])
            {
                var values = value as float[];
                return ArrayToValue(values, dimensions);
            }

            if (value is double[])
            {
                var values = (value as double[]).Select(x => (float)x).ToArray();
                return ArrayToValue(values, dimensions);
            }

            if (value is int[])
            {
                var values = (value as int[]).Select(x => (float)x).ToArray();
                return ArrayToValue(values, dimensions);
            }

            if (value is object[])
            {
                var values = (value as object[]).Select(x => Convert.ToSingle(x is PSObject ? (x as PSObject).BaseObject : x)).ToArray();
                return ArrayToValue(values, dimensions);
            }

            // single element value
            return new DataSource<float>(new float[] { Convert.ToSingle(value is PSObject ? (value as PSObject).BaseObject : value) }, new int[] { 1 }).ToValue();
        }

        public static double ToDouble(object value)
        {
            if (value is PSObject)
                value = (value as PSObject).BaseObject;

            return Convert.ToDouble(value);
        }

        public static float ToFloat(object value)
        {
            if (value is PSObject)
                value = (value as PSObject).BaseObject;

            return Convert.ToSingle(value);
        }

        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable h, Func<object, K> keyConverter, Func<object, V> valueConverter)
        {
            var result = new Dictionary<K, V>();

            foreach (DictionaryEntry entry in h)
            {
                object key = entry.Key;
                if (key is PSObject)
                    key = (key as PSObject).BaseObject;

                object value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;

                result.Add(keyConverter.Invoke(key), valueConverter.Invoke(value));
            }

            return result;
        }

        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable h)
        {
            var keyConverter = new Func<object, K>(x => (K)x);
            var valueConverter = new Func<object, V>(x => (V)x);
            return HashtableToDictionary<K, V>(h, keyConverter, valueConverter);
        }
    }
}