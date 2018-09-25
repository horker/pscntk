using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKOnMemorySampler")]
    [CmdletBinding(DefaultParameterSetName = "new")]
    [Alias("cntk.sampler")]
    public class NewCNTKOneMemorySampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "new")]
        public DataSourceSet DataSources;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "new")]
        public int MinibatchSize = 32;

        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "new")]
        public double ValidationRate = 0.0;

        [Parameter(Position = 3, Mandatory = false, ParameterSetName = "new")]
        public SwitchParameter NoRandomize = false;

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

                var result = OnMemorySampler.Load(Path, !NoDecompress);
                WriteObject(result);
            }
            else
            {
                var sampler = new OnMemorySampler(DataSources, MinibatchSize, ValidationRate, !NoRandomize);

                WriteObject(sampler);
            }
        }
    }
}
