Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$MNIST_DATA_FILE = "$PSScriptRoot\mnist_data.bin"

$OUT_CLASSES = 10

$IN_MODEL_FILE = "$PSScriptRoot\mnist.model"
$OUT_MODEL_FILE = "$PSScriptRoot\mnist_retrain.model"

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

$out = Import-CNTKFunction $IN_MODEL_FILE

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
    -MaxIteration 10000 `
    -ProgressOutputStep 500 `
    -LogFile "$PSScriptRoot\mnist_retrain.log"

$out.Save($OUT_MODEL_FILE)
