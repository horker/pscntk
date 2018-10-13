param(
  [switch]$UseResNet
)

Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$TRAIN_FILE = "$PSScriptRoot\cifar10_train.txt"
$TEST_FILE = "$PSScriptRoot\cifar10_test.txt"

$MINIBATCH_SIZE = 50
$VALIDATION_SIZE = 1000

$IMAGE_DIM = (32, 32, 3)
$OUT_CLASSES = 10

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
  $n = cntk.conv2d $n (3, 3) $OutDim (1, 1) $true (cntk.glorotuniform $WScale -1 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial
  $n = cntk.relu $n

  $n = cntk.conv2d $n (3, 3) $OutDim (1, 1) $true (cntk.glorotuniform $WScale -1 2)
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
  $n = cntk.conv2d $n (3, 3) $OutDim (2, 2) $true (cntk.glorotuniform $WScale -1 2)
  $n = cntk.batchnorm $n -Spatial:$Spatial
  $n = cntk.relu $n

  $n = cntk.conv2d $n (3, 3) $OutDim (1, 1) $true (cntk.glorotuniform $WScale -1 2)
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
    [CNTK.Variable]$In
  )

  $n = $In

  ###### Layer 1

  $dim1 = 16

  $n = cntk.conv2d $n (3, 3) $dim1 (1, 1) $true (cntk.glorotuniform .26 -1 2)
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
  $n = cntk.dense $n $OUT_CLASSES (cntk.glorotuniform .4 1 0) -Name "output"

  $n
}

############################################################
# Model (Simple CNN)
############################################################

function New-CNNClassifier {
  param(
    [CNTK.Variable]$In
  )

  $n = $In

  $dim = 16
  for ($i = 0; $i -lt 1; ++$i) {
    $n = cntk.conv2d $n (3, 3) $dim (1, 1) $true (cntk.glorotuniform)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n

    $n = cntk.conv2d $n (3, 3) $dim (1, 1) $true (cntk.glorotuniform)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n

    $n = cntk.maxpooling $n (3, 3) (2, 2)

    $dim *= 2
  }

  $n = cntk.dropout $n .5
  $n = cntk.dense $n $OUT_CLASSES (cntk.glorotuniform) -Name "output"

  $n
}

# Build a model

$in = cntk.input $IMAGE_DIM -Name "input"

if ($UseResNet) {
  $out = New-ResNetClassifier $in
}
else {
  $out = New-CNNClassifier $in
}

$label = cntk.input $OUT_CLASSES -Name "label"

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .01 .7

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

$sampler = cntk.ctfsampler $TRAIN_FILE $MINIBATCH_SIZE

$testSampler = cntk.ctfsampler $TEST_FILE $VALIDATION_SIZE

cntk.starttraining $trainer $sampler $testSampler -MaxIteration 50000 -ProgressOutputStep 500

$out.Save("$PSScriptRoot\cifar10.model")
