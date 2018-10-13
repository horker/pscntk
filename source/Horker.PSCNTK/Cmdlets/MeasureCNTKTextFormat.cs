using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Horker.PSCNTK
{
    public class CTFLineInfo
    {
        public int Lines;
        public int Sequences;
    }

    [Cmdlet("Measure", "CNTKTextFormat")]
    [OutputType(typeof(CTFLineInfo))]
    public class MeasureCNTKTextFormat : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        protected override void BeginProcessing()
        {
            Path = IO.GetAbsolutePath(this, Path);

            // Count sequeneces

            Tuple<int, int> lines;
            using (var reader = new StreamReader(Path, Encoding.UTF8))
            {
                lines = CTFTools.CountLines(reader);
            }

            WriteObject(new CTFLineInfo() { Lines = lines.Item1, Sequences = lines.Item2 });
        }
    }
}
