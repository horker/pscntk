using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class TrainingLoop
    {
        public static IEnumerable<TrainingProgress> Start(TrainingSession session, int maxIteration, int progressOutputStep, Logger logger)
        {
            int sampleCount = 0;
            var loss = 0.0;
            var metric = 0.0;

            var oldControlCTreatment = Console.TreatControlCAsInput;
            Console.TreatControlCAsInput = true;

            try
            {
                foreach (var t in session.GetIterator(maxIteration))
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key == ConsoleKey.C)
                            break;
                    }

                    sampleCount += t.SampleCount;
                    loss += t.Loss;
                    metric += t.Metric;
                    if (t.Iteration % progressOutputStep == 0 || t.Iteration == maxIteration)
                    {
                        var p = new TrainingProgress();

                        p.Epoch = t.Epoch;
                        p.Iteration = t.Iteration;
                        p.SampleCount = sampleCount;
                        p.Loss = loss / progressOutputStep;
                        p.Metric = metric / progressOutputStep;
                        p.Validation = t.GetValidationMetric();
                        p.Elapsed = t.Elapsed;

                        if (logger != null)
                            logger.Info(p, "Training Progress");

                        yield return p;

                        sampleCount = 0;
                        loss = 0.0;
                        metric = 0.0;
                    }
                }
            }
            finally
            {
                Console.TreatControlCAsInput = oldControlCTreatment;
            }
        }
    }
}
