using System;
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

        public static DataSource<float> ValueToDataSource(CNTK.Value value)
        {
            if (value.IsSparse)
                throw new NotImplementedException("Sparse value is not supported yet");

            if (value.DataType != CNTK.DataType.Float)
                throw new NotImplementedException("Only float value is supported");

            var variable = CNTK.Variable.InputVariable(value.Shape, CNTK.DataType.Float);

            var result = value.GetDenseData<float>(variable);

            return new DataSource<float>(result[0], value.Shape.Dimensions.ToArray());
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

            return new DataSource<float>(result[0], value.Shape.Dimensions.ToArray());
        }

        public static CNTK.Value DataSourceToValue(DataSource<float> ds)
        {
            return ds.ToValue();
        }

        public static CNTK.Value ToValue(object value)
        {
            if (value is PSObject)
                value = (value as PSObject).BaseObject;

            if (value is CNTK.Value)
                return value as CNTK.Value;

            if (value is DataSource<float>)
                return (value as DataSource<float>).ToValue();

            if (value is object[])
            {
                var values = (value as object[]).Select(x => Convert.ToSingle(x)).ToArray();
                return ArrayToValue(values, new int[] { values.Length });
            }

            // single element value
            return new DataSource<float>(new float[] { Convert.ToSingle(value) }, new int[] { 1 }).ToValue();
        }
    }
}