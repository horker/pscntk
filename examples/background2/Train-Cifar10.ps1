Set-StrictMode -Version Latest

############################################################
# Data producer
############################################################

$dataProducerScript = {
  param(
    $Sampler,
    $PSScriptRoot
  )

  Set-StrictMode -Version Latest

  $ErrorActionPreference = "Stop"

  Import-Module HorkerDataQuery
  Import-Module psmath
  Import-Module pscntk

  $SQLITE_FILE = "$PSScriptRoot\cifar10.sqlite"
  $TABLE = "minibatches"
  $VALIDATION_SIZE = 50

  function Get-DataSource {
    param($image, $label)

    $image = $image.Scale(0, 255, 0, 1.0)
    $label = $label.OneHot(10).ToFlatArray($true)

    cntk.datasourceset @{
      input = cntk.datasource $image 32, 32, 3, 1, -1
      label = cntk.datasource $label 10, 1, -1
    }
  }

  $connection = New-DataConnection $SQLITE_FILE

  try {

    # validation data

    $totalSize = (Invoke-DataQuery $connection "select count(*) Count from $TABLE").Count

    $sample = Invoke-DataQuery $connection "select label, image from $TABLE where rowid >= $($totalSize - $VALIDATION_SIZE + 1)"

    $image = ([byte[][]]$sample.Select({ $args[0].image })).Concatenate();
    $label = ([byte[][]]$sample.Select({ $args[0].label })).Concatenate();
    $data = Get-DataSource $image $label

    $Sampler.SetValidationData($data)

    # test data

    $exit = $false
    while (!$exit) {
      for ($i = 1; $i -le $totalSize - $VALIDATION_SIZE; ++$i) {
        $sample = Invoke-DataQuery $connection "select label, image from $TABLE where rowid = @id" @{ id = $i }

        $data = Get-DataSource $sample.image $sample.label

        $result = $Sampler.AddMinibatch($data)
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
# Model (ResNet)
############################################################

function New-ResNetNode {
  param(
    [CNTK.Variable]$In,
    [int]$OutDim,
    [bool]$Spatial
  )

  $WScale = 7.07

  $n = $In
  $n = cntk.conv $n (3, 3) $OutDim (cntk.glorotuniform $WScale -1 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial
  $n = cntk.relu $n

  $n = cntk.conv $n (3, 3) $OutDim (cntk.glorotuniform $WScale -1 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial

  $n = cntk.plus $n $In
  cntk.relu($n)
}

function New-ResNetNodeInc {
  param(
    [CNTK.Variable]$In,
    [int]$OutDim,
    [bool]$Spatial,
    [CNTK.Variable]$Projection
  )

  $WScale = 7.07

  $n = $In
  $n = cntk.conv $n (3, 3) $OutDim (cntk.glorotuniform $WScale -1 2) -Strides (2, 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial
  $n = cntk.relu $n

  $n = cntk.conv $n (3, 3) $OutDim (cntk.glorotuniform $WScale -1 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial

  $proj = New-ProjectLayer $Projection $In 2 2
  $n = cntk.plus $n $proj
  cntk.relu($n)
}

function New-ProjectLayer {
  param(
    [CNTK.Variable]$Projection,
    [CNTK.Variable]$In,
    [int]$HStride,
    [int]$VStride
  )

  $numInputChannels = $In.Shape[$In.Shape.Rank - 1]

  $n = $In
  $n = cntk.convolution $Projection $n ($HStride, $VStride, $numInputChannels) -AutoPadding $false
  cntk.batchnorm $n
}

function Get-ProjectionMap {
  param(
    [int]$OutputDim,
    [int]$InputDim
  )

  if ($inputDim -gt $outputDim) {
    throw "Can only project from lower to higher dimensionality"
  }

  $projectionMapValues = New-Object float[] ($InputDim * $OutputDim)

  for ($i = 0; $i -lt $inputDim; ++$i) {
    $projectionMapValues[$i * $InputDim + $i] = 1.0
  }

  cntk.constant (1, 1, $InputDim, $OutputDim) $projectionMapValues
}

function New-ResNetClassifier {
  param(
    [CNTK.Variable]$In,
    [int]$NumOutputClasses,
    [string]$OutputName
  )

  $n = $In

  ###### Layer 1

  $dim1 = 16

  $n = cntk.conv $n (3, 3) $dim1 (cntk.glorotuniform .26 -1 2)
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.relu $n

  $n = New-ResNetNode $n $dim1 $false
  $n = New-ResNetNode $n $dim1 $true
  $n = New-ResNetNode $n $dim1 $false

  ###### Layer 2

  $dim2 = 32

  $projection = Get-ProjectionMap $dim2 $dim1
  $n = New-ResNetNodeInc $n $dim2  $true $projection
  $n = New-ResNetNode $n $dim2 $false
  $n = New-ResNetNode $n $dim2 $true

  ###### Layer 3

  $dim3 = 64

  $projection = Get-ProjectionMap $dim3 $dim2
  $n = New-ResNetNodeInc $n $dim3 $true $projection
  $n = New-ResNetNode $n $dim3 $false
  $n = New-ResNetNode $n $dim3 $false

  ###### Global average pooling

  $n = cntk.averagepooling $n (8, 8, 1) (1, 1, 1)

  ##### Output DNN layer

  $n = cntk.dropout $n .5
  $n = cntk.dense $n $NumOutputClasses (cntk.glorotuniform .4 1 0) -Name $OutputName

  $n
}

############################################################
# Model (Simple CNN)
############################################################

function New-CNNClassifier {
  param(
    [CNTK.Variable]$In,
    [int]$NumOutputClasses,
    [string]$OutputName
  )

  $n = $In

  $dim = 16
  for ($i = 0; $i -lt 3; ++$i) {
    $n = cntk.conv $n (3, 3) $dim (cntk.glorotuniform)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n

    $n = cntk.conv $n (3, 3) $dim (cntk.glorotuniform)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n

    $n = cntk.maxpooling $n (3, 3) (2, 2)

    $dim *= 2
  }

  $n = cntk.dropout $n .5
  $n = cntk.dense $n $NumOutputClasses (cntk.glorotuniform) -Name $OutputName

  $n
}

# Build a model

$IMAGE_DIM = (32, 32, 3)
$NUM_CLASSES = 10

$in = cntk.input $IMAGE_DIM -Name "input"

#$out = New-ResNetClassifier $in $NUM_CLASSES "output"
$out = New-CNNClassifier $in $NUM_CLASSES "output"

$label = cntk.input $NUM_CLASSES -Name "label"

############################################################
# Training
############################################################

$MINIBATCH_SIZE = 50
$SAMPLE_COUNT_PER_EPOCH = 50000

#$learner = cntk.sgd $out .00781251
$learner = cntk.momentumsgd $out .005 .5

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

$sampler = cntk.parallelsampler -SampleCountPerEpoch $SAMPLE_COUNT_PER_EPOCH -QueueSize 1000

$runner = cntk.backgroundscriptrunner
$runner.Start($dataProducerScript, $sampler, $PSScriptRoot)

#$m = $sampler.GetNextBatch()
#$d = $m.Features["input"].data.ToDataSource().Transpose(2, 0, 1, 3, 4)
#0..49 | foreach { $d.Slice($_, ($_+1)).ToBitmap("RGB", $true) } | out-canvas
#mat $m.Features["label"].data.ToArray() -row 10 -transpose

try {
  cntk.starttraining $trainer $sampler -MaxIteration 50000 -ProgressOutputStep 500
}
finally {
  $sampler.CancelAdding()
  $null = $runner.Finish()
  $runner.Dispose()
}

$out.Save("$PSScriptRoot\cifar10.model")
