using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKCTFMinibatchDefinition")]
    [Alias("cntk.ctfminibatchdef")]
    public class NewCNTKCTFMinibatchDefinition : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = false)]
        public int MinibatchSize = 32;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter NoRandomize;

        protected override void EndProcessing()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                var current = SessionState.Path.CurrentFileSystemLocation;
                Path = SessionState.Path.Combine(current.ToString(), Path);
            }

            var result = new CTFMinibatchDefinition(Path, MinibatchSize, !NoRandomize);
            WriteObject(result);
        }
    }
}
