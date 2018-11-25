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
        public double MinRate { get; private set; }
        public double MaxRate { get; private set; }
        public double TimeScale { get; }

        public double LearningRate { get; private set; }

        public CosineAnealingScheduler(double minRate, double maxRate, double timeScale)
        {
            MinRate = minRate;
            LearningRate = MaxRate = maxRate;
            TimeScale = timeScale;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            LearningRate = MinRate + .5 * (MaxRate - MinRate) * (1.0 + Math.Cos(Math.PI * iteration / TimeScale));
            return true;
        }
    }
}
