using System;
using System.Collections.Generic;
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
        public int[] Dimensions = null;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "load")]
        public string Path;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "load")]
        public SwitchParameter NoDecompress;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "rows")]
        public object[][] Rows;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "columns")]
        public object[][] Columns;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "new")]
        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "rows")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "columns")]
        public Type DataType = typeof(Single);

        protected override void EndProcessing()
        {
            if (DataType == typeof(Double)) ProcessInternal(x => Converter.ToDouble(x));
            else if (DataType == typeof(Single)) ProcessInternal(x => Converter.ToFloat(x));
            else if (DataType == typeof(Int64)) ProcessInternal(x => Convert.ToInt64(x));
            else if (DataType == typeof(Int32)) ProcessInternal(x => Convert.ToInt32(x));
            else if (DataType == typeof(Int16)) ProcessInternal(x => Convert.ToInt16(x));
            else if (DataType == typeof(Byte)) ProcessInternal(x => Convert.ToByte(x));
            else if (DataType == typeof(SByte)) ProcessInternal(x => Convert.ToSByte(x));
        }

        private void ProcessInternal<T>(Func<object, T> converter)
        {
            if (ParameterSetName == "load")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    var current = SessionState.Path.CurrentFileSystemLocation;
                    Path = SessionState.Path.Combine(current.ToString(), Path);
                }

                var result = DataSource<T>.Load(Path, !NoDecompress);
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

                var result = DataSource<T>.FromRows(data.ToArray(), Dimensions);
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

                var result = DataSource<T>.FromColumns(data.ToArray(), Dimensions);
                WriteObject(result);
            }
            else
            {
                // new
                var result = new DataSource<T>(Data.Select(x => converter.Invoke(x)).ToArray(), Dimensions);
                WriteObject(result);
            }
        }
    }
}
