using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Horker.PSCNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDataSource")]
    [CmdletBinding(DefaultParameterSetName = "new")]
    [Alias("cntk.datasource")]
    public class NewCNTKDataSource : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "new")]
        public object[] Data;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "new")]
        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "rows")]
        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "columns")]
        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "psobjects")]
        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "datatable")]
        public int[] Dimensions = null;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "load")]
        public string Path;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "load")]
        public SwitchParameter NoDecompress;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "rows")]
        public object[][] Rows;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "columns")]
        public object[][] Columns;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "psobjects")]
        public PSObject[] PSObjects;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "datatable")]
        public DataTable DataTable;

        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "new")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "rows")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "columns")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "psobjects")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "datatable")]
        public DataSourceType DataType = DataSourceType.Float;

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
            if (ParameterSetName == "load")
            {
                Path = IO.GetAbsolutePath(this, Path);
                var result = DataSourceFactory.Load<float>(Path, !NoDecompress);
                WriteObject(result);
            }
            else if (ParameterSetName == "rows")
            {
                var data = new List<T[]>();
                foreach (var row in Rows)
                {
                    var r = row.Select(x => {
                        if (x is PSObject)
                            x = (x as PSObject).BaseObject;

                        return converter.Invoke(x);
                    });
                    data.Add(r.ToArray());
                }

                var result = DataSourceFactory.FromRows<T>(data, Dimensions);
                WriteObject(result);
            }
            else if (ParameterSetName == "columns")
            {
                var data = new List<T[]>();
                foreach (var column in Columns)
                {
                    var c = column.Select(x => {
                        if (x is PSObject)
                            x = (x as PSObject).BaseObject;

                        return converter.Invoke(x);
                    });
                    data.Add(c.ToArray());
                }

                var result = DataSourceFactory.FromColumns(data, Dimensions);
                WriteObject(result);
            }
            else if (ParameterSetName == "psobjects")
            {
                var result = DataSourceFactory.FromPSObjects(PSObjects, converter);

                if (Dimensions != null)
                    result.Reshape(Dimensions);

                WriteObject(result);
            }
            else if (ParameterSetName == "datatable")
            {
                var result = DataSourceFactory.FromDataTable(DataTable, converter);

                if (Dimensions != null)
                    result.Reshape(Dimensions);

                WriteObject(result);
            }
            else
            {
                // new
                var result = DataSourceFactory.Create(Data.Select(x => converter.Invoke(x)).ToArray(), Dimensions);
                WriteObject(result);
            }
        }
    }
}
