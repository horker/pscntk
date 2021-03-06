﻿using System;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKParallelSampler")]
    [Alias("cntk.parallelSampler")]
    public class NewCNTKParallelSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public int SampleCountPerEpoch = 3200;

        [Parameter(Position = 1, Mandatory = false)]
        public int QueueSize = 1000;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter ReuseSamples;

        [Parameter(Position = 3, Mandatory = false)]
        public int BufferSize = 1000;

        [Parameter(Position = 4, Mandatory = false)]
        public int TimeoutForAdd = 15 * 1000;

        [Parameter(Position = 5, Mandatory = false)]
        public int TimeoutForTake = 15 * 1000;

        protected override void EndProcessing()
        {
            var sampler = new ParallelSampler(SampleCountPerEpoch, QueueSize, ReuseSamples, BufferSize, TimeoutForAdd, TimeoutForTake);
            WriteObject(sampler);
        }
    }
}
