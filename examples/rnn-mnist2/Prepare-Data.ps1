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

Write-Host "Saving..."
$set = cntk.datasourceset @{ input = $data; label = $label }
$set.Save($MNIST_DATA_FILE)

$train, $test = $set.Split(@(.8, .2))

#Write-CNTKTextFormat $train $MNIST_TRAIN_CTF -WithSequenceAxis
#Write-CNTKTextFormat $test $MNIST_TEST_CTF -WithSequenceAxis
