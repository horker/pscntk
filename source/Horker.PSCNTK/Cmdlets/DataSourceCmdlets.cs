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
    [Alias("ds")]
    public class NewCNTKDataSource : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public object[] Data;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] Dimensions;

        protected override void EndProcessing()
        {
            var result = new DataSource<float>(Data.Select(x => Convert.ToSingle(x)).ToArray(), Dimensions);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKDataSourceFromRows")]
    [Alias("ds.fromrows")]
    public class NewCNTKDataSourceFromRows : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public object[][] Rows;

        protected override void EndProcessing()
        {
            var data = new List<float[]>();
            foreach (var row in Rows)
            {
                data.Add(row.Select(x => Convert.ToSingle(x)).ToArray());
            }

            var result = DataSource<float>.FromRows(data.ToArray());
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKDataSourceFromColumns")]
    [Alias("ds.fromcolumns")]
    public class NewCNTKDataSourceFromColumns : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public object[][] Columns;

        protected override void EndProcessing()
        {
            var data = new List<float[]>();
            foreach (var column in Columns)
            {
                data.Add(column.Select(x => Convert.ToSingle(x)).ToArray());
            }

            var result = DataSource<float>.FromColumns(data.ToArray());
            WriteObject(result);
        }
    }
}
