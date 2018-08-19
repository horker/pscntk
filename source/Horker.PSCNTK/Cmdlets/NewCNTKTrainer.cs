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

        [Parameter(Position = 1, Mandatory = true)]
        public Variable Label;

        [Parameter(Position = 2, Mandatory = false)]
        public string LossFunctionName;

        [Parameter(Position = 3, Mandatory = false)]
        public string ErrorFunctionName;

        [Parameter(Position = 4, Mandatory = true)]
        public Learner[] Learners;

        private MethodInfo FindMethod(string name)
        {
            var methods = typeof(CNTKLib).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods)
            {
                if (m.Name == name)
                {
                    var p = m.GetParameters();
                    if (p.Length == 2 && p[0].ParameterType == typeof(Variable) && p[1].ParameterType == typeof(Variable))
                    {
                        return m;
                    }
                }
            }

            throw new ArgumentException("LossFunctionName doesn't indicate the proper CNTK function name");
        }

        protected override void EndProcessing()
        {
            Function loss = null, error = null;

            if (!string.IsNullOrEmpty(LossFunctionName))
            {
                var lossMethod = FindMethod(LossFunctionName);
                loss = (Function)lossMethod.Invoke(null, new object[] { Model, Label });
            }

            if (!string.IsNullOrEmpty(ErrorFunctionName))
            {
                var errorMethod = FindMethod(ErrorFunctionName);
                error = (Function)errorMethod.Invoke(null, new object[] { Model, Label });
            }

            var trainer = Trainer.CreateTrainer(Model, loss, error, Learners.ToList());
            WriteObject(trainer);
        }
    }
}
