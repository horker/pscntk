Set-StrictMode -Version Latest

Import-Module psmath
#Import-Module oxyplotcli

$SEQLEN = 100

############################################################
# Prepare data
############################################################

Write-Host "Preparing data..."

$seq = seq 0 5000 .1 -func { (math.sin $x) + (st.normal 0 .05).gen() }

#oxyline -x $seq.x[0..200] -y $seq.y0[0..200] -markertype circle | show-oxyplot

$features = $seq.y0.Slice(@(0, -1))
$features = cntk.datasource $features 1, -1, 1
$features = $features.GetSubsequences($SEQLEN)

$labels = $seq.y0 | select -skip $SEQLEN
$labels = cntk.datasource $labels 1, 1, -1

$dataSet = cntk.datasourceset @{ input = $features; labels = $labels }
$trainData, $testData = $dataSet.Split(.8, .2)
$sampler = cntk.sampler $trainData -MinibatchSize 32 -WithSequenceAxis
$testSampler = cntk.sampler $testData -MinibatchSize 100 -WithSequenceAxis

############################################################
# Build a model
############################################################

Write-Host "Building model..."

$in = cntk.input 1 -Name input -WithSequenceAxis

$n = $in
$n = cntk.lstm $n 30 30
$n = cntk.dense $n 1 (cntk.glorotuniform)
$out = $n

$label = cntk.input 1 -Name labels

############################################################
# Training
############################################################

Write-Host "Training started..."

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label SquaredError SquaredError $learner

cntk.starttraining $trainer $sampler $testSampler -MaxIteration 1000 -ProgressOutputStep 100

$out.Save("$PSScriptRoot\sinewave.model")
