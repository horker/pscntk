﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Horker.PSCNTK
{
    [Cmdlet("Split", "CNTKMsgPack")]
    [CmdletBinding(DefaultParameterSetName = "counts")]
    [OutputType(typeof(void))]
    public class SplitCNTKMsgPack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "counts")]
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ratios")]
        public string Path;

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "counts")]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "ratios")]
        public string[] OutFiles;

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "counts")]
        public int[] SampleCounts;

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "ratios")]
        public double[] Ratios;

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "counts")]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "ratios")]
        public int SplitCount;

        protected override void BeginProcessing()
        {
            Path = IO.GetAbsolutePath(this, Path);
            var total = MsgPackTools.GetTotalSampleCount(Path);

            // Convert ratios into sample counts

            if (ParameterSetName == "ratios")
            {
                SampleCounts = new int[Ratios.Length];

                for (var i = 0; i < Ratios.Length; ++i)
                {
                    if (Ratios[i] == -1)
                        continue;

                    SampleCounts[i] = (int)(Ratios[i] * total);
                }
            }

            // Fix sample counts

            if (OutFiles.Length == SampleCounts.Length)
            {
                for (var i = 0; i < SampleCounts.Length; ++i)
                {
                    if (SampleCounts[i] == -1)
                    {
                        SampleCounts[i] = total - (SampleCounts.Sum() + 1);
                        break;
                    }
                }
            }
            else if (OutFiles.Length == SampleCounts.Length + 1)
            {
                var lastCount = total - SampleCounts.Sum();
                var newSamples = new int[SampleCounts.Length + 1];

                SampleCounts.CopyTo(newSamples, 0);
                newSamples[newSamples.Length - 1] = lastCount;

                SampleCounts = newSamples;
            }
            else
            {
                WriteError(new ErrorRecord(new ArgumentException("The number of SampleCounts/Ratios should match the number of OutFiles"), "", ErrorCategory.InvalidArgument, null));
                return;
            }

            for (var i = 0; i < SampleCounts.Length; ++i)
                WriteVerbose(string.Format("{0}-th sample count: {1}", i, SampleCounts[i]));

            // Get absolute paths

            for (var i = 0; i < OutFiles.Length; ++i)
                OutFiles[i] = IO.GetAbsolutePath(this, OutFiles[i]);

            // Write partial files

            var dssEnum = MsgPackTools.ReadDataSourceSet(Path, total, SplitCount).GetEnumerator();
            for (var i = 0; i < OutFiles.Length; ++i)
            {
                using (var outStream = new FileStream(OutFiles[i], FileMode.Create, FileAccess.Write))
                {
                    var count = 0;
                    while (count < SampleCounts[i])
                    {
                        if (!dssEnum.MoveNext())
                            break;

                        var dss = dssEnum.Current;
                        count += dss.SampleCount;

                        MsgPackSerializer.Serialize(dss, outStream);
                    }
                }
            }
        }
    }
}
