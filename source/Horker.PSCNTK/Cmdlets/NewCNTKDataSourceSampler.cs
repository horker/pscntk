using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDataSourceSampler")]
    [Alias("cntk.sampler", "cntk.dataSourceSampler")]
    public class NewCNTKDataSourceSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public DataSourceSet DataSourceSet;

        [Parameter(Position = 1, Mandatory = false)]
        public int MinibatchSize = 32;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter Randomize = false;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter WithSequenceAxis = false;

        protected override void EndProcessing()
        {
            var sampler = new DataSourceSampler(DataSourceSet, MinibatchSize, Randomize, WithSequenceAxis);
            WriteObject(sampler);
        }
    }
}
