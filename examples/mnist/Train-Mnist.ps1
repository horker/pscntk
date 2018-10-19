param(
    [switch]$UseConv
)

Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_data.bin"

$OUT_CLASSES = 10

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

$data = cntk.datasourceset -Path $MNIST_DATA_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize 64
$testSampler = cntk.sampler $testData -MinibatchSize 64 -NoRandomize

############################################################
# Model
############################################################

$in = cntk.input (28, 28, 1) -Name input
$n = $in

if ($UseConv) {
    # conv1: 28 x 28 x 1 -> 28 x 28 x 32 -> 14 x 14 x 32
    $n = cntk.conv2d $n (5, 5) 32 (1, 1) $true (cntk.init.truncatednormal .1)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n
    $n = cntk.maxpooling $n (3, 3) (2, 2)

    # conv2: 14 x 14 x 32 -> 14 x 14 x 64 -> 7 x 7 x 64
    $n = cntk.conv2d $n (5, 5) 64 (1, 1) $true (cntk.init.truncatednormal .1)
    $n = cntk.batchnorm $n
    $n = cntk.relu $n
    $n = cntk.maxpooling $n (3, 3) (2, 2)

    # fc
    $n = cntk.dense $n $OUT_CLASSES (cntk.init.glorotuniform)
}
else {
    $n = cntk.dense $n 64 (cntk.init.heuniform) relu
    $n = cntk.dense $n $OUT_CLASSES (cntk.init.glorotuniform)
}

$out = $n

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

cntk.starttraining `
    -Model $out `
    -LossFunction (cntk.crossEntropyWithSoftmax $out $label) `
    -EvaluationFunction (cntk.classificationError $out $label) `
    -Learner (cntk.adam $out .1) `
    -Sampler $sampler `
    -ValidationSampler $testSampler `
    -MaxIteration 20000 `
    -ProgressOutputStep 500 `
    -LogFile "$PSScriptRoot\mnist_log.log"
