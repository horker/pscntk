using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using Horker.PSCNTK;

namespace benchmarkapp
{
    class Program
    {
        static string backgroundScript = @"
  param(
    $sampler,
    $MINIBATCH_SIZE
  )

  Set-StrictMode -Version Latest

  $ErrorActionPreference = 'Stop'

  Import-Module psmath
  Import-Module pscntk

  function Generate-Samples {
    $dataA = seq 0 (math.pi) 0.001 -func { (math.sin $x) + (st.normal 0 .1).gen() }, { (math.cos $x) + (st.normal 0 .1).gen() }, { 'A' }
    $dataB = seq (math.pi) (2 * (math.pi)) 0.01 -func { .7 + (math.sin $x) + (st.normal 0 .1).gen() }, { 1 + (math.cos $x) + (st.normal 0 .1).gen() }, { 'B' }
    $data = $dataA + $dataB

    $data = pso.shuffle $data

    $data
  }

  function Preprocess-Data {
    param($batch)

    $features = cntk.datasource -Rows $batch.y0, $batch.y1
    $features.Reshape(2, 1, -1)

    $l = pso.onehot $batch.y2 -Categories 'A', 'B'
    $labels = cntk.datasource -Rows $l.A, $l.B
    $labels.Reshape(2, 1, -1)

    return $features, $labels
  }

  $data = Generate-Samples
  $validation = $data.Slice(@(0, ($data.Length * .3)))
  $features, $labels = Preprocess-Data $validation
  $sampler.SetValidationData(@{ 'input' = $features; 'label' = $labels })

  $exit = $false
  while (!$exit) {
    $data = Generate-Samples

    for ($i = 0; $i -lt $data.Length - $MINIBATCH_SIZE; $i += $MINIBATCH_SIZE) {
      $batch = $data.Slice(@($i, ($i + $MINIBATCH_SIZE)))

      $features, $labels = Preprocess-Data $batch

      if (!$sampler.AddMinibatch(@{ 'input' = $features; 'label' = $labels })) {
        $exit = $true
        break
      }
    }
  }
";

        public static readonly int MINIBATCH_SIZE = 20;

        static Tuple<Function, Variable> GetModel()
        {
            var input = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "input");
            var n = Composite.Dense(input, new int[] { 100 }, CNTKLib.HeNormalInitializer(), true, CNTKLib.ConstantInitializer(0), false, "relu", null, "");
            var output = Composite.Dense(n, new int[] { 2 }, CNTKLib.GlorotNormalInitializer(), true, CNTKLib.ConstantInitializer(0), false, "sigmoid", null, "");

            var label = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "label");

            return Tuple.Create(output, label);
        }

        static void TestApp()
        {
            var sampler = new ParallelSampler(10000, 1000);

            var runners = new BackgroundScriptRunner[2];

            try
            {
                for (var i = 0; i < runners.Length; ++i)
                {
                    var runner = new BackgroundScriptRunner();
                    var script = ScriptBlock.Create(backgroundScript);
                    runner.Start(script, new object[] { sampler, MINIBATCH_SIZE });
                    runners[i] = runner;
                }

                var model = GetModel();
                var output = model.Item1;
                var label = model.Item2;

                var loss = CNTKLib.BinaryCrossEntropy(output, label);
                var metric = CNTKLib.ClassificationError(output, label);

                var lr = new TrainingParameterScheduleDouble(.05);
                var learner = CNTKLib.SGDLearner(new ParameterVector(output.Parameters().ToArray()), lr);

                var session = new TrainingSession(output, loss, metric, learner, sampler, null);

                var progress = session.GetIterator().GetEnumerator();
                for (var i = 0; i < 10000; ++i)
                {
                    progress.MoveNext();
                    var p = progress.Current;

                    if (p.Iteration == 1 || p.Iteration % 100 == 0)
                    {
                        Console.WriteLine(string.Format("Iteration: {0}  Loss: {1}  Metric: {2}  Validation: {3}  Elapsed: {4}  CountInQueue: {5}",
                            p.Iteration, p.Loss, p.Metric, p.GetValidationMetric(), p.Elapsed, sampler.CountInQueue));
                    }
                }
            }
            finally
            {
                sampler.CancelAdding();

                foreach (var runner in runners)
                {
                    runner.Finish();
                    runner.Dispose();
                }
            }
        }

        static void Main(string[] args)
        {
            UnmanagedDllLoader.Load("..\\..\\..\\..\\pscntk\\lib");

            TestApp();

            Console.Write("Push any key to exit");
            Console.ReadLine();
        }
    }
}
