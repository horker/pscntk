using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKExpressionSampler")]
    [Alias("cntk.expressionsampler")]
    public class NewCNTKExpressionSampler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedFunction Expression;

        [Parameter(Position = 2, Mandatory = false)]
        public object InputVariable = null;

        [Parameter(Position = 3, Mandatory = false)]
        public object InitialValue = null;

        [Parameter(Position = 4, Mandatory = false)]
        public int IterationsPerEpoch = int.MaxValue;

        protected override void EndProcessing()
        {
            Variable input;

            if (InputVariable == null)
                input = null;
            else if (InputVariable is Variable)
                input = InputVariable as Variable;
            else if (InputVariable is string)
            {
                input = FunctionFind.FindVariable(Expression, InputVariable as string);
                if (input == null)
                    throw new ArgumentException("Input variable not found");
            }
            else
                throw new ArgumentException("Input variable should be a CNTK.Variable or a string");

            Value value = null;
            if (InitialValue != null)
                value = Converter.ToValue(InitialValue, input.Shape.Dimensions.ToArray());

            var sampler = new ExpressionSampler(Name, Expression, input, value, IterationsPerEpoch);

            WriteObject(sampler);
        }
    }
}
