using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKNoiseSampler")]
    [Alias("cntk.noisesampler")]
    public class NewCNTKNoiseSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 2, Mandatory = false)]
        public int MinibatchSize = 32;

        [Parameter(Position = 3, Mandatory = false)]
        public double Min = -1.0;

        [Parameter(Position = 4, Mandatory = false)]
        public double Max = 1.0;

        protected override void EndProcessing()
        {
            var sampler = new NoiseSampler(Name, Shape, MinibatchSize, Min, Max);
            WriteObject(sampler);
        }
    }
}
