using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [CmdletBinding(DefaultParameterSetName = "Model")]
    public abstract class LearnerCmdletBaseWithoutLR : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Model")]
        public Function Model;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Parameters")]
        public Parameter[] Parameters;

        protected abstract Learner GenerateLearner(IList<Parameter> parameters);

        protected override void EndProcessing()
        {
            IList<Parameter> parameters;

            if (ParameterSetName == "Model")
                parameters = Model.Parameters();
            else
                parameters = Parameters;

            var learner = GenerateLearner(parameters);

            WriteObject(learner);
        }
    }

    [CmdletBinding(DefaultParameterSetName = "Model")]
    public abstract class LearnerCmdletBase : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Model")]
        public Function Model;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Parameters")]
        public Parameter[] Parameters;

        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 0.01;

        [Parameter(Position = 99, Mandatory = false)]
        public AdditionalLearningOptions Options = new AdditionalLearningOptions();

        protected abstract Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule);

        protected override void EndProcessing()
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);

            IList<Parameter> parameters;

            if (ParameterSetName == "Model")
                parameters = Model.Parameters();
            else
                parameters = Parameters;

            var learner = GenerateLearner(parameters, lr);

            WriteObject(learner);
        }
    }

    [Cmdlet("New", "CNTKSGD")]
    [Alias("cntk.sgd")]
    public class NewCNTKSGD : LearnerCmdletBase
    {
        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            return Learner.SGDLearner(parameters, learningRateSchedule, Options);
        }
    }

    [Cmdlet("New", "CNTKMomentumSGD")]
    [Alias("cntk.momentumsgd")]
    public class NewCNTKMomentumSGD : LearnerCmdletBase
    {
        [Parameter(Position = 2, Mandatory = false)]
        public double Momentum = 0.9;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter NoUnitGrain = !Constants.DefaultUnitGainValue;

        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            var m = new TrainingParameterScheduleDouble(Momentum);

            return Learner.MomentumSGDLearner(parameters, learningRateSchedule, m, !NoUnitGrain, Options);
        }
    }

    [Cmdlet("New", "CNTKAdaGrad")]
    [Alias("cntk.adagrad")]
    public class NewCNTKAdaGrad : LearnerCmdletBase
    {
        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter NoNeedAveMultiplier = false;

        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            return CNTKLib.AdaGradLearner(new ParameterVector(parameters.ToArray()), learningRateSchedule, NoNeedAveMultiplier, Options);
        }
    }

    [Cmdlet("New", "CNTKFSAdaGrad")]
    [Alias("cntk.fsadagrad")]
    public class NewCNTKFSAdaGrad : LearnerCmdletBase
    {
        [Parameter(Position = 2, Mandatory = false)]
        [Alias("Beta1")]
        public double Momentum = 0.9;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter NoUnitGrain = !Constants.DefaultUnitGainValue;

        [Parameter(Position = 4, Mandatory = false)]
        [Alias("Beta2")]
        public double VarianceMomentum = Constants.DefaultVarianceMomentum;

        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            var m = new TrainingParameterScheduleDouble(Momentum);
            var vm = new TrainingParameterScheduleDouble(VarianceMomentum);

            return CNTKLib.FSAdaGradLearner(new ParameterVector(parameters.ToArray()), learningRateSchedule, m, !NoUnitGrain, vm, Options);
        }
    }

    [Cmdlet("New", "CNTKRMSProp")]
    [Alias("cntk.rmsprop")]
    public class NewCNTKRMSProp : LearnerCmdletBase
    {
        [Parameter(Position = 2, Mandatory = true)]
        public double Gamma;

        [Parameter(Position = 3, Mandatory = true)]
        public double Inc;

        [Parameter(Position = 4, Mandatory = true)]
        public double Dec;

        [Parameter(Position = 5, Mandatory = true)]
        public double Max;

        [Parameter(Position = 6, Mandatory = false)]
        public double Min;

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter NoNeedAveMultiplier = false;

        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            return CNTKLib.RMSPropLearner(new ParameterVector(parameters.ToArray()), learningRateSchedule, Gamma, Inc, Dec, Max, Min, !NoNeedAveMultiplier, Options);
        }
    }

    [Cmdlet("New", "CNTKAdaDelta")]
    [Alias("cntk.adadelta")]
    public class NewCNTKAdaDelta : LearnerCmdletBaseWithoutLR
    {
        [Parameter(Position = 1, Mandatory = false)]
        public double LearningRate = 1.0;

        [Parameter(Position = 2, Mandatory = false)]
        public double Rho = 0.95;

        [Parameter(Position = 3, Mandatory = false)]
        public double Epsilon = 1e-8;

        [Parameter(Position = 4, Mandatory = false)]
        public AdditionalLearningOptions Options = new AdditionalLearningOptions();

        protected override Learner GenerateLearner(IList<Parameter> parameters)
        {
            var lr = new TrainingParameterScheduleDouble(LearningRate);

            return CNTKLib.AdaDeltaLearner(new ParameterVector(parameters.ToArray()), lr, Rho, Epsilon, Options);
        }
    }

    [Cmdlet("New", "CNTKAdam")]
    [Alias("cntk.adam")]
    public class NewCNTKAdam : LearnerCmdletBase
    {
        [Parameter(Position = 2, Mandatory = false)]
        [Alias("Beta1")]
        public double Momentum = 0.9;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter NoUnitGrain = !Constants.DefaultUnitGainValue;

        [Parameter(Position = 4, Mandatory = false)]
        [Alias("Beta2")]
        public double VarianceMomentum = Constants.DefaultVarianceMomentum;

        [Parameter(Position = 5, Mandatory = false)]
        public double Epsilon = 1e-8;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter Adamax = false;

        protected override Learner GenerateLearner(IList<Parameter> parameters, TrainingParameterScheduleDouble learningRateSchedule)
        {
            var m = new TrainingParameterScheduleDouble(Momentum);
            var vm = new TrainingParameterScheduleDouble(VarianceMomentum);

            return CNTKLib.AdamLearner(new ParameterVector(parameters.ToArray()), learningRateSchedule, m, !NoUnitGrain, vm, Epsilon, Adamax, Options);
        }
    }
}