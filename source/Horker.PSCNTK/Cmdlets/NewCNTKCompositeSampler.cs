using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKCompositeSampler")]
    [Alias("cntk.compositesampler")]
    public class NewCNTKCompositeSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ISampler[] Samplers;

        protected override void EndProcessing()
        {
            var sampler = new CompositeSampler(Samplers);
            WriteObject(sampler);
        }
    }
}
