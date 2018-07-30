using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKMinibatchDefinition")]
    [CmdletBinding(DefaultParameterSetName = "new")]
    [Alias("cntk.minibatchdef")]
    public class NewCNTKMinibatchDefinition : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "new")]
        public Hashtable DataSources;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "new")]
        public int MinibatchSize = 32;

        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "new")]
        public double ValidationRate = 0.0;

        [Parameter(Position = 4, Mandatory = false, ParameterSetName = "new")]
        public SwitchParameter NoRandomize = false;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "load")]
        public string Path;

        protected override void EndProcessing()
        {
            if (ParameterSetName == "load")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    var current = SessionState.Path.CurrentFileSystemLocation;
                    Path = SessionState.Path.Combine(current.ToString(), Path);
                }

                var result = MinibatchDefinition.Load(Path);
                WriteObject(result);
            }
            else
            {
                var ds = new Dictionary<string, DataSource<float>>();
                foreach (DictionaryEntry entry in DataSources)
                {
                    var value = entry.Value;
                    if (value is PSObject)
                        value = (value as PSObject).BaseObject;

                    ds.Add(entry.Key.ToString(), (DataSource<float>)value);
                }

                var minibatchDef = new MinibatchDefinition(ds, MinibatchSize, ValidationRate, !NoRandomize);

                WriteObject(minibatchDef);
            }
        }
    }
}
