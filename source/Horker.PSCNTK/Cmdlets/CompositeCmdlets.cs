﻿using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKDense")]
    [Alias("cntk.dense")]
    [OutputType(typeof(WrappedFunction))]
    public class NewCNTKDense : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = false)]
        public int[] Shape = null;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 3, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 4, Mandatory = false)]
        public bool Bias = true;

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter Stabilize = false;

        [Parameter(Position = 7, Mandatory = false)]
        public double Steepness = 4;

        [Parameter(Position = 8, Mandatory = false)]
        public DeviceDescriptor Device = null;

        [Parameter(Position = 9, Mandatory = false)]
        public string Name = "dense";

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = DeviceDescriptor.UseDefaultDevice();

            var result = Composite.Dense(
                Input,           // Variable input,
                Shape,           // Shape outputShape,
                Initializer,     // CNTKDictionary initializer,
                Bias,            // bool hasBias,
                BiasInitializer, // CNTKDictionary biasInitializer,
                Stabilize,       // bool stabilize,
                Steepness,       // double steepness
                Activation,      // string activation
                Device,          // DeviceDescriptor device,
                Name             // string name
            );

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKMaxPooling")]
    [Alias("cntk.maxPooling")]
    [OutputType(typeof(WrappedFunction))]
    public class NewCNTKMaxPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool CeilOutDim = false;

        [Parameter(Position = 5, Mandatory = false)]
        public bool IncludePad = false;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "maxPooling";

        protected override void EndProcessing()
        {
            var result = CNTKLib.Pooling(Input, PoolingType.Max, FilterShape, Strides, new BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAveragePooling")]
    [Alias("cntk.averagePooling")]
    [OutputType(typeof(WrappedFunction))]
    public class NewCNTKAveragePooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool CeilOutDim = false;

        [Parameter(Position = 5, Mandatory = false)]
        public bool IncludePad = false;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "averagePooling";

        protected override void EndProcessing()
        {
            var result = CNTKLib.Pooling(Input, PoolingType.Average, FilterShape, Strides, new BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKBatchNorm")]
    [Alias("cntk.batchNorm")]
    [OutputType(typeof(WrappedFunction))]
    public class NewCNTKBatchNorm : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

        [Parameter(Position = 1, Mandatory = false)]
        public SwitchParameter Spatial = false;

        [Parameter(Position = 2, Mandatory = false)]
        public double InitialScale = 1.0;

        [Parameter(Position = 3, Mandatory = false)]
        public double NormalizationTimeConstant = 5000;

        [Parameter(Position = 4, Mandatory = false)]
        public double BlendTimeConstant = 0;

        [Parameter(Position = 5, Mandatory = false)]
        public double Epsilon = 0.00001;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter UseCuDNNEngine = false;

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter DisableRegularization = false;

        [Parameter(Position = 8, Mandatory = false)]
        public string Name = "batchNorm";

        protected override void EndProcessing()
        {
            var result = Composite.BatchNormalization(Input, Spatial, InitialScale, NormalizationTimeConstant, BlendTimeConstant, Epsilon, UseCuDNNEngine, DisableRegularization, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKOptimizedRNNStack")]
    [Alias("cntk.rnnStack")]
    [OutputType(typeof(WrappedFunction))]
    public class NewCNTKOptimizedRNNStack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int HiddenSize;

        [Parameter(Position = 2, Mandatory = false)]
        public int LayerSize = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter Bidirectional = false;

        [Parameter(Position = 4, Mandatory = false)]
        [ValidateSet("lstm", "gru", "rnnTanh", "rnnReLU")]
        public string CellType = "lstm";

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "optimizedRNNStack";

        protected override void EndProcessing()
        {
            var result = Composite.OptimizedRNNStack(Input, HiddenSize, LayerSize, Bidirectional, CellType, Name);

            WriteObject(new WrappedFunction(result));
        }
    }
}
