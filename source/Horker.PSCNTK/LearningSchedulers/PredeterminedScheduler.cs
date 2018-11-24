using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class LearningSchedule
    {
        public int IterationSize { get; }
        public double LearningRate { get; }

        public LearningSchedule(int iterationSize, double learningRate)
        {
            IterationSize = iterationSize;
            LearningRate = learningRate;
        }
    }

    public class PredeterminedScheduler : ILearningScheduler
    {
        private List<LearningSchedule> _schedules;
        private int _stage;
        private int _totalIterationSize;

        public double LearningRate { get; private set; }

        public PredeterminedScheduler()
        {
            _schedules = new List<LearningSchedule>();
            _stage = 0;
        }

        public void AddLearningSchedule(int iterationSize, double learningRate)
        {
            _schedules.Add(new LearningSchedule(iterationSize, learningRate));

            // Initialize _accumativeIterationSize every time
            _totalIterationSize = _schedules[0].IterationSize;
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            if (_stage < _schedules.Count - 1 && iteration >= _totalIterationSize)
            {
                ++_stage;
                _totalIterationSize += _schedules[_stage].IterationSize;
                LearningRate = _schedules[_stage].LearningRate;
                return true;
            }

            return false;
        }
    }
}
