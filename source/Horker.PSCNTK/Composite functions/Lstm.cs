using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

// This file is copied from the original CNTK source code and modified to fit the pscntk project.
// The original code is written by Microsoft and published under the MIT License.
//
//   Location: https://github.com/Microsoft/CNTK/blob/master/Examples/TrainingCSharp/Common/LSTMSequenceClassifier.cs

namespace Horker.PSCNTK.Microsoft
{
    /// <summary>
    /// This class shows how to build a recurrent neural network model from ground up and train the model. 
    /// </summary>
    public class LSTMSequenceClassifierNet
    {
        public static Function Stabilize<ElementType>(Variable x, DeviceDescriptor device, string name = "")
        {
            try
            {
                NodeGroup.EnterNewGroup(name);

                bool isFloatType = typeof(ElementType).Equals(typeof(float));
                Constant f, fInv;
                if (isFloatType)
                {
                    f = Constant.Scalar(4.0f, device);
                    fInv = Constant.Scalar(f.DataType, 1.0 / 4.0f);
                }
                else
                {
                    f = Constant.Scalar(4.0, device);
                    fInv = Constant.Scalar(f.DataType, 1.0 / 4.0f);
                }
                Composite.Register(f);
                Composite.Register(fInv);

                // var beta = CNTKLib.ElementTimes(
                //     fInv,
                //     CNTKLib.Log(
                //         Constant.Scalar(f.DataType, 1.0) +
                //         CNTKLib.Exp(CNTKLib.ElementTimes(f, new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device, name + "_w")))));
                // return CNTKLib.ElementTimes(beta, x, name);

                var weight = new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device, name + "_w");
                Composite.Register(weight);
                var one = Constant.Scalar(f.DataType, 1.0);
                Composite.Register(one);

                var output = CNTKLib.ElementTimes(f, weight);
                Composite.Register(output);
                output = CNTKLib.Exp(output);
                Composite.Register(output);
                output = CNTKLib.Plus(one, output);
                Composite.Register(output);
                output = CNTKLib.Log(output);
                Composite.Register(output);
                var beta = CNTKLib.ElementTimes(fInv, output);
                Composite.Register(beta);
                output = CNTKLib.ElementTimes(beta, x, name);
                Composite.Register(output);

                return output;
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }
        }

        static Tuple<Function, Function> LSTMPCellWithSelfStabilization<ElementType>(
            Variable input, Variable prevOutput, Variable prevCellState, DeviceDescriptor device, string baseName)
        {
            int outputDim = prevOutput.Shape[0];
            int cellDim = prevCellState.Shape[0];

            bool isFloatType = typeof(ElementType).Equals(typeof(float));
            DataType dataType = isFloatType ? DataType.Float : DataType.Double;

            Func<int, string, Parameter> createBiasParam;
            if (isFloatType)
                createBiasParam = (dim, name) => {
                    var param = new Parameter(new int[] { dim }, 0.01f, device, name + "_b");
                    Composite.Register(param);
                    return param;
                };
            else
                createBiasParam = (dim, name) => {
                    var param = new Parameter(new int[] { dim }, 0.01, device, name + "_b");
                    Composite.Register(param);
                    return param;
                };

            Func<int, string, Parameter> createProjectionParam = (oDim, name) => {
                var param = new Parameter(new int[] { oDim, NDShape.InferredDimension },
                        dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0), device, name + "_w");
                Composite.Register(param);
                return param;
            };

