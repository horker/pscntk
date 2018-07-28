using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKMinibatchDefinition")]
    [Alias("CNTK.minibatchdef")]
    public class NewCNTKMinibatchDefinition : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Hashtable DataSources;

        [Parameter(Position = 1, Mandatory = false)]
        public int MinibatchSize = 32;

        [Parameter(Position = 2, Mandatory = false)]
        public double ValidationRate = 0.0;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter NoRandomize = false;

        [Parameter(Position = 5, Mandatory = false)]
        public DeviceDescriptor Device = null;

        protected override void EndProcessing()
        {
            var ds = new Dictionary<string, DataSource<float>>();
            foreach (DictionaryEntry entry in DataSources)
            {
                var value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;

                ds.Add(entry.Key.ToString(), (DataSource<float>)value);
            }

            var minibatchDef = new MinibatchDefinition(ds, MinibatchSize, ValidationRate, !NoRandomize, Device);

            WriteObject(minibatchDef);
        }
    }
}
