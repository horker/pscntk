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
        public double Smoothing { get; }

        public double CurrentLoss { get; private set; }
        public double LastLoss { get; private set; }

        public double LearningRate { get; private set; }

        public PerformanceScheduler(double initialRate, double decayRate, int updateInterval, double smoothing = double.NaN)
        {
            InitialLearningRate = LearningRate = initialRate;
            DecayRate = decayRate;
            UpdateInterval = updateInterval;
            CurrentLoss = 0.0;
            LastLoss = double.MaxValue;

            if (double.IsNaN(smoothing))
                Smoothing = 2.0 / (updateInterval + 1);
            else
                Smoothing = smoothing;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            CurrentLoss = Smoothing * loss + (1 - Smoothing) * CurrentLoss;

            bool update = false;
            if (iteration % UpdateInterval == 0)
            {
                if (CurrentLoss >= LastLoss)
                {
                    LearningRate = LearningRate * DecayRate;
                    update = true;
                }
                LastLoss = CurrentLoss;
            }

            return update;
        }
    }
}
