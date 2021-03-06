﻿using System.Management.Automation;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKBackgroundScriptRunner")]
    [Alias("cntk.backgroundScriptRunner")]
    public class NewCNTKBackgroundScriptRunner : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var runner = new BackgroundScriptRunner();
            WriteObject(runner);
        }
    }
}
