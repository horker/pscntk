Set-StrictMode -Version Latest

Import-Module psmath

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

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_seq2.bin"

$MNIST_TRAIN_CTF = "$PSScriptRoot\mnist_seq2_train.ctf"
$MNIST_TEST_CTF = "$PSScriptRoot\mnist_seq2_test.ctf"

$MNIST_TRAIN_MP = "$PSScriptRoot\mnist_seq2_train.msgpack"
$MNIST_TEST_MP = "$PSScriptRoot\mnist_seq2_test.msgpack"

$MINIBATCH_SIZE = 8

############################################################
# Prepraring data
############################################################

Write-Host "Loading MNIST image file..."
$data = [System.IO.File]::ReadAllBytes($MNIST_IMAGE_FILE)

Write-Host "Preprocessing images..."
$data = $data.Slice(@(16, 0)) # Skip header
$data = $data.Scale(0, 255, 0, 1.0)
$data = cntk.datasource $data (28, 28, -1)

Write-Host "Loading MNIST label file..."
$label = [System.IO.File]::ReadAllBytes($MNIST_LABEL_FILE)

Write-Host "Preprocessing labels..."
$label = $label.Slice(@(8, 0))
$label = $label.OneHot(10).ToFlatArray($true)
$label = cntk.datasource $label (10, 1, -1)

$set = cntk.datasourceset @{ input = $data; label = $label }

Write-Host "Saving..."

# Serialization

#$set.Save($MNIST_DATA_FILE)

#$train, $test = $set.Split(@(.8, .2))

#CTF
#Write-CNTKTextFormat $train $MNIST_TRAIN_CTF -WithSequenceAxis
#Write-CNTKTextFormat $test $MNIST_TEST_CTF -WithSequenceAxis

# MessagePack

$length = $data.Shape[-1]

if (Test-Path $MNIST_TRAIN_MP) {
    Remove-Item $MNIST_TRAIN_MP
}

for ($i = 0; $i -lt $length * .8; $i += $MINIBATCH_SIZE) {
    $slice = $set.Slice($i, $MINIBATCH_SIZE)
    Add-CNTKMsgPack $slice $MNIST_TRAIN_MP
}

if (Test-Path $MNIST_TEST_MP) {
    Remove-Item $MNIST_TEST_MP
}

for ($i; $i -lt $length; $i += $MINIBATCH_SIZE) {
    $size = [Math]::Min($MINIBATCH_SIZE, $length - $i)
    $slice = $set.Slice($i, $size)
    Add-CNTKMsgPack $slice $MNIST_TEST_MP
}
