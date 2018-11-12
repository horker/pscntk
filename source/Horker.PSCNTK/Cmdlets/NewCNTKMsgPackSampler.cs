using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKMsgPackSampler")]
    [Alias("cntk.msgPackSampler")]
    public class NewCNTKMsgPackSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = true)]
        public int MinibatchSize;

        [Parameter(Position = 2, Mandatory = false)]
        public int SampleCountPerEpoch = -1;

        [Parameter(Position = 3, Mandatory = false)]
        public int QueueSize = 100;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter Randomize = false;

        [Parameter(Position = 5, Mandatory = false)]
        public SwitchParameter ReuseSamples = false;

        [Parameter(Position = 6, Mandatory = false)]
        public int BufferSize = 100;

        [Parameter(Position = 7, Mandatory = false)]
        public int TimeoutForAdd = 60 * 60 * 1000;

        [Parameter(Position = 8, Mandatory = false)]
        public int TimeoutForTake = 60 * 60 * 1000;

        protected override void EndProcessing()
        {
            Path = IO.GetAbsolutePath(this, Path);

            var sampler = new MsgPackSampler(Path, MinibatchSize, Randomize, SampleCountPerEpoch, QueueSize, ReuseSamples, BufferSize, TimeoutForAdd, TimeoutForTake);

            WriteObject(sampler);
        }
    }
}
