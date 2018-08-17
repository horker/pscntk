param(
  [switch]$Progressive
)

Set-StrictMode -Version latest

Import-Module psmath

$MINIBATCH_SIZE = 20
$SAMPLE_COUNT_PER_EPOCH = 20 * 100

############################################################
# On-memory minibatch definition
############################################################

function Generate-Samples {
  $dataA = seq 0 (math.pi) 0.001 -func { (math.sin $x) + (st.normal 0 .1).gen() }, { (math.cos $x) + (st.normal 0 .1).gen() }, { "A" }
  $dataB = seq (math.pi) (2 * (math.pi)) 0.01 -func { .7 + (math.sin $x) + (st.normal 0 .1).gen() }, { 1 + (math.cos $x) + (st.normal 0 .1).gen() }, { "B" }
  $data = $dataA + $dataB

  $data = pso.shuffle $data

  $data
}

function New-MinibatchDefinition {

  $data = Generate-Samples

#$data | oxyscat -xname y0 -yname y1 -groupname y2 | show-oxyplot

  $features = cntk.datasource -Rows $data.y0, $data.y1
  $features.Reshape(2, 1, -1)

  $l = pso.onehot $data.y2
  $labels = cntk.datasource -Rows $l.A, $l.B
  $labels.Reshape(2, 1, -1)

  cntk.minibatchdef @{ input = $features; label = $labels } 20 .3
}

############################################################
# Progressive minibatch definition
############################################################

$dataProducerScript = {
  param(
    $minibatchDef,
    $MINIBATCH_SIZE
  )

  Set-StrictMode -Version Latest

  $ErrorActionPreference = "Stop"

  Import-Module psmath
  Import-Module pscntk

  function Generate-Samples {
    $dataA = seq 0 (math.pi) 0.001 -func { (math.sin $x) + (st.normal 0 .1).gen() }, { (math.cos $x) + (st.normal 0 .1).gen() }, { "A" }
    $dataB = seq (math.pi) (2 * (math.pi)) 0.01 -func { .7 + (math.sin $x) + (st.normal 0 .1).gen() }, { 1 + (math.cos $x) + (st.normal 0 .1).gen() }, { "B" }
    $data = $dataA + $dataB

    $data = pso.shuffle $data

    $data
  }

  function Preprocess-Data {
    param($batch)

    $features = cntk.datasource -Rows $batch.y0, $batch.y1
    $features.Reshape(2, 1, -1)

    $l = pso.onehot $batch.y2 -Categories "A", "B"
    $labels = cntk.datasource -Rows $l.A, $l.B
    $labels.Reshape(2, 1, -1)

    return $features, $labels
  }

  $data = Generate-Samples
  $validation = $data.Slice(@(0, ($data.Length * .3)))
  $features, $labels = Preprocess-Data $validation
  $minibatchdef.SetValidationData(@{ "input" = $features; "label" = $labels })

  $exit = $false
  while (!$exit) {
    $data = Generate-Samples

    for ($i = 0; $i -lt $data.Length - $MINIBATCH_SIZE; $i += $MINIBATCH_SIZE) {
      $batch = $data.Slice(@($i, ($i + $MINIBATCH_SIZE)))

      $features, $labels = Preprocess-Data $batch

      if (!$minibatchdef.AddMinibatch(@{ "input" = $features; "label" = $labels })) {
        $exit = $true
        break
      }
    }
  }
}

function New-ProgressiveMinibatchDefinition {
  cntk.progminibatchdef -SampleCountPerEpoch $SAMPLE_COUNT_PER_EPOCH -QueueSize 1000
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

if ($Progressive) {
  $minibatchDef = New-ProgressiveMinibatchDefinition

  $runners = 1..3 | foreach {
    $runner = cntk.backgroundscriptrunner
    $runner.Start($dataProducerScript, $minibatchDef, $MINIBATCH_SIZE)
    $runner
  }
}
else {
  $minibatchDef = New-MinibatchDefinition
}

$learner = cntk.momentumsgd $out .01 .5

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

try {
  cntk.starttraining $trainer $minibatchDef -MaxIteration 1000 -ProgressOutputStep 100
}
finally {
  if ($Progressive) {
    $minibatchDef.CancelAdding()

    $runners | foreach {
      $null = $_.Finish()
      $_.Dispose()
    }
  }
}
