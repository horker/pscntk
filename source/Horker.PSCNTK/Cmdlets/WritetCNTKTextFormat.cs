using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Write", "CNTKTextFormat")]
    public class WritetCNTKTextFormat : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Hashtable DataSources;

        [Parameter(Position = 1, Mandatory = true)]
        public string Path;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter WithSequenceAxis;

        protected override void EndProcessing()
        {
            Path = IO.GetAbsolutePath(this, Path);
            DataSourceSetCTFBuilder.Write(Path, DataSources, WithSequenceAxis);
        }
    }
}
