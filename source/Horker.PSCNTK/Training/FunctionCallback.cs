using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class ScriptCallback : ICallback
    {
        public Func<TrainingSession, bool> Func;
        public int Step;

        public ScriptCallback(Func<TrainingSession, bool> func, int step)
        {
            Func = func;
            Step = step;
        }

        public void Run(TrainingSession session)
        {
            if (session.Iteration % Step != 0)
                return;

            Func.Invoke(session);
        }
    }
}
