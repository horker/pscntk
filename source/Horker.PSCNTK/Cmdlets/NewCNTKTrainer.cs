using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKTrainer")]
    [Alias("cntk.trainer")]
    public class NewCNTKTrainer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Model;

        [Parameter(Position = 1, Mandatory = false)]
        public Variable Label;

        [Parameter(Position = 2, Mandatory = false)]
        public object LossFunction;

        [Parameter(Position = 3, Mandatory = false)]
        public object ErrorFunction;

        [Parameter(Position = 4, Mandatory = true)]
        public Learner[] Learners;

        protected override void EndProcessing()
        {
            Function loss = CmdletHelpers.GetFunctionInstance(LossFunction, new object[] { Model, Label }, "LossFunction");
            Function error = CmdletHelpers.GetFunctionInstance(ErrorFunction, new object[] { Model, Label }, "ErrorFunction");

            var trainer = Trainer.CreateTrainer(Model, loss, error, Learners.ToList());
            WriteObject(trainer);
        }
    }
}
