using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    // ref.
    // I. Loshchilov, et al. "SGDR: Stochastic Gradient Descent with Warm Restarts"
    // https://arxiv.org/abs/1608.03983

    public class CosineAnealingScheduler : ILearningScheduler
    {
        public double MinimumRate { get; private set; }
        public double MaximumRate { get; private set; }
        public double Step { get; }

        public double LearningRate { get; private set; }

        public CosineAnealingScheduler(double minimumRate, double maximumRate, double step)
        {
            MinimumRate = minimumRate;
            LearningRate = MaximumRate = maximumRate;
            Step = step;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            LearningRate = MinimumRate + .5 * (MaximumRate - MinimumRate) * (1.0 + Math.Cos(Math.PI * iteration / Step));
            return true;
        }
    }
}
