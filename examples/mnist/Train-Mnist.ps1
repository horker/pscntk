param(
  [switch]$UseConv
)

Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_CACHE_FILE = "$PSScriptRoot\mnist_data.bin"

$OUT_CLASSES = 10

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

$data = cntk.datasourceset -Path $MNIST_CACHE_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize 64
$testSampler = cntk.sampler $testData -MinibatchSize 64

############################################################
# Model
############################################################

$in = cntk.input (28, 28, 1) -Name input
$n = $in

if ($UseConv) {
  # conv1: 28 x 28 x 1 -> 28 x 28 x 8 -> 14 x 14 x 8
  $n = cntk.conv2d $n (3, 3) 8 (1, 1) $true (cntk.init.truncatednormal .1) relu
  $n = cntk.maxpooling $n (3, 3) (2, 2)

  # conv2: 14 x 14 x 8 -> 14 x 14 x 32 -> 7 x 7 x 32
  $n = cntk.conv2d $n (3, 3) 32 (1, 1) $true (cntk.init.truncatednormal .1) relu
  $n = cntk.maxpooling $n (3, 3) (2, 2)

  # fc
  $n = cntk.dense $n $OUT_CLASSES (cntk.init.glorotuniform)
}
else {
  $n = cntk.dense $n 1000 (cntk.init.heuniform) relu
  $n = cntk.dense $n 1000 (cntk.init.heuniform) relu
  $n = cntk.dense $n 1000 (cntk.init.heuniform) relu
  $n = cntk.dense $n $OUT_CLASSES (cntk.init.glorotuniform)
}

$out = $n

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .1 .9

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

cntk.starttraining $trainer $sampler $testSampler -MaxIteration 10000 -ProgressOutputStep 500 -LogFile "$PSScriptRoot\mnist_log.log"
