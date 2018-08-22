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

            var beta = CNTKLib.ElementTimes(
                fInv,
                CNTKLib.Log(
                    Constant.Scalar(f.DataType, 1.0) +
                    CNTKLib.Exp(CNTKLib.ElementTimes(f, new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device, name + "_w")))));
            return CNTKLib.ElementTimes(beta, x, name);
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
                createBiasParam = (dim, name) => new Parameter(new int[] { dim }, 0.01f, device, name + "_b");
            else
                createBiasParam = (dim, name) => new Parameter(new int[] { dim }, 0.01, device, name + "_b");

            Func<int, string, Parameter> createProjectionParam = (oDim, name) => new Parameter(new int[] { oDim, NDShape.InferredDimension },
                    dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0), device, name + "_w");

            Func<int, string, Parameter> createDiagWeightParam = (dim, name) =>
                new Parameter(new int[] { dim }, dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0), device, name + "_diagw");

            Function stabilizedPrevOutput = Stabilize<ElementType>(prevOutput, device, baseName + "_stab1");
            Function stabilizedPrevCellState = Stabilize<ElementType>(prevCellState, device, baseName + "_stab2");

            Func<string, Variable> projectInput = (name) =>
                createBiasParam(cellDim, name) + (createProjectionParam(cellDim, name) * input);

            // Input gate
            var n = baseName + "_it";
            Function it =
                CNTKLib.Sigmoid(
                    (Variable)(projectInput(n) + (createProjectionParam(cellDim, n) * stabilizedPrevOutput)) +
                    CNTKLib.ElementTimes(createDiagWeightParam(cellDim, n), stabilizedPrevCellState));

            n = baseName + "_bit";
            Function bit = CNTKLib.ElementTimes(
                it,
                CNTKLib.Tanh(projectInput(n) + (createProjectionParam(cellDim, n) * stabilizedPrevOutput)));

            // Forget-me-not gate
            n = baseName + "_ft";
            Function ft = CNTKLib.Sigmoid(
                (Variable)(
                        projectInput(n) + (createProjectionParam(cellDim, n) * stabilizedPrevOutput)) +
                        CNTKLib.ElementTimes(createDiagWeightParam(cellDim, n), stabilizedPrevCellState));
            Function bft = CNTKLib.ElementTimes(ft, prevCellState);

            Function ct = (Variable)bft + bit;

            // Output gate
            n = baseName + "_ot";
            Function ot = CNTKLib.Sigmoid(
                (Variable)(projectInput(n) + (createProjectionParam(cellDim, n) * stabilizedPrevOutput)) +
                CNTKLib.ElementTimes(createDiagWeightParam(cellDim, n), Stabilize<ElementType>(ct, device, n)));
            Function ht = CNTKLib.ElementTimes(ot, CNTKLib.Tanh(ct));

            n = baseName + "_h";
            Function c = ct;
            Function h = (outputDim != cellDim) ? (createProjectionParam(outputDim, n) * Stabilize<ElementType>(ht, device, n)) : ht;

            return new Tuple<Function, Function>(h, c);
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
            var actualDc = recurrenceHookC(LSTMCell.Item2);

            // Form the recurrence loop by replacing the dh and dc placeholders with the actualDh and actualDc
            (LSTMCell.Item1).ReplacePlaceholders(new Dictionary<Variable, Variable> { { dh, actualDh }, { dc, actualDc } });

            return new Tuple<Function, Function>(LSTMCell.Item1, LSTMCell.Item2);
        }

        private static Function Embedding(Variable input, int embeddingDim, DeviceDescriptor device, string baseName)
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];
            var embeddingParameters = new Parameter(new int[] { embeddingDim, inputDim }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device, baseName + "_embed_w");
            return CNTKLib.Times(embeddingParameters, input);
        }

        /// <summary>
        /// Build a one direction recurrent neural network (RNN) with long-short-term-memory (LSTM) cells.
        /// http://colah.github.io/posts/2015-08-Understanding-LSTMs/
        /// </summary>
        /// <param name="input">the input variable</param>
        /// <param name="embeddingDim">dimension of the embedding layer</param>
        /// <param name="LSTMDim">LSTM output dimension</param>
        /// <param name="cellDim">cell dimension</param>
        /// <param name="device">CPU or GPU device to run the model</param>
        /// <param name="outputName">name of the model output</param>
        /// <returns>the RNN model</returns>
        public static Function Create(Variable input, int embeddingDim, int LSTMDim, int cellDim, DeviceDescriptor device,
            string outputName)
        {
            Function embeddingFunction = Embedding(input, embeddingDim, device, outputName);
            Func<Variable, Function> pastValueRecurrenceHook = (x) => CNTKLib.PastValue(x);
            Function LSTMFunction = LSTMPComponentWithSelfStabilization<float>(
                embeddingFunction,
                new int[] { LSTMDim },
                new int[] { cellDim },
                pastValueRecurrenceHook,
                pastValueRecurrenceHook,
                device,
                outputName).Item1;
            Function thoughtVectorFunction = CNTKLib.SequenceLast(LSTMFunction);

            thoughtVectorFunction.RootFunction.SetName(outputName);

            return thoughtVectorFunction;
        }
    }
}