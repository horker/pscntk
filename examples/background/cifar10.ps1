Set-StrictMode -Version Latest

############################################################
# Data producer
############################################################

$dataProducerScript = {
  param(
    $MinibatchDef,
    $PSScriptRoot
  )

  Set-StrictMode -Version Latest

  $ErrorActionPreference = "Stop"

  Import-Module HorkerDataQuery
  Import-Module psmath
  Import-Module pscntk

  $SQLITE_FILE = "$PSScriptRoot\cifar10.sqlite"
  $VALIDATION_SIZE = 20

  function Get-DataSource {
    param($image, $label)

    $image = $sample.image.Scale(0, 255, 0, 1.0)
    $label = (mat $sample.label).OneHot(10).ToFlatArray()

    cntk.datasourceset @{
      input = cntk.datasource $image 32, 32, 3, 1, -1
      label = cntk.datasource $label 10, 1, -1
    }
  }

  $connection = New-DataConnection $SQLITE_FILE

  try {

    # validation data

    $totalSize = (Invoke-DataQuery $connection "select count(*) Count from minibatches").Count

    $sample = Invoke-DataQuery $connection "select label, image from minibatches where rowid >= $($totalSize - $VALIDATION_SIZE + 1)"
    $data = Get-DataSource $sample.image $sample.label

    $MinibatchDef.SetValidationData($data)

    # test data

    $exit = $false
    while (!$exit) {
      for ($i = 1; $i -le $totalSize - $VALIDATION_SIZE; ++$i) {
        $sample = Invoke-DataQuery $connection "select label, image from minibatches where rowid = @id" @{ id = $i }

        $data = Get-DataSource $sample.image $sample.label

        $result = $MinibatchDef.AddDataSourceSet($data)
        if (!$result) {
          $exit = $true
          break
        }
      }
    }
  }
  finally {
    Close-DataConnection $connection
  }
}

############################################################
# Model
############################################################

$in = cntk.input 32, 32, 3 -Name "input"
$n = $in

# conv1: 32 x 32 x 3 -> 32 x 32 x 12
$w = cntk.parameter 3, 3, 3, 12 (cntk.truncatednormal)
$n = cntk.convolution -ConvolutionMap $w -Operand $n -Strides 1, 1, 3
$n = cntk.relu $n

# pool1: 32 x 32 x 12 -> 16 x 16 x 12
$n = cntk.pooling -Operand $n -PoolingType Max -PoolingWindowShape 3, 3 -Strides 2, 2 -AutoPadding $true

# conv2: 16 x 16 x 12 -> 16 x 16 x 24
$w = cntk.parameter 3, 3, 12, 24 (cntk.truncatednormal)
$n = cntk.convolution $w $n 1, 1, 12
$n = cntk.relu $n

# pool2: 16 x 16 x 24 -> 8 x 8 x 24
$n = cntk.pooling $n Max (3, 3) (2, 2) $true

# fc
$n = cntk.dense $n 10 (cntk.glorotuniform .5)

$out = $n

$label = cntk.input 10 -Name "label"

############################################################
# Training
############################################################

$MINIBATCH_SIZE = 50
$SAMPLE_COUNT_PER_EPOCH = 50 * 100

$learner = cntk.momentumsgd $out .05 .9

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

$minibatchDef = cntk.progminibatchdef -MinibatchSize $MINIBATCH_SIZE -SampleCountPerEpoch $SAMPLE_COUNT_PER_EPOCH -ValidationSize (50 * 20) -QueueSize 1000

$runner = cntk.backgroundscriptrunner
$runner.Start($dataProducerScript, $minibatchDef, $PSScriptRoot)

try {
  cntk.starttraining $trainer $minibatchDef -MaxIteration 100000 -ProgressOutputStep 500
}
finally {
  $minibatchDef.CancelAdding()
  $null = $runner.Finish()
  $runner.Dispose()
}

$out.Save("$PSScriptRoot\..\nonlinear.cntkmodel")
