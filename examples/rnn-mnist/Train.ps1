Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_CACHE_FILE = "$PSScriptRoot\mnist_seq.bin"

$OUT_CLASSES = 10
$IMAGE_SIZE = 28 * 28

$CELL_DIM = 200

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

Write-Host "Loading data..."

$data = cntk.datasourceset -Path $MNIST_CACHE_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize 16
$testSampler = cntk.sampler $testData -MinibatchSize 16

############################################################
# Model
############################################################

Write-Host "Building model..."

function Get-Model($in) {
    $values = cntk.constant $CELL_DIM (cntk.normalrandom $CELL_DIM -DataType float).Invoke().ToArray()

    $n = $in
    $n = cntk.dense $n $CELL_DIM
    $n = cntk.dropout $n .5
    $n = cntk.gru $n -ReturnSequences -InitialState $values
    $n = cntk.batchnorm $n -InitialScale .01
    $n = cntk.dropout $n .5
    $n = cntk.gru $n
    $n = cntk.batchnorm $n -InitialScale .01
    $n = cntk.dense $n $OUT_CLASSES
    $n
}

$in = cntk.input 1 -Name input -WithSequenceAxis
$out = Get-Model $in

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

Write-Host "Training started..."

$learner = cntk.adam $out .001 -GradientClippingThresholdPerSample 1 -GradientClippingWithTruncation $true

$trainer = cntk.trainer $out $label CrossEntropyWithSoftmax ClassificationError $learner

cntk.starttraining $trainer $sampler $testSampler -MaxIteration 10000 -ProgressOutputStep 500 -LogFile "$PSScriptRoot\mnist_log.log"

Write-Host "Saving model..."

$out.Save("$PSScriptRoot\mnist_seq.model")
