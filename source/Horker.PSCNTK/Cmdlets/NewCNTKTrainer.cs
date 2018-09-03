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

        private Function GetFunctionInstance(object func, string displayName)
        {
            if (func == null)
                return null;

            if (func is PSObject)
                func = (func as PSObject).BaseObject;

            if (func is Function)
            {
                return func as Function;
            }
            else if (func is string)
            {
                var f = func as string;
                if (string.IsNullOrEmpty(f))
                    return null;

                var lossMethod = Helpers.GetCNTKLibMethod(f, 2);
                return (Function)lossMethod.Invoke(null, new object[] { Model, Label });
            }

            throw new ArgumentException(displayName + " should be an instance of Function or a function name");
        }

        protected override void EndProcessing()
        {
            Function loss = GetFunctionInstance(LossFunction, "LossFunction");
            Function error = GetFunctionInstance(ErrorFunction, "ErrorFunction");

            var trainer = Trainer.CreateTrainer(Model, loss, error, Learners.ToList());
            WriteObject(trainer);
        }
    }
}
