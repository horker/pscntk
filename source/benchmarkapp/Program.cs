using System;
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
    $minibatchDef,
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

  $exit = $false
  while (!$exit) {
    $data = Generate-Samples

    for ($i = 0; $i -lt $data.Length - $MINIBATCH_SIZE; $i += $MINIBATCH_SIZE) {
      $batch = $data.Slice(@($i, ($i + $MINIBATCH_SIZE)))

      $features = cntk.datasource -Rows $batch.y0, $batch.y1
      $features.Reshape(2, 1, -1)

      $l = pso.onehot $batch.y2 -Categories 'A', 'B'
      $labels = cntk.datasource -Rows $l.A, $l.B
      $labels.Reshape(2, 1, -1)

      $d = New-Object 'Collections.Generic.Dictionary[string, Horker.PSCNTK.DataSource[float]]'
      $d.Add('input', $features)
      $d.Add('label', $labels)
      $minibatch = New-Object Horker.PSCNTK.Minibatch $d

      if (!$minibatchdef.AddMinibatch($minibatch)) {
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
            var n = Layers.Dense(input, new int[] { 10 }, CNTKLib.HeNormalInitializer(), true, CNTKLib.GlorotNormalInitializer(), "relu", "");
            var output = Layers.Dense(n, new int[] { 2 }, CNTKLib.GlorotNormalInitializer(), true, CNTKLib.GlorotNormalInitializer(), "sigmoid", "");

            var label = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "label");

            return Tuple.Create(output, label);
        }

        static void TestApp()
        {
            var minibatchDef = new ProgressiveMinibatchDefinition(10000, 1000);

            var runners = new BackgroundScriptRunner[10];
            for (var i = 0; i < runners.Length; ++i)
            {
                var runner = new BackgroundScriptRunner();
                var script = ScriptBlock.Create(backgroundScript);
                runner.Start(script, new object[] { minibatchDef, MINIBATCH_SIZE });
                runners[i] = runner;
            }

            try
            {
                var model = GetModel();
                var output = model.Item1;
                var label = model.Item2;

                var lr = new TrainingParameterScheduleDouble(.05);
                var learner = CNTKLib.SGDLearner(new ParameterVector(output.Parameters().ToArray()), lr, new AdditionalLearningOptions());

                var loss = CNTKLib.BinaryCrossEntropy(output, label);
                var metric = CNTKLib.ClassificationError(output, label);

                var trainer = CNTKLib.CreateTrainer(output, loss, metric, new LearnerVector(new Learner[] { learner }));

                var session = new TrainingSession(trainer, minibatchDef);

                var progress = session.GetSession().GetEnumerator();
                for (var i = 0; i < 10000; ++i)
                {
                    progress.MoveNext();
                    var p = progress.Current;

                    Console.WriteLine(string.Format("Iteration: {0}  Loss: {1}  Metric: {2}  Validation: {3}  Elapsed: {4}  CountInQueue: {5}",
                        p.Iteration, p.Loss, p.Metric, p.GetValidationMetric(), p.Elapsed, minibatchDef.CountInQueue));
                }
            }
            finally
            {
                minibatchDef.CancelAdding();

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
