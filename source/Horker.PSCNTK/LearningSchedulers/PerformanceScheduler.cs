using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class PerformanceScheduler : ILearningScheduler
    {
        public double InitialLearningRate { get; }
        public double DecayRate { get; }
        public int UpdateInterval { get; }

        public double LastLoss { get; private set; }

        public double LearningRate { get; private set; }

        public PerformanceScheduler(double initialRate, double decayRate, int updateInterval)
        {
            InitialLearningRate = LearningRate = initialRate;
            DecayRate = decayRate;
            UpdateInterval = updateInterval;
            LastLoss = double.MaxValue;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            bool update = false;
            if (iteration % UpdateInterval == 0)
            {
                if (loss >= LastLoss)
                {
                    LearningRate = LearningRate * DecayRate;
                    update = true;
                }
                LastLoss = loss;
            }

            return update;
        }
    }
}
