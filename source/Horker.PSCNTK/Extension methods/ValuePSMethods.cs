using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;

namespace Horker.PSCNTK
{
    public class ValuePSMethods
    {
        public static string AsString(PSObject value, bool longFormat = true)
        {
            var v = value.BaseObject as CNTK.Value;
            var ds = DataSourceFactory.FromValue(v);

            return Converter.ArrayToString<float>("CNTK.Value", ds.Data, ds.Shape, longFormat);
        }

        public static IDataSource<float> ToDataSource(PSObject value)
        {
            var v = value.BaseObject as CNTK.Value;

            return DataSourceFactory.FromValue(v);
        }

        public static float[] ToArray(PSObject value)
        {
            var v = value.BaseObject as CNTK.Value;
            return Converter.ValueToArray(v);
        }
    }
}
