using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDataSourceSet")]
    [CmdletBinding(DefaultParameterSetName = "new")]
    [Alias("cntk.datasourceset")]
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
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    var current = SessionState.Path.CurrentFileSystemLocation;
                    Path = SessionState.Path.Combine(current.ToString(), Path);
                }

                var dss = DataSourceSet.Load(Path, !NoDecompress);
                WriteObject(dss);
            }
            else
            {
                var dss = new DataSourceSet();
                foreach (DictionaryEntry entry in DataSources)
                {
                    var value = entry.Value;
                    if (value is PSObject)
                        value = (value as PSObject).BaseObject;

                    dss.Add(entry.Key.ToString(), (DataSource<float>)value);
                }

                WriteObject(dss);
            }
        }
    }
}
