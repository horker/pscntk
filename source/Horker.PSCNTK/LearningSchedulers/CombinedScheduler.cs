using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class CombinedScheduler : ILearningScheduler
    {
        private List<ILearningScheduler> _schedulers;

        public IReadOnlyList<ILearningScheduler> Schedulers => _schedulers;

        public double LearningRate { get; private set; }

        public CombinedScheduler(IList<ILearningScheduler> schedulers)
        {
            _schedulers = new List<ILearningScheduler>(schedulers);

            LearningRate = _schedulers.Aggregate(1.0, (acc, s) => acc * s.LearningRate);
        }

        public bool UpdateLearningRate(int epoch, int iteration, double loss)
        {
            var update = false;
            var rate = 1.0;

            foreach (var s in _schedulers)
            {
                update |= s.UpdateLearningRate(epoch, iteration, loss);
                rate *= s.LearningRate;
            }

            LearningRate = rate;

            return update;
        }
    }
}
