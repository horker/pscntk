using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public interface ILearningScheduler
    {
        double LearningRate { get; }
        bool UpdateLearningRate(int epoch, int iteration, double loss);
    }
}
