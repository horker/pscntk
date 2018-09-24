using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;
using CNTK;

namespace Horker.PSCNTK
{
    public class NDArrayViewPSMethods
    {
        public static string AsString(PSObject value, bool longFormat = true)
        {
            var a = value.BaseObject as NDArrayView;
            var ds = DataSource<float>.FromValue(new Value(a));

            return Converter.ArrayToString<float>("CNTK.NDArrayView", ds.Data, ds.Shape, longFormat);
        }
    }
}
