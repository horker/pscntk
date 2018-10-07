using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Horker.PSCNTK
{
    [Cmdlet("Write", "CNTKPartitionedCTF")]
    [CmdletBinding(DefaultParameterSetName = "sequences")]
    [OutputType(typeof(void))]
    public class WriteCNTKPartitionedCTF : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "sequences")]
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ratios")]
        public string Path;

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sequences")]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "ratios")]
        public string[] OutFiles;

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "sequences")]
        public int[] SequenceCounts;

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = "ratios")]
        public double[] Ratios;

        private int _totalSeqCount = -1;

        private int GetTotalSeqCount()
        {
            if (_totalSeqCount != -1)
                return _totalSeqCount;

            using (var reader = new StreamReader(Path, Encoding.UTF8))
            {
                _totalSeqCount = CTFTools.CountLines(reader).Item2;
                return _totalSeqCount;
            }
        }

        protected override void BeginProcessing()
        {
            // Convert ratios into sequence counts

            if (ParameterSetName == "ratios")
            {
                SequenceCounts = new int[Ratios.Length];

                var total = GetTotalSeqCount();
                for (var i = 0; i < Ratios.Length; ++i)
                {
                    if (Ratios[i] == -1)
                        continue;

                    SequenceCounts[i] = (int)(Ratios[i] * total);
                }
            }

            // Fix sequence counts

            if (OutFiles.Length == SequenceCounts.Length)
            {
                var total = GetTotalSeqCount();

                for (var i = 0; i < SequenceCounts.Length; ++i)
                {
                    if (SequenceCounts[i] == -1)
                    {
                        SequenceCounts[i] = total - (SequenceCounts.Sum() + 1);
                        break;
                    }
                }
            }
            else if (OutFiles.Length == SequenceCounts.Length + 1)
            {
                var lastCount = GetTotalSeqCount() - SequenceCounts.Sum();
                var newSeqs = new int[SequenceCounts.Length + 1];

                SequenceCounts.CopyTo(newSeqs, 0);
                newSeqs[newSeqs.Length - 1] = lastCount;

                SequenceCounts = newSeqs;
            }
            else
            {
                WriteError(new ErrorRecord(new ArgumentException("The number of SequenceCounts/Ratios should match the number of OutFiles"), "", ErrorCategory.InvalidArgument, null));
                return;
            }

            for (var i = 0; i < SequenceCounts.Length; ++i)
                WriteVerbose(string.Format("{0}-th sequences: {1}", i, SequenceCounts[i]));

            // Get absolute paths

            var current = SessionState.Path.CurrentFileSystemLocation;

            if (!System.IO.Path.IsPathRooted(Path))
                Path = SessionState.Path.Combine(current.ToString(), Path);

            for (var i = 0; i < OutFiles.Length; ++i)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                    OutFiles[i] = SessionState.Path.Combine(current.ToString(), OutFiles[i]);
            }

            // Write partial files

            using (var reader = new StreamReader(Path, Encoding.UTF8))
            {
                string lastLine = null;
                for (var i = 0; i < OutFiles.Length; ++i)
                {
                    using (var writer = new StreamWriter(OutFiles[i], false, Encoding.UTF8))
                    {
                        lastLine = CTFTools.WritePartial(reader, writer, SequenceCounts[i], lastLine);
                    }
                }
            }
        }
    }
}
