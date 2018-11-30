param(
    [switch]$UseConv
)

Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_data.bin"

$OUT_CLASSES = 10

$MODEL_FILE = "$PSScriptRoot\models\mnist.model"
$LOG_FILE = "$PSScriptRoot\mnist_log.log"

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

$data = cntk.datasourceset -Path $MNIST_DATA_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize 64 -Randomize
$testSampler = cntk.sampler $testData -MinibatchSize 64

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
    $n = cntk.dense $n $OUT_CLASSES
}
else {
    $n = cntk.dense $n 64 (cntk.init.heuniform) relu
    $n = cntk.dense $n $OUT_CLASSES
}

$out = $n

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

$logger = cntk.logger $LOG_FILE
$saver = cntk.scriptCallback -Step 10000 {
    param($session)
    $session.Trainer.Model().Save("$PSScriptRoot\models\mnist$($session.Iteration).model")
}

cntk.starttraining `
    -Model $out `
    -LossFunction (cntk.crossEntropyWithSoftmax $out $label) `
    -EvaluationFunction (cntk.classificationError $out $label) `
    -Learner (cntk.momentumSgd $out 1) `
    -LearningScheduler (cntk.scheduler.performance 1 .9 500) `
    -Sampler $sampler `
    -ValidationSampler $testSampler `
    -MaxIteration 50000 `
    -ProgressOutputStep 500 `
    -Logger $logger `
    -Callbacks $saver

$out.Save($MODEL_FILE)
$logger.Close()
