using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace cntktest
{
    public enum Activation
    {
        None,
        ReLU,
        Sigmoid,
        Tanh
    }

    class TestHelper
    {
        public static Function Dense(Variable input, int outputDim, DeviceDescriptor device,
            Activation activation = Activation.None, string outputName = "")
        {
            if (input.Shape.Rank != 1)
            {
                // 
                int newDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                input = CNTKLib.Reshape(input, new int[] { newDim });
            }

            Function fullyConnected = FullyConnectedLinearLayer(input, outputDim, device, outputName);
            switch (activation)
            {
                default:
                case Activation.None:
                    return fullyConnected;
                case Activation.ReLU:
                    return CNTKLib.ReLU(fullyConnected);
                case Activation.Sigmoid:
                    return CNTKLib.Sigmoid(fullyConnected);
                case Activation.Tanh:
                    return CNTKLib.Tanh(fullyConnected);
            }
        }

        public static Function FullyConnectedLinearLayer(Variable input, int outputDim, DeviceDescriptor device,
            string outputName = "")
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];

            int[] s = { outputDim, inputDim };
            var timesParam = new Parameter((NDShape)s, DataType.Float,
                CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1),
                device, "timesParam");
            var timesFunction = CNTKLib.Times(timesParam, input, "times");

            int[] s2 = { outputDim };
            var plusParam = new Parameter(s2, 0.0f, device, "plusParam");
            return CNTKLib.Plus(plusParam, timesFunction, outputName);
        }

        public static float ValidateModelWithMinibatchSource(
            string modelFile, MinibatchSource testMinibatchSource,
            int[] imageDim, int numClasses, string featureInputName, string labelInputName, string outputName,
            DeviceDescriptor device, int maxCount = 1000)
        {
            Function model = Function.Load(modelFile, device);
            var imageInput = model.Arguments[0];
            var labelOutput = model.Outputs.Single(o => o.Name == outputName);

            var featureStreamInfo = testMinibatchSource.StreamInfo(featureInputName);
            var labelStreamInfo = testMinibatchSource.StreamInfo(labelInputName);

            int batchSize = 50;
            int miscountTotal = 0, totalCount = 0;
            while (true)
            {
                var minibatchData = testMinibatchSource.GetNextMinibatch((uint)batchSize, device);
                if (minibatchData == null || minibatchData.Count == 0)
                    break;
                totalCount += (int)minibatchData[featureStreamInfo].numberOfSamples;

                // expected labels are in the minibatch data.
                var labelData = minibatchData[labelStreamInfo].data.GetDenseData<float>(labelOutput);
                var expectedLabels = labelData.Select(l => l.IndexOf(l.Max())).ToList();

                var inputDataMap = new Dictionary<Variable, Value>() {
                    { imageInput, minibatchData[featureStreamInfo].data }
                };

                var outputDataMap = new Dictionary<Variable, Value>() {
                    { labelOutput, null }
                };

                model.Evaluate(inputDataMap, outputDataMap, device);
                var outputData = outputDataMap[labelOutput].GetDenseData<float>(labelOutput);
                var actualLabels = outputData.Select(l => l.IndexOf(l.Max())).ToList();

                int misMatches = actualLabels.Zip(expectedLabels, (a, b) => a.Equals(b) ? 0 : 1).Sum();

                miscountTotal += misMatches;
                Console.WriteLine($"Validating Model: Total Samples = {totalCount}, Misclassify Count = {miscountTotal}");

                if (totalCount > maxCount)
                    break;
            }

            float errorRate = 1.0F * miscountTotal / totalCount;
            Console.WriteLine($"Model Validation Error = {errorRate}");
            return errorRate;
        }

        public static void SaveAndReloadModel(ref Function function, IList<Variable> variables, DeviceDescriptor device, uint rank = 0)
        {
            string tempModelPath = "feedForward.net" + rank;
            File.Delete(tempModelPath);

            IDictionary<string, Variable> inputVarUids = new Dictionary<string, Variable>();
            IDictionary<string, Variable> outputVarNames = new Dictionary<string, Variable>();

            foreach (var variable in variables)
            {
                if (variable.IsOutput)
                    outputVarNames.Add(variable.Owner.Name, variable);
                else
                    inputVarUids.Add(variable.Uid, variable);
            }

            function.Save(tempModelPath);
            function = Function.Load(tempModelPath, device);

            File.Delete(tempModelPath);

            var inputs = function.Inputs;
            foreach (var inputVarInfo in inputVarUids.ToList())
            {
                var newInputVar = inputs.First(v => v.Uid == inputVarInfo.Key);
                inputVarUids[inputVarInfo.Key] = newInputVar;
            }

            var outputs = function.Outputs;
            foreach (var outputVarInfo in outputVarNames.ToList())
            {
                var newOutputVar = outputs.First(v => v.Owner.Name == outputVarInfo.Key);
                outputVarNames[outputVarInfo.Key] = newOutputVar;
            }
        }

        public static bool MiniBatchDataIsSweepEnd(ICollection<MinibatchData> minibatchValues)
        {
            return minibatchValues.Any(a => a.sweepEnd);
        }

        public static void PrintTrainingProgress(Trainer trainer, int minibatchIdx, int outputFrequencyInMinibatches)
        {
            if ((minibatchIdx % outputFrequencyInMinibatches) == 0 && trainer.PreviousMinibatchSampleCount() != 0)
            {
                float trainLossValue = (float)trainer.PreviousMinibatchLossAverage();
                float evaluationValue = (float)trainer.PreviousMinibatchEvaluationAverage();
                Console.WriteLine($"Minibatch: {minibatchIdx} CrossEntropyLoss = {trainLossValue}, EvaluationCriterion = {evaluationValue}");
            }
        }

        public static void PrintOutputDims(Function function, string functionName)
        {
            NDShape shape = function.Output.Shape;

            if (shape.Rank == 3)
            {
                Console.WriteLine($"{functionName} dim0: {shape[0]}, dim1: {shape[1]}, dim2: {shape[2]}");
            }
            else
            {
                Console.WriteLine($"{functionName} dim0: {shape[0]}");
            }
        }

    }

    class LogisticRegression
    {
        static int inputDim = 3;
        static int numOutputClasses = 2;

        static public void TrainAndEvaluate(DeviceDescriptor device)
        {
            // build a logistic regression model
            Variable featureVariable = Variable.InputVariable(new int[] { inputDim }, DataType.Float);
            Variable labelVariable = Variable.InputVariable(new int[] { numOutputClasses }, DataType.Float);
            var classifierOutput = CreateLinearModel(featureVariable, numOutputClasses, device);
            var loss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, labelVariable);
            var evalError = CNTKLib.ClassificationError(classifierOutput, labelVariable);

            // prepare for training
            CNTK.TrainingParameterScheduleDouble learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.02, 1);
            IList<Learner> parameterLearners =
                new List<Learner>() { Learner.SGDLearner(classifierOutput.Parameters(), learningRatePerSample) };
            var trainer = Trainer.CreateTrainer(classifierOutput, loss, evalError, parameterLearners);

            int minibatchSize = 64;
            int numMinibatchesToTrain = 1000;
            int updatePerMinibatches = 50;

            // train the model
            for (int minibatchCount = 0; minibatchCount < numMinibatchesToTrain; minibatchCount++)
            {
                Value features, labels;
                GenerateValueData(minibatchSize, inputDim, numOutputClasses, out features, out labels, device);
                //TODO: sweepEnd should be set properly instead of false.
#pragma warning disable 618
                trainer.TrainMinibatch(
                    new Dictionary<Variable, Value>() { { featureVariable, features }, { labelVariable, labels } }, device);
#pragma warning restore 618
                TestHelper.PrintTrainingProgress(trainer, minibatchCount, updatePerMinibatches);
            }

            // test and validate the model
            int testSize = 100;
            Value testFeatureValue, expectedLabelValue;
            GenerateValueData(testSize, inputDim, numOutputClasses, out testFeatureValue, out expectedLabelValue, device);

            // GetDenseData just needs the variable's shape
            IList<IList<float>> expectedOneHot = expectedLabelValue.GetDenseData<float>(labelVariable);
            IList<int> expectedLabels = expectedOneHot.Select(l => l.IndexOf(1.0F)).ToList();

            var inputDataMap = new Dictionary<Variable, Value>() { { featureVariable, testFeatureValue } };
            var outputDataMap = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
            classifierOutput.Evaluate(inputDataMap, outputDataMap, device);
            var outputValue = outputDataMap[classifierOutput.Output];
            IList<IList<float>> actualLabelSoftMax = outputValue.GetDenseData<float>(classifierOutput.Output);
            var actualLabels = actualLabelSoftMax.Select((IList<float> l) => l.IndexOf(l.Max())).ToList();
            int misMatches = actualLabels.Zip(expectedLabels, (a, b) => a.Equals(b) ? 0 : 1).Sum();

            Console.WriteLine($"Validating Model: Total Samples = {testSize}, Misclassify Count = {misMatches}");
        }

        private static void GenerateValueData(int sampleSize, int inputDim, int numOutputClasses,
            out Value featureValue, out Value labelValue, DeviceDescriptor device)
        {
            float[] features;
            float[] oneHotLabels;
            GenerateRawDataSamples(sampleSize, inputDim, numOutputClasses, out features, out oneHotLabels);

            featureValue = Value.CreateBatch<float>(new int[] { inputDim }, features, device);
            labelValue = Value.CreateBatch<float>(new int[] { numOutputClasses }, oneHotLabels, device);
        }

        private static void GenerateRawDataSamples(int sampleSize, int inputDim, int numOutputClasses,
            out float[] features, out float[] oneHotLabels)
        {
            Random random = new Random(0);

            features = new float[sampleSize * inputDim];
            oneHotLabels = new float[sampleSize * numOutputClasses];

            for (int sample = 0; sample < sampleSize; sample++)
            {
                int label = random.Next(numOutputClasses);
                for (int i = 0; i < numOutputClasses; i++)
                {
                    oneHotLabels[sample * numOutputClasses + i] = label == i ? 1 : 0;
                }

                for (int i = 0; i < inputDim; i++)
                {
                    features[sample * inputDim + i] = (float)GenerateGaussianNoise(3, 1, random) * (label + 1);
                }
            }
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
        /// https://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <returns></returns>
        static double GenerateGaussianNoise(double mean, double stdDev, Random random)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double stdNormalRandomValue = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * stdNormalRandomValue;
        }

        private static Function CreateLinearModel(Variable input, int outputDim, DeviceDescriptor device)
        {
            int inputDim = input.Shape[0];
            var weightParam = new Parameter(new int[] { outputDim, inputDim }, DataType.Float, 1, device, "w");
            var biasParam = new Parameter(new int[] { outputDim }, DataType.Float, 0, device, "b");

            return CNTKLib.Times(weightParam, input) + biasParam;
        }
    }

    /// <summary>
    /// This class shows how to build a recurrent neural network model from ground up and train the model. 
    /// </summary>
    public class LSTMSequenceClassifier
    {
        /// <summary>
        /// Execution folder is: CNTK/x64/BuildFolder
        /// Data folder is: CNTK/Tests/EndToEndTests/Text/SequenceClassification/Data
        /// </summary>
        public static string DataFolder = "../../Tests/EndToEndTests/Text/SequenceClassification/Data";

        /// <summary>
        /// Build and train a RNN model.
        /// </summary>
        /// <param name="device">CPU or GPU device to train and run the model</param>
        public static void Train(DeviceDescriptor device)
        {
            const int inputDim = 2000;
            const int cellDim = 25;
            const int hiddenDim = 25;
            const int embeddingDim = 50;
            const int numOutputClasses = 5;

            // build the model
            var featuresName = "features";
            var features = Variable.InputVariable(new int[] { inputDim }, DataType.Float, featuresName, null, true /*isSparse*/);
            var labelsName = "labels";
            var labels = Variable.InputVariable(new int[] { numOutputClasses }, DataType.Float, labelsName,
                new List<Axis>() { Axis.DefaultBatchAxis() }, true);

            var classifierOutput = LSTMSequenceClassifierNet(features, numOutputClasses, embeddingDim, hiddenDim, cellDim, device, "classifierOutput");
            Function trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, labels, "lossFunction");
            Function prediction = CNTKLib.ClassificationError(classifierOutput, labels, "classificationError");

            // prepare training data
            IList<StreamConfiguration> streamConfigurations = new StreamConfiguration[]
            { new StreamConfiguration(featuresName, inputDim, true, "x"), new StreamConfiguration(labelsName, numOutputClasses, false, "y") };
            var minibatchSource = MinibatchSource.TextFormatMinibatchSource(
                Path.Combine(DataFolder, "Train.ctf"), streamConfigurations,
                MinibatchSource.InfinitelyRepeat, true);
            var featureStreamInfo = minibatchSource.StreamInfo(featuresName);
            var labelStreamInfo = minibatchSource.StreamInfo(labelsName);

            // prepare for training
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(
                0.0005, 1);
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() {
                Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, /*unitGainMomentum = */true)  };
            var trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);

            // train the model
            uint minibatchSize = 200;
            int outputFrequencyInMinibatches = 20;
            int miniBatchCount = 0;
            int numEpochs = 5;
            while (numEpochs > 0)
            {
                var minibatchData = minibatchSource.GetNextMinibatch(minibatchSize, device);

                var arguments = new Dictionary<Variable, MinibatchData>
                {
                    { features, minibatchData[featureStreamInfo] },
                    { labels, minibatchData[labelStreamInfo] }
                };

                trainer.TrainMinibatch(arguments, device);
                TestHelper.PrintTrainingProgress(trainer, miniBatchCount++, outputFrequencyInMinibatches);

                // Because minibatchSource is created with MinibatchSource.InfinitelyRepeat, 
                // batching will not end. Each time minibatchSource completes an sweep (epoch),
                // the last minibatch data will be marked as end of a sweep. We use this flag
                // to count number of epochs.
                if (TestHelper.MiniBatchDataIsSweepEnd(minibatchData.Values))
                {
                    numEpochs--;
                }
            }
        }

        static Function Stabilize<ElementType>(Variable x, DeviceDescriptor device)
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
                    CNTKLib.Exp(CNTKLib.ElementTimes(f, new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device)))));
            return CNTKLib.ElementTimes(beta, x);
        }

        static Tuple<Function, Function> LSTMPCellWithSelfStabilization<ElementType>(
            Variable input, Variable prevOutput, Variable prevCellState, DeviceDescriptor device)
        {
            int outputDim = prevOutput.Shape[0];
            int cellDim = prevCellState.Shape[0];

            bool isFloatType = typeof(ElementType).Equals(typeof(float));
            DataType dataType = isFloatType ? DataType.Float : DataType.Double;

            Func<int, Parameter> createBiasParam;
            if (isFloatType)
                createBiasParam = (dim) => new Parameter(new int[] { dim }, 0.01f, device, "");
            else
                createBiasParam = (dim) => new Parameter(new int[] { dim }, 0.01, device, "");

            uint seed2 = 1;
            Func<int, Parameter> createProjectionParam = (oDim) => new Parameter(new int[] { oDim, NDShape.InferredDimension },
                    dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0, seed2++), device);

            Func<int, Parameter> createDiagWeightParam = (dim) =>
                new Parameter(new int[] { dim }, dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0, seed2++), device);

            Function stabilizedPrevOutput = Stabilize<ElementType>(prevOutput, device);
            Function stabilizedPrevCellState = Stabilize<ElementType>(prevCellState, device);

            Func<Variable> projectInput = () =>
                createBiasParam(cellDim) + (createProjectionParam(cellDim) * input);

            // Input gate
            Function it =
                CNTKLib.Sigmoid(
                    (Variable)(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                    CNTKLib.ElementTimes(createDiagWeightParam(cellDim), stabilizedPrevCellState), "inputGate");
            Function bit = CNTKLib.ElementTimes(
                it,
                CNTKLib.Tanh(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)), "bit");

            // Forget-me-not gate
            Function ft = CNTKLib.Sigmoid(
                (Variable)(
                        projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                        CNTKLib.ElementTimes(createDiagWeightParam(cellDim), stabilizedPrevCellState), "forgetGate");
            Function bft = CNTKLib.ElementTimes(ft, prevCellState, "bft");

            Function ct = (Variable)bft + bit;

            // Output gate
            Function ot = CNTKLib.Sigmoid(
                (Variable)(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                CNTKLib.ElementTimes(createDiagWeightParam(cellDim), Stabilize<ElementType>(ct, device)), "outputGate");
            Function ht = CNTKLib.ElementTimes(ot, CNTKLib.Tanh(ct));

            Function c = ct;
            Function h = (outputDim != cellDim) ? (createProjectionParam(outputDim) * Stabilize<ElementType>(ht, device)) : ht;

            return new Tuple<Function, Function>(h, c);
        }


        static Tuple<Function, Function> LSTMPComponentWithSelfStabilization<ElementType>(Variable input,
            NDShape outputShape, NDShape cellShape,
            Func<Variable, Function> recurrenceHookH,
            Func<Variable, Function> recurrenceHookC,
            DeviceDescriptor device)
        {
            var dh = Variable.PlaceholderVariable(outputShape, input.DynamicAxes);
            var dc = Variable.PlaceholderVariable(cellShape, input.DynamicAxes);

            var LSTMCell = LSTMPCellWithSelfStabilization<ElementType>(input, dh, dc, device);
            var actualDh = recurrenceHookH(LSTMCell.Item1);
            var actualDc = recurrenceHookC(LSTMCell.Item2);

            // Form the recurrence loop by replacing the dh and dc placeholders with the actualDh and actualDc
            (LSTMCell.Item1).ReplacePlaceholders(new Dictionary<Variable, Variable> { { dh, actualDh }, { dc, actualDc } });

            return new Tuple<Function, Function>(LSTMCell.Item1, LSTMCell.Item2);
        }

        private static Function Embedding(Variable input, int embeddingDim, DeviceDescriptor device)
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];
            var embeddingParameters = new Parameter(new int[] { embeddingDim, inputDim }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device);
            return CNTKLib.Times(embeddingParameters, input);
        }

        /// <summary>
        /// Build a one direction recurrent neural network (RNN) with long-short-term-memory (LSTM) cells.
        /// http://colah.github.io/posts/2015-08-Understanding-LSTMs/
        /// </summary>
        /// <param name="input">the input variable</param>
        /// <param name="numOutputClasses">number of output classes</param>
        /// <param name="embeddingDim">dimension of the embedding layer</param>
        /// <param name="LSTMDim">LSTM output dimension</param>
        /// <param name="cellDim">cell dimension</param>
        /// <param name="device">CPU or GPU device to run the model</param>
        /// <param name="outputName">name of the model output</param>
        /// <returns>the RNN model</returns>
        public static Function LSTMSequenceClassifierNet(Variable input, int numOutputClasses, int embeddingDim, int LSTMDim, int cellDim, DeviceDescriptor device,
            string outputName)
        {
            Function embeddingFunction = Embedding(input, embeddingDim, device);
            Func<Variable, Function> pastValueRecurrenceHook = (x) => CNTKLib.PastValue(x);
            Function LSTMFunction = LSTMPComponentWithSelfStabilization<float>(
                embeddingFunction,
                new int[] { LSTMDim },
                new int[] { cellDim },
                pastValueRecurrenceHook,
                pastValueRecurrenceHook,
                device).Item1;
            Function thoughtVectorFunction = CNTKLib.SequenceLast(LSTMFunction);

            return TestHelper.FullyConnectedLinearLayer(thoughtVectorFunction, numOutputClasses, device, outputName);
        }
    }
}
