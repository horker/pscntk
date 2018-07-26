using System.Linq;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKSGD")]
    [Alias("cntk.sgd")]
    public class NewCNTKSGD : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Function Model;

        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 0.01;

        [Parameter(Position = 2, Mandatory = false)]
        public AdditionalLearningOptions Options = null;

        protected override void EndProcessing()
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);

            var result = Learner.SGDLearner(Model.Parameters(), lr, Options);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKMomentumSGD")]
    [Alias("cntk.momentumsgd")]
    public class NewCNTKMomentumSGD : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Function Model;

        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 0.01;

        [Parameter(Position = 2, Mandatory = false)]
        public double Momentum = 0.9;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter NoUnitGrain = false;

        [Parameter(Position = 4, Mandatory = false)]
        public AdditionalLearningOptions Options = null;

        protected override void EndProcessing()
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);
            var m = new TrainingParameterScheduleDouble(Momentum);

            var result = Learner.MomentumSGDLearner(Model.Parameters(), lr, m, !NoUnitGrain, Options);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKAdaGrad")]
    [Alias("cntk.adagrad")]
    public class NewCNTKAdaGrad : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Function Model;

        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 0.01;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter NoNeedAveMultiplier = false;

        [Parameter(Position = 3, Mandatory = false)]
        public AdditionalLearningOptions Options = null;

        protected override void EndProcessing()
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);

            var result = CNTKLib.AdaGradLearner(new CNTK.ParameterVector(Model.Parameters().ToArray()), lr, !NoNeedAveMultiplier, Options);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKAdam")]
    [Alias("cntk.adam")]
    public class NewCNTKAdam : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Function Model;

        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 0.01;

        [Parameter(Position = 2, Mandatory = false)]
        public double Momentum = 0.9;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter NoUnitGrain = false;

//        [Parameter(Position = 4, Mandatory = false)]
//        public AdditionalLearningOptions Options = null;

        protected override void EndProcessing()
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);
            var m = new TrainingParameterScheduleDouble(Momentum);

            var result = CNTKLib.AdamLearner(new CNTK.ParameterVector(Model.Parameters().ToArray()), lr, m, !NoUnitGrain);
            WriteObject(result);
        }
    }
}