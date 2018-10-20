using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using Horker.PSCNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("ConvertTo", "CNTKDataSource")]
    [Alias("cntk.toDataSource")]
    public class ConvertToCNTKDataSource : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        public PSObject InputObject;

        [Parameter(Position = 1, Mandatory = false)]
        public int[] Dimensions = null;

        [Parameter(Position = 2, Mandatory = false)]
        public DataSourceType DataType = DataSourceType.Float;

        private List<PSObject> _data;

        protected override void BeginProcessing()
        {
            _data = new List<PSObject>();
        }

        protected override void ProcessRecord()
        {
            _data.Add(InputObject);
        }

        protected override void EndProcessing()
        {
            switch (DataType)
            {
                case DataSourceType.Double:
                    ProcessInternal(x => Converter.ToDouble(x));
                    break;
                case DataSourceType.Float:
                    ProcessInternal(x => Converter.ToFloat(x));
                    break;
                case DataSourceType.Int64:
                    ProcessInternal(x => Convert.ToInt64(x));
                    break;
                case DataSourceType.Int32:
                    ProcessInternal(x => Convert.ToInt32(x));
                    break;
                case DataSourceType.Int16:
                    ProcessInternal(x => Convert.ToInt16(x));
                    break;
                case DataSourceType.Byte:
                    ProcessInternal(x => Convert.ToByte(x));
                    break;
                case DataSourceType.SByte:
                    ProcessInternal(x => Convert.ToSByte(x));
                    break;
            }
        }

        private void ProcessInternal<T>(Func<object, T> converter)
        {
            var ds = DataSourceFactory.FromPSObjects(_data, converter);

            if (Dimensions != null)
                ds.Reshape(Dimensions);

            WriteObject(ds);
        }
    }
}
