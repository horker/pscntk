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
        public Function Expression { get; }
        public int MinibatchSize;
        public int IterationsPerEpoch;

        public Variable InputVariable { get; }
        public Value PrevValue;

        public int Iterations;

        private float[] _initialBuffer;

        public ExpressionSampler(string name, Function expression, Variable inputVariable = null, Value initialValue = null, int iterationsPerEpoch = int.MaxValue)
        {
            Name = name;
            Expression = expression;
            InputVariable = inputVariable;

            if (initialValue != null)
                PrevValue = initialValue;
            else
            {
                if (InputVariable != null)
                {
                    var shape = expression.Output.Shape;
                    _initialBuffer = new float[shape.TotalSize]; 
                    PrevValue = new Value(new NDArrayView(shape, _initialBuffer, DeviceDescriptor.UseDefaultDevice(), true));
                }
            }

            IterationsPerEpoch = iterationsPerEpoch;

            Iterations = 0;
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Value value;
            if (InputVariable == null)
                value = FunctionInvoke.Invoke(Expression, new Dictionary<Variable, Value>(), device, false);
            else
                value = FunctionInvoke.Invoke(Expression, new Dictionary<Variable, Value>() { { InputVariable, PrevValue } }, device, false);

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

        public Minibatch GetValidationBatch(DeviceDescriptor device = null)
        {
            return null;
        }
    }
}
