using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class ExponentialScheduler : ILearningScheduler
    {
        public double InitialLearningRate { get; private set; }
        public double TimeScale { get; }

        public double LearningRate { get; private set; }

        public ExponentialScheduler(double initialRate, double timeScale)
        {
            InitialLearningRate = LearningRate = initialRate;
            TimeScale = timeScale;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            LearningRate = InitialLearningRate * Math.Pow(10, -iteration / TimeScale);
            return true;
        }
    }
}
