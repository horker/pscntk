using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class PowerScheduler : ILearningScheduler
    {
        public double InitialLearningRate { get; private set; }
        public double TimeScale { get; }
        public double Exponent { get; }

        public double LearningRate { get; private set; }

        public PowerScheduler(double initialRate, double timeScale, double exponent = 1.0)
        {
            InitialLearningRate = LearningRate = initialRate;
            TimeScale = timeScale;
            Exponent = exponent;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            LearningRate = InitialLearningRate * Math.Pow(1.0 + iteration / TimeScale, -Exponent);
            return true;
        }
    }
}
