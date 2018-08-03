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

        protected override void EndProcessing()
        {
            if (ParameterSetName == "load")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    var current = SessionState.Path.CurrentFileSystemLocation;
                    Path = SessionState.Path.Combine(current.ToString(), Path);
                }

                var result = DataSource<float>.Load(Path, !NoDecompress);
                WriteObject(result);
            }
            else if (ParameterSetName == "rows")
            {
                var data = new List<float[]>();
                foreach (var row in Rows)
                {
                    var r = row.Select(x => {
                        if (x is PSObject)
                            x = (x as PSObject).BaseObject;

                        return Convert.ToSingle(x);
                    });
                    data.Add(r.ToArray());
                }

                var result = DataSource<float>.FromRows(data.ToArray(), Dimensions);
                WriteObject(result);
            }
            else if (ParameterSetName == "columns")
            {
                var data = new List<float[]>();
                foreach (var column in Columns)
                {
                    var c = column.Select(x => {
                        if (x is PSObject)
                            x = (x as PSObject).BaseObject;

                        return Convert.ToSingle(x);
                    });
                    data.Add(c.ToArray());
                }

                var result = DataSource<float>.FromColumns(data.ToArray(), Dimensions);
                WriteObject(result);
            }
            else
            {
                // new
                var result = new DataSource<float>(Data.Select(x => Converter.ToFloat(x)).ToArray(), Dimensions);
                WriteObject(result);
            }
        }
    }

    [Cmdlet("New", "CNTKDataSourceFromRows")]
    [Alias("cntk.fromrows")]
    public class NewCNTKDataSourceFromRows : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public object[][] Rows;

        [Parameter(Position = 1, Mandatory = false)]
        public int[] Dimensions = null;

        protected override void EndProcessing()
        {
            var data = new List<float[]>();
            foreach (var row in Rows)
            {
                var r = row.Select(x => {
                    if (x is PSObject)
                        x = (x as PSObject).BaseObject;

                    return Convert.ToSingle(x);
                });
                data.Add(r.ToArray());
            }

            var result = DataSource<float>.FromRows(data.ToArray(), Dimensions);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKDataSourceFromColumns")]
    [Alias("cntk.fromcolumns")]
    public class NewCNTKDataSourceFromColumns : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public object[][] Columns;

        [Parameter(Position = 1, Mandatory = false)]
        public int[] Dimensions = null;

        protected override void EndProcessing()
        {
            var data = new List<float[]>();
            foreach (var column in Columns)
            {
                var c = column.Select(x => {
                    if (x is PSObject)
                        x = (x as PSObject).BaseObject;

                    return Convert.ToSingle(x);
                });
                data.Add(c.ToArray());
            }

            var result = DataSource<float>.FromColumns(data.ToArray(), Dimensions);
            WriteObject(result);
        }
    }
}
