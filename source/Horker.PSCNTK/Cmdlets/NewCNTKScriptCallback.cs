using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKScriptCallback")]
    [Alias("cntk.scriptCallback")]
    public class NewCNTKScriptCallback : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Func<TrainingSession, bool> Func;

        [Parameter(Position = 1, Mandatory = false)]
        public int Step = 1;

        protected override void BeginProcessing()
        {
            var callback = new ScriptCallback(Func, Step);
            WriteObject(callback);
        }
    }
}
