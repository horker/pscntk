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
            if (!System.IO.Path.IsPathRooted(Path))
            {
                var current = SessionState.Path.CurrentFileSystemLocation;
                Path = SessionState.Path.Combine(current.ToString(), Path);
            }

            DataSourceSetCTFBuilder.Write(Path, DataSources, WithSequenceAxis);
        }
    }
}
