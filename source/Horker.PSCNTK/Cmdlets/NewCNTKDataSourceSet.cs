using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDataSourceSet")]
    [CmdletBinding(DefaultParameterSetName = "new")]
    [Alias("cntk.dataSourceSet")]
    public class NewCNTKDataSourceSet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "new")]
        public Hashtable DataSources;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "load")]
        public string Path;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "load")]
        public SwitchParameter NoDecompress;

        protected override void EndProcessing()
        {
            if (ParameterSetName == "load")
            {
                Path = IO.GetAbsolutePath(this, Path);
                var dss = DataSourceSet.Load(Path, !NoDecompress);
                WriteObject(dss);
            }
            else
            {
                var dss = new DataSourceSet(DataSources);
                WriteObject(dss);
            }
        }
    }
}
