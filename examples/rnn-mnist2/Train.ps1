Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_seq2.bin"

$MODEL_FILE = "$PSScriptRoot\mnist_seq2.model"
$LOG_FILE = "$PSScriptRoot\rnn-mnist2_$((Get-Date).ToString("yyyyMMddHHmmss")).log"

$OUT_CLASSES = 10
$FEATURE_DIM = 28
$CELL_DIM = 50

$MINIBATCH_SIZE = 8

Set-CNTKRandomSeed 1234

############################################################
# Data
############################################################

Write-Host "Loading data..."

$data = cntk.dataSourceSet -Path $MNIST_DATA_FILE
$trainData, $testData = $data.Split(@(.8, .2))
$sampler = cntk.sampler $trainData -MinibatchSize $MINIBATCH_SIZE
$testSampler = cntk.sampler $testData -MinibatchSize 16

############################################################
# Model
############################################################

Write-Host "Building model..."

function Get-Model($in) {
    $initialState = (cntk.normalRandom $CELL_DIM).Invoke()[0]

    $n = $in
    $n = cntk.dense $n $CELL_DIM -Stabilize
    $n = cntk.gru $n -ReturnSequences -InitialState $initialState -Stabilize
    $n = cntk.gru $n -Stabilize
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
    -MaxIteration 20000 `
    -ProgressOutputStep 1000 `
    -LogFile $LOG_FILE

Write-Host "Saving model..."

$out.Save($MODEL_FILE)
