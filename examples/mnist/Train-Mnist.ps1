param(
  [switch]$UseConv
)

Set-StrictMode -Version Latest

############################################################
# MNIST data files
############################################################

# Before running this program, do the following:
#
# (1) Download the MNIST data files from the following web site:
#       url: http://yann.lecun.com/exdb/mnist/
#       files: train-images-idx3-ubyte.gz train-labels-idx1-ubytes.gz
#
# (2) Unzip them and place the files in <project dir>\data\MNIST

$MNIST_IMAGE_FILE = "$PSScriptRoot\..\..\data\MNIST\train-images.idx3-ubyte"
$MNIST_LABEL_FILE = "$PSScriptRoot\..\..\data\MNIST\train-labels.idx1-ubyte"

$MNIST_CACHE_FILE = "$PSScriptRoot\mnist_data.bin"

############################################################
# Prepraring data
############################################################

if (Test-Path $MNIST_CACHE_FILE) {
  $sampler = cntk.sampler -Path $MNIST_CACHE_FILE
}
else {
  Write-Host "Loading MNIST image file..."
  $data = [System.IO.File]::ReadAllBytes($MNIST_IMAGE_FILE)

  Write-Host "Preprocessing images..."
  $data = $data.Slice(@(16, 0)) # Skip header
  $data = $data.Scale(0, 255, 0, 1.0)
  $data = cntk.datasource $data (28, 28, 1, 1, -1)

  Write-Host "Loading MNIST label file..."
  $label = [System.IO.File]::ReadAllBytes($MNIST_LABEL_FILE)

  Write-Host "Preprocessing labels..."
  $label = $label.Slice(@(8, 0))
  $label = $label.OneHot(10).ToFlatArray($true)
  $label = cntk.datasource $label (10, 1, -1)

  Write-Host "Saving..."
  $sampler = cntk.sampler @{ input = $data; label = $label }
  $sampler.Save($MNIST_CACHE_FILE)
}

$sampler.MinibatchSize = 64
$sampler.ValidationRate = .2

############################################################
# Model
############################################################

$OUT_CLASSES = 10

$in = cntk.input (28, 28, 1) -Name input
$n = $in

if ($UseConv) {
  # conv1: 28 x 28 x 1 -> 28 x 28 x 8 -> 14 x 14 x 8
  $n = cntk.conv2d $n (3, 3) 8 (1, 1) (cntk.truncatednormal .1) relu -Padding $true
  $n = cntk.maxpooling $n (3, 3) (2, 2)

  # conv2: 14 x 14 x 8 -> 14 x 14 x 32 -> 7 x 7 x 32
  $n = cntk.conv2d $n (3, 3) 32 (1, 1) (cntk.truncatednormal .1) relu -Padding $true
  $n = cntk.maxpooling $n (3, 3) (2, 2)

  # fc
  $n = cntk.dense $n $OUT_CLASSES (cntk.glorotuniform)
}
else {
  $n = cntk.dense $n 1000 (cntk.heuniform) relu
  $n = cntk.dense $n 1000 (cntk.heuniform) relu
  $n = cntk.dense $n 1000 (cntk.heuniform) relu
  $n = cntk.dense $n $OUT_CLASSES (cntk.glorotuniform)
}

$out = $n

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .1 .9

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

cntk.starttraining $trainer $sampler -MaxIteration 10000 -ProgressOutputStep 500
