using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Open", "CNTKTextFormatBuilder")]
    public class OpentCNTKCTextFormatBuilder : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = false)]
        public int InitialSequence = 0;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter ManualIncrement;

        protected override void EndProcessing()
        {
            Path = IO.GetAbsolutePath(this, Path);
            var writer = new StreamWriter(Path, false, new UTF8Encoding(false));
            var builder = new CTFBuilder(writer, InitialSequence, !ManualIncrement);

            WriteObject(builder);
        }
    }

    [Cmdlet("Close", "CNTKTextFormatBuilder")]
    public class CloseCNTKCTextFormatBuilder : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CTFBuilder CTFBuilder;

        protected override void EndProcessing()
        {
            try
            {
                CTFBuilder.Finish();
            }
            finally
            {
                if (CTFBuilder.Writer != null)
                {
                    CTFBuilder.Writer.Close();
                    CTFBuilder.Writer.Dispose();
                }
            }
        }
    }
}
