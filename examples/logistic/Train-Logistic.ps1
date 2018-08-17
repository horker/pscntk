Set-StrictMode -Version Latest

Import-Module psmath
#Import-Module oxyplotcli

############################################################
# Prepare data
############################################################

$dataA = seq 1000 -NoSeq -func { (st.normal).gen() }, { (st.normal).gen() }, { "A" }
$dataB = seq 1000 -NoSeq -func { (st.normal 5).gen() }, { (st.normal 3).gen() }, { "B" }
$data = $dataA + $dataB

#$data | oxyscat -xname y0 -yname y1 -groupname y2 | show-oxyplot

$features = cntk.datasource -Rows $data.y0, $data.y1
$features.Reshape(2, 1, -1)

$l = pso.onehot $data.y2
$labels = cntk.datasource -Rows $l.A, $l.B
$labels.Reshape(2, 1, -1)

$sampler = cntk.sampler @{ input = $features; label = $labels } 20 .3

############################################################
# Build a model
############################################################

$HIDDEN_NODES = 2
$OUTPUT_CLASSES = 2

$in = cntk.input 2 -Name "input"
$out = cntk.dense $in $OUTPUT_CLASSES (cntk.glorotnormal) sigmoid

$label = cntk.input $OUTPUT_CLASSES -Name "label"

############################################################
# Start training
############################################################

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

cntk.starttraining $trainer $sampler -MaxIteration 1000 -ProgressOutputStep 100

$out.Save("$PSScriptRoot\logistic.model")