            Func<int, string, Parameter> createDiagWeightParam = (dim, name) => {
                var param = new Parameter(new int[] { dim }, dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0), device, name + "_diagw");
                Composite.Register(param);
                return param;
            };

            Function stabilizedPrevOutput = Stabilize<ElementType>(prevOutput, device, baseName + "_prevOutput");
            Function stabilizedPrevCellState = Stabilize<ElementType>(prevCellState, device, baseName + "_prevCellState");

            Func<string, Variable> projectInput = (name) => {
                var param = createProjectionParam(cellDim, name) * input;
                Composite.Register(param);
                param = createBiasParam(cellDim, name) + param;
                Composite.Register(param);
                return param;
            };

            // Input gate
            Function it;
            try
            {
                var name = baseName + "_it";
                NodeGroup.EnterNewGroup(name);

                it = createProjectionParam(cellDim, name) * stabilizedPrevOutput;
                Composite.Register(it);
                it = projectInput(name) + it;
                Composite.Register(it);
                var it2 = CNTKLib.ElementTimes(createDiagWeightParam(cellDim, name), stabilizedPrevCellState);
                Composite.Register(it2);
                it = (Variable)it + it2;
                Composite.Register(it);
                it = CNTKLib.Sigmoid(it);
                Composite.Register(it);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            Function bit;
            try
            {
                var name = baseName + "_bit";
                NodeGroup.EnterNewGroup(name);

                bit = createProjectionParam(cellDim, name) * stabilizedPrevOutput;
                Composite.Register(bit);
                bit = projectInput(name) + bit;
                Composite.Register(bit);
                bit = CNTKLib.Tanh(bit);
                Composite.Register(bit);
                bit = CNTKLib.ElementTimes(it, bit);
                Composite.Register(bit);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            // Forget-me-not gate
            Function ft;
            try
            {
                var name = baseName + "_ft";
                NodeGroup.EnterNewGroup(name);

                ft = createProjectionParam(cellDim, name) * stabilizedPrevOutput;
                Composite.Register(ft);
                ft = projectInput(name) + ft;
                Composite.Register(ft);
                var ft2 = CNTKLib.ElementTimes(createDiagWeightParam(cellDim, name), stabilizedPrevCellState);
                Composite.Register(ft2);
                ft = (Variable)ft + ft2;
                Composite.Register(ft);
                ft = CNTKLib.Sigmoid(ft);
                Composite.Register(ft);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            Function bft;
            try
            {
                var name = baseName + "_bft";
                NodeGroup.EnterNewGroup(name);

                bft = CNTKLib.ElementTimes(ft, prevCellState, name);
                Composite.Register(bft);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            Function ct;
            try
            {
                var name = baseName + "_ct";
                NodeGroup.EnterNewGroup(name);

                ct = (Variable)bft + bit;
                Composite.Register(ct);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            // Output gate
            Function ot;
            try
            {
                var name = baseName + "_ot";
                NodeGroup.EnterNewGroup(name);

                ot = createProjectionParam(cellDim, name) * stabilizedPrevOutput;
                Composite.Register(ot);
                ot = projectInput(name) + ot;
                Composite.Register(ot);
                var ot2 = CNTKLib.ElementTimes(createDiagWeightParam(cellDim, name), Stabilize<ElementType>(ct, device, name));
                Composite.Register(ot2);
                ot = (Variable)ot + ot2;
                Composite.Register(ot);
                ot = CNTKLib.Sigmoid(ot);
                Composite.Register(ot);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            Function ht;
            try
            {
                var name = baseName + "_ht";
                NodeGroup.EnterNewGroup(name);

                ht = CNTKLib.Tanh(ct);
                Composite.Register(ht);
                ht = CNTKLib.ElementTimes(ot, ht);
                Composite.Register(ht);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            Function h;
            try
            {
                var name = baseName + "_h";
                NodeGroup.EnterNewGroup(name);

                h = (outputDim != cellDim) ? (createProjectionParam(outputDim, name) * Stabilize<ElementType>(ht, device, name + "_stab")) : ht;
                Composite.Register(h);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            return new Tuple<Function, Function>(h, ct);
        }

        static Tuple<Function, Function> LSTMPComponentWithSelfStabilization<ElementType>(Variable input,
            NDShape outputShape, NDShape cellShape,
            Func<Variable, Function> recurrenceHookH,
            Func<Variable, Function> recurrenceHookC,
            DeviceDescriptor device,
            string baseName)
        {
            var dh = Variable.PlaceholderVariable(outputShape, input.DynamicAxes);
            var dc = Variable.PlaceholderVariable(cellShape, input.DynamicAxes);

            var LSTMCell = LSTMPCellWithSelfStabilization<ElementType>(input, dh, dc, device, baseName);
            var actualDh = recurrenceHookH(LSTMCell.Item1);
            Composite.Register(actualDh);
            Composite.Register(actualDh.Inputs[1]);
            var actualDc = recurrenceHookC(LSTMCell.Item2);
            Composite.Register(actualDc);
            Composite.Register(actualDc.Inputs[1]);

            // Form the recurrence loop by replacing the dh and dc placeholders with the actualDh and actualDc
            (LSTMCell.Item1).ReplacePlaceholders(new Dictionary<Variable, Variable> { { dh, actualDh }, { dc, actualDc } });

            return new Tuple<Function, Function>(LSTMCell.Item1, LSTMCell.Item2);
        }

        private static Function Embedding(Variable input, int embeddingDim, DeviceDescriptor device, string baseName)
        {
            try
            {
                var name = baseName + "_embed";
                NodeGroup.EnterNewGroup(name);

                int inputDim = input.Shape[0];
                var embeddingParameters = new Parameter(new int[] { embeddingDim, inputDim }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device, name + "_w");
                Composite.Register(embeddingParameters);

                var output = CNTKLib.Times(embeddingParameters, input);
                Composite.Register(output);

                return output;
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }
        }

        /// <summary>
        /// Build a one direction recurrent neural network (RNN) with long-short-term-memory (LSTM) cells.
        /// http://colah.github.io/posts/2015-08-Understanding-LSTMs/
        /// </summary>
        /// <param name="input">the input variable</param>
        /// <param name="LSTMDim">LSTM output dimension</param>
        /// <param name="cellDim">cell dimension</param>
        /// <param name="returnSequences">whether to return the last output in the output sequence, or the full sequence</param>
        /// <param name="device">CPU or GPU device to run the model</param>
        /// <param name="outputName">name of the model output</param>
        /// <returns>the RNN model</returns>
        public static Function Create(Variable input, int LSTMDim, int cellDim, bool returnSequences, DeviceDescriptor device,
            string outputName)
        {
            try
            {
                NodeGroup.EnterNewGroup(outputName);

                Func<Variable, Function> pastValueRecurrenceHook = (x) => CNTKLib.PastValue(x); 
                Function LSTMFunction = LSTMPComponentWithSelfStabilization<float>(
                    input,
                    new int[] { LSTMDim },
                    new int[] { cellDim },
                    pastValueRecurrenceHook,
                    pastValueRecurrenceHook,
                    device,
                    outputName).Item1;
                Composite.Register(LSTMFunction);

                if (!returnSequences)
                {
                    var f = CNTKLib.SequenceLast(LSTMFunction);
                    f.RootFunction.SetName(outputName);
                    return f;
                }

                LSTMFunction.RootFunction.SetName(outputName);

                return LSTMFunction;
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }
        }
    }
}