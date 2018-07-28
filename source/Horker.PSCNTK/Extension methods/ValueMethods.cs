using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;

namespace Horker.PSCNTK
{
    public class ValueMethods
    {
        public static string AsString(PSObject value, bool longFormat = true)
        {
            var v = value.BaseObject as CNTK.Value;
            var ds = DataSource<float>.FromValue(v);

            return Converter.ArrayToString<float>("CNTK.Value", ds.Data, ds.Shape, longFormat);
        }

        public static DataSource<float> ToDataSource(PSObject value)
        {
            var v = value.BaseObject as CNTK.Value;

            return DataSource<float>.FromValue(v);
        }

        public static float[] ToArray(PSObject value)
        {
            var v = value.BaseObject as CNTK.Value;
            return Converter.ValueToArray(v);
        }
    }
}
