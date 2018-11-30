using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public interface ICallback
    {
        void Run(TrainingSession session);
    }
}
