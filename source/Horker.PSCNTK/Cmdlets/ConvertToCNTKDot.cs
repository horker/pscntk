using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("ConvertTo", "CNTKDot")]
    [Alias("cntk.todot")]
    public class ConvertToCNTKDot : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Function Function;

        protected override void EndProcessing()
        {
            var g = new DotGenerator(Function);
            WriteObject(g.Result);
        }
    }
}
