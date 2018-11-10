Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_seq2.bin"

$MNIST_TRAIN_MP = "$PSScriptRoot\mnist_seq2_train.msgpack"
$MNIST_TEST_MP = "$PSScriptRoot\mnist_seq2_test.msgpack"

$MODEL_FILE = "$PSScriptRoot\mnist_seq2.model"
$LOG_FILE = "$PSScriptRoot\log\rnn-mnist2_$((Get-Date).ToString("yyyyMMddHHmmss")).log"

$OUT_CLASSES = 10
$FEATURE_DIM = 28
$CELL_DIM = 50

$MINIBATCH_SIZE = 8

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

Write-Host "Loading data..."

#$data = cntk.dataSourceSet -Path $MNIST_DATA_FILE
#$trainData, $testData = $data.Split(@(.8, .2))
#$sampler = cntk.sampler $trainData -MinibatchSize $MINIBATCH_SIZE
#$testSampler = cntk.sampler $testData -MinibatchSize 16

$sampler = cntk.msgPackSampler $MNIST_TRAIN_MP -MinibatchSize $MINIBATCH_SIZE -SampleCountPerEpoch (60000 * .8) -Randomize
$testSampler = cntk.msgPackSampler $MNIST_TEST_MP -MinibatchSize 256 -SampleCountPerEpoch (60000 * .2)

############################################################
# Model
############################################################

Write-Host "Building model..."

function Get-Model($in) {
    $initialState = (cntk.normalRandom $CELL_DIM).Invoke()[0]

    $n = $in
    $n = cntk.dense $n $CELL_DIM -Stabilize
    $n = cntk.gru $n -ReturnSequences -InitialState $initialState -Stabilize -DropoutRate .2
    $n = cntk.gru $n -Stabilize -DropoutRate .2
    $n = cntk.dense $n $OUT_CLASSES -Stabilize
    $n
}

$in = cntk.input $FEATURE_DIM -Name input -WithSequenceAxis
$out = Get-Model $in

$label = cntk.input $OUT_CLASSES -Name label

############################################################
# Training
############################################################

Write-Host "Training started..."
Write-Host "Log file: $(Split-Path -Leaf $LOG_FILE)"

$learner = cntk.adam $out .001 -GradientClippingThresholdPerSample 1

cntk.starttraining `
    $out `
    (cntk.crossEntropyWithSoftmax $out $label) `
    (cntk.classificationError $out $label) `
    $learner `
    $sampler `
    $testSampler `
    -MaxIteration 50000 `
    -ProgressOutputStep 1000 `
    -LogFile $LOG_FILE

Write-Host "Saving model..."

$out.Save($MODEL_FILE)
