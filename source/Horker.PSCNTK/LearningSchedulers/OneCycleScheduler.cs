using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    // ref.
    // L. Smith, "Super-Convergence: Very Fast Training of Neural Networks Using Large Learning Rates"
    // See: https://arxiv.org/abs/1708.07120

    public class OneCycleScheduler : ILearningScheduler
    {

        public double InitialRate { get; }
        public double MaximumRate { get; }
        public double MinimumRate { get; }
        public double Step { get; }

        public double LearningRate { get; private set; }

        public OneCycleScheduler(double initialRate, double maximumRate, double minimumRate, int step)
        {
            InitialRate = initialRate;
            MaximumRate = maximumRate;
            MinimumRate = minimumRate;
            Step = step;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            if (iteration <= Step)
                LearningRate = InitialRate + iteration * (MaximumRate - InitialRate) / Step;
            else
                LearningRate = Math.Max(MaximumRate - (iteration - Step) * (MaximumRate - InitialRate) / Step, MinimumRate);

            return true;
        }
    }
}
