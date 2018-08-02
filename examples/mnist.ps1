param(
  [switch]$Conv
)

Set-StrictMode -Version Latest

# MNIST
# url: http://yann.lecun.com/exdb/mnist/

$MNIST_IMAGE_FILE = "$PSScriptRoot\..\data\MNIST\train-images.idx3-ubyte"
$MNIST_LABEL_FILE = "$PSScriptRoot\..\data\MNIST\train-labels.idx1-ubyte"

$MNIST_CACHE_FILE = "$PSScriptRoot\mnist_minibatchdef.bin"

############################################################
# Prepraring data
############################################################

if (Test-Path $MNIST_CACHE_FILE) {
  $minibatchDef = cntk.minibatchdef -Path $MNIST_CACHE_FILE
}
else {
  Write-Host "Loading MNIST image file..."
  $data = [System.IO.File]::ReadAllBytes($MNIST_IMAGE_FILE)

  Write-Host "Preprocessing images..."
  $data = $data.Slice(@(16, 0)) # Skip header
  $data = $data.Scale(0, 255, 0, 1.0)
  $data = ds $data 28, 28, 1, 1, -1

  Write-Host "Loading MNIST label file..."
  $label = [System.IO.File]::ReadAllBytes($MNIST_LABEL_FILE)

  Write-Host "Preprocessing labels..."
  $label = $label.Slice(@(8, 0))
  $label = (mat $label).OneHot(10).ToFlatArray($true)
  $label = ds $label 10, 1, -1

  Write-Host "Saving..."
  $minibatchDef = cntk.minibatchdef @{ input = $data; label = $label }
  $minibatchDef.Save($MNIST_CACHE_FILE)
}

$minibatchDef.MinibatchSize = 64
$minibatchDef.ValidationRate = .2

############################################################
# Model
############################################################

# convolutionMap = cntk.parameter kernelWidth, kernelHeight, numInputChannels, featureMapCount initializer
# cntk.convolution convolutionMasp input Strides(hStrides, vStrides, numInputChannels)
#
# cntk.pooling input PoolingType PoolingWindowShape(windowWidth, windowHeight) Strides(hStrides, vStrides) AutoPadding

$OUT_CLASS = 10

$in = cntk.input 28, 28, 1 -Name input
$n = $in

if ($Conv) {
  # conv1: 28 x 28 x 1 -> 28 x 28 x 8
  $w = cntk.parameter 3, 3, 1, 8 (cntk.truncatednormal .1)
  $n = cntk.convolution -ConvolutionMap $w -Operand $n -Strides 1, 1, 1
  $n = cntk.relu $n

  # pool1: 28 x 28 x 8 -> 14 x 14 x 8
  $n = cntk.pooling -Operand $n -PoolingType Max -PoolingWindowShape 3, 3 -Strides 2, 2 -AutoPadding $true

  # conv2: 14 x 14 x 8 -> 14 x 14 x 32
  $w = cntk.parameter 3, 3, 8, 32 (cntk.truncatednormal .1)
  $n = cntk.convolution $w $n 1, 1, 8
  $n = cntk.relu $n

  # pool2: 14 x 14 x 32 -> 7 x 7 x 32
  $n = cntk.pooling $n Max (3, 3) (2, 2) $true

  # fc
  $n = cntk.dense $n $OUT_CLASS (cntk.glorotuniform .1)
}
else {
  $n = cntk.dense $n 1000 (cntk.heuniform .1)
  $n = cntk.relu $n

  $n = cntk.dense $n 1000 (cntk.heuniform .1)
  $n = cntk.relu $n

  $n = cntk.dense $n 1000 (cntk.heuniform .1)
  $n = cntk.relu $n

  $n = cntk.dense $n $OUT_CLASS (cntk.glorotuniform .1)
}

$out = $n
$label = cntk.input $OUT_CLASS -Name label

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .1 .9

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

cntk.starttraining $trainer $minibatchDef -MaxIteration 50000 -ProgressOutputStep 1000
