Set-StrictMode -Version latest

Import-Module psmath

############################################################
# Data producer
############################################################

$dataProducerScript = {
  param(
    $minibatchDef
  )

  Set-StrictMode -Version Latest

  $ErrorActionPreference = "Stop"

  Import-Module psmath
  Import-Module pscntk

  while ($true) {
    $dataA = seq 0 (math.pi) 0.001 -func { (math.sin $x) + (st.normal 0 .1).gen() }, { (math.cos $x) + (st.normal 0 .1).gen() }, { "A" }
    $dataB = seq (math.pi) (2 * (math.pi)) 0.01 -func { .7 + (math.sin $x) + (st.normal 0 .1).gen() }, { 1 + (math.cos $x) + (st.normal 0 .1).gen() }, { "B" }
    $data = $dataA + $dataB

    $data = pso.shuffle $data

    $features = cntk.datasource -Rows $data.y0, $data.y1
    $features.Reshape(2, 1, -1)

    $l = pso.onehot $data.y2
    $labels = cntk.datasource -Rows $l.A, $l.B
    $labels.Reshape(2, 1, -1)

    $data = cntk.datasourceset @{
      input = $features
      label = $labels
    }

    $result = $minibatchdef.AddDataSourceSet($data)
    if (!$result) {
      break;
    }
  }
}

############################################################
# Model
############################################################

$INPUT_DIM = 2
$OUTPUT_CLASSES = 2
$HIDDEN_NODES = 100

$in  = cntk.input $INPUT_DIM -Name "input"

$n = $in
$n = cntk.dense $n $HIDDEN_NODES (cntk.henormal) relu
$n = cntk.dense $n $OUTPUT_CLASSES (cntk.glorotnormal) sigmoid
$out = $n

$label = cntk.input $OUTPUT_CLASSES -Name "label"

############################################################
# Start training
############################################################

$MINIBATCH_SIZE = 20
$SAMPLE_COUNT_PER_EPOCH = 20 * 100

$learner = cntk.momentumsgd $out .01 .5

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

$minibatchDef = cntk.progminibatchdef -MinibatchSize $MINIBATCH_SIZE -SampleCountPerEpoch $SAMPLE_COUNT_PER_EPOCH -ValidationSize 1000 -QueueSize 1000

$runner = cntk.backgroundscriptrunner
$runner.Start($dataProducerScript, $minibatchDef)

try {
  cntk.starttraining $trainer $minibatchDef -MaxIteration 10000 -ProgressOutputStep 100
}
finally {
  $minibatchDef.CancelAdding()
  $null = $runner.Finish()
  $runner.Dispose()
}
