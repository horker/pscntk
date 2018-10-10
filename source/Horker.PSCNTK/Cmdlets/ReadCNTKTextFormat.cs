using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Read", "CNTKTextFormat")]
    public class ReadCNTKTextFormat : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = false)]
        public int ReadCount = int.MaxValue;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter AsEnumerator = false;

        protected override void BeginProcessing()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                var current = SessionState.Path.CurrentFileSystemLocation;
                Path = SessionState.Path.Combine(current.ToString(), Path);
            }

            if (AsEnumerator)
            {
                var reader = new StreamReader(Path, Encoding.UTF8);
                var e = CTFTools.GetSampleReader(reader).GetEnumerator();
                var psobj = new PSObject(e);
                psobj.Properties.Add(new PSNoteProperty("Reader", reader));

                WriteObject(psobj);
            }
            else
            {
                using (var reader = new StreamReader(Path, Encoding.UTF8))
                {
                    var e = CTFTools.GetSampleReader(reader);

                    foreach (var sample in e)
                    {
                        WriteObject(sample);
                        if (sample.SequenceCount == ReadCount)
                            break;
                    }
                }
            }
        }
    }
}
