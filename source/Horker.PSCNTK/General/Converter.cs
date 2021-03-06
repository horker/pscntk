﻿using System;
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

            return NDArrayViewMethods.SafeCreate(dimensions, data, device);
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

        public static IDataSource<float> ValueToDataSource(CNTK.Value value)
        {
            return DataSourceFactory.FromValue(value);
        }

        public static IDataSource<float> VariableToDataSource(CNTK.Variable variable)
        {
            return DataSourceFactory.FromVariable(variable);
        }

        public static CNTK.Value DataSourceToValue(IDataSource<float> ds)
        {
            return ds.ToValue();
        }

        public static CNTK.NDArrayView ToNDArrayView(object value, int[] dimensions = null, CNTK.DeviceDescriptor device = null)
        {
            if (value is PSObject psobj)
                value = psobj.BaseObject;

            if (value is CNTK.NDArrayView a)
                return a;

            if (value is CNTK.Value v)
                return v.Data;

            if (value is IDataSource<float> ds)
            {
                if (dimensions != null)
                    ds.Reshape(dimensions);
                return ds.ToNDArrayView(device);
            }

            if (value is float[] floatValues)
            {
                return ArrayToNDArrayView(floatValues, dimensions, device);
            }

            if (value is double[] doubleValues)
            {
                var values = doubleValues.Select(x => (float)x).ToArray();
                if (dimensions == null)
                    dimensions = new int[] { values.Length };
                return ArrayToNDArrayView(values, dimensions, device);
            }

            if (value is int[] intValues)
            {
                var values = intValues.Select(x => (float)x).ToArray();
                if (dimensions == null)
                    dimensions = new int[] { values.Length };
                return ArrayToNDArrayView(values, dimensions, device);
            }

            if (value is object[] objs)
            {
                var values = objs.Select(x => Convert.ToSingle(x is PSObject ? (x as PSObject).BaseObject : x)).ToArray();
                if (dimensions == null)
                    dimensions = new int[] { values.Length };
                return ArrayToNDArrayView(values, dimensions, device);
            }

            // Single value

            if (dimensions != null && (dimensions.Length != 1 || dimensions[0] != 1))
                throw new ArgumentException("Dimensions exepct [1] is invalid because Value is a single value");

            return new DataSourceBase<float, float[]>(new float[] { Convert.ToSingle(value is PSObject pso ? pso.BaseObject : value) }, new int[] { 1 }).ToNDArrayView(device);
        }

        public static CNTK.Value ToValue(object value, int[] dimensions = null, CNTK.DeviceDescriptor device = null)
        {
            if (value is PSObject psobj)
                value = psobj.BaseObject;

            if (value is CNTK.Value v)
                return v;

            var a = ToNDArrayView(value, dimensions, device);
            return new CNTK.Value(a);
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
                if (key is PSObject pskey)
                    key = pskey.BaseObject;

                object value = entry.Value;
                if (value is PSObject psvalue)
                    value = psvalue.BaseObject;

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