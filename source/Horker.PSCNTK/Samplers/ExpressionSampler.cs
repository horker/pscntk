using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class ExpressionSampler : ISampler
    {
        public string Name;
        public Variable Expression { get; }
        public int MinibatchSize;
        public int IterationsPerEpoch;

        public Variable InputVariable { get; }
        public Value PrevValue;

        public int Iterations;

        public ExpressionSampler(string name, Variable expression, Variable inputVariable = null, int minibatchSize = 1, Value initialValue = null, int iterationsPerEpoch = int.MaxValue)
        {
            Name = name;
            Expression = expression;
            InputVariable = inputVariable;
            MinibatchSize = minibatchSize;

            if (initialValue != null)
                PrevValue = initialValue;
            else
            {
                if (InputVariable != null)
                {
                    var shape = expression.Shape;
                    var initialBuffer = new float[shape.TotalSize * minibatchSize];

                    var dims = new int[shape.Rank + 1];
                    shape.Dimensions.CopyTo(dims, 0);
                    dims[shape.Rank] = minibatchSize;

                    PrevValue = new Value(NDArrayViewMethods.SafeCreate(dims, initialBuffer, DeviceDescriptor.UseDefaultDevice()));
                }
            }

            IterationsPerEpoch = iterationsPerEpoch;

            Iterations = 0;
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Value value;
            if (InputVariable == null)
                value = FunctionInvoke.Invoke(Expression, new Dictionary<Variable, Value>(), device, false)[0];
            else
                value = FunctionInvoke.Invoke(Expression, new Dictionary<Variable, Value>() { { InputVariable, PrevValue } }, device, false)[0];

            int sampleCount = 0;
            int rank = value.Shape.Rank;
            if (rank == 0)
                sampleCount = 1;
            else
                sampleCount = value.Shape[rank - 1];

            ++Iterations;
            var sweepEnd = (Iterations + 1) % IterationsPerEpoch == 0;

            var data = new MinibatchData(value, (uint)sampleCount, sweepEnd);
            var minibatch = new Minibatch();
            minibatch.Add(Name, data);

            PrevValue = value;

            return minibatch;
        }
    }
}
