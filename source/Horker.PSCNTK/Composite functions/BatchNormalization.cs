using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function BatchNormalization(Variable input, bool spatial, double initScale, double normalizationTimeConstant, double blendTimeConstant, double epsilon, bool useCNTKEngine, bool disableRegularization, string name)
        {
            var normShape = new int[] { CNTK.NDShape.InferredDimension };

            var scale = new Parameter(normShape, DataType.Float, initScale, DeviceDescriptor.UseDefaultDevice(), name + "_w");
            var bias = new Parameter(normShape, DataType.Float, 0, DeviceDescriptor.UseDefaultDevice(), name + "_b");

            var runningMean = new Constant(normShape, 0.0f, DeviceDescriptor.UseDefaultDevice());
            var runningInvStd = new Constant(normShape, 0.0f, DeviceDescriptor.UseDefaultDevice());
            var runningCount = Constant.Scalar(0.0f, DeviceDescriptor.UseDefaultDevice());

            var output = CNTKLib.BatchNormalization(
                input,                     // CNTK.Variable operand
                scale,                     // CNTK.Variable scale
                bias,                      // CNTK.Variable bias
                runningMean,               // CNTK.Variable runningMean
                runningInvStd,             // CNTK.Variable runningInvStd
                runningCount,              // CNTK.Variable runningCount
                spatial,                   // bool spatial
                normalizationTimeConstant, // double normalizationTimeConstant
                blendTimeConstant,         // double blendTimeConstant
                epsilon,                   // double epsilon
                !useCNTKEngine,            // bool useCuDNNEngine
                disableRegularization,     // bool disableRegularization
                ""                         // string name
            );

            output.RootFunction.SetName(name);

            return output;
        }
    }
}
