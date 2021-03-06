Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_seq.bin"

$MNIST_TRAIN_FILE = "$PSScriptRoot\mnist_seq_train.ctf"
$MNIST_TEST_FILE = "$PSScriptRoot\mnist_seq_test.ctf"

$MNIST_TRAIN_MP = "$PSScriptRoot\mnist_seq_train.msgpack"
$MNIST_TEST_MP = "$PSScriptRoot\mnist_seq_test.msgpack"

$OUT_CLASSES = 10
$IMAGE_SIZE = 28 * 28

$CELL_DIM = 100

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

Write-Host "Loading data..."

$data = cntk.datasourceset -Path $MNIST_DATA_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize 8
$testSampler = cntk.sampler $testData -MinibatchSize 8

#$sampler = cntk.ctfsampler $MNIST_TRAIN_FILE -MinibatchSize 16
#$testSampler = cntk.ctfsampler $MNIST_TEST_FILE -MinibatchSize 16 -NoRandomize

#$sampler = cntk.msgPackSampler $MNIST_TRAIN_MP (60000 * .8) 100 -TimeoutForAdd (60 * 60 * 1000)
#$testSampler = cntk.msgPackSampler $MNIST_TEST_MP (60000 * .2) 100 -TimeoutForAdd (60 * 60 * 1000)

############################################################
# Model
############################################################

Write-Host "Building model..."

function Get-Model($in) {
    $values = cntk.constant $CELL_DIM (cntk.normalRandom $CELL_DIM).Invoke().ToArray()

    $n = $in
    $n = cntk.dense $n $CELL_DIM
    $n = cntk.dropout $n .5
    $n = cntk.gru $n -ReturnSequences -InitialState $values
    $n = cntk.dropout $n .5
    $n = cntk.gru $n
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

$learner = cntk.adam $out .001 -GradientClippingThresholdPerSample 1

cntk.starttraining `
    $out `
    (cntk.crossEntropyWithSoftmax $out $label) `
    (cntk.classificationError $out $label) `
    $learner `
    $sampler `
    $testSampler `
    -MaxIteration 20000 `
    -ProgressOutputStep 100 `
    -LogFile "$PSScriptRoot\rnn-mnist.log"

Write-Host "Saving model..."

$out.Save("$PSScriptRoot\mnist_seq.model")
