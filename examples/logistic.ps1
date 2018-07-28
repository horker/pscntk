Set-StrictMode -Version Latest

Import-Module psmath

############################################################
# Prepare data
############################################################

$dataA = seq 1000 -NoSeq -func { (st.norm).gen() }, { (st.norm).gen() }, { "A" }
$dataB = seq 1000 -NoSeq -func { (st.norm 5).gen() }, { (st.norm 3).gen() }, { "B" }
$data = $dataA + $dataB

#$data | oxyscat -xname y0 -yname y1 -groupname y2 | show-oxyplot

$features = ds.fromrows $data.y0, $data.y1
$features.Reshape(2, 1, -1)

$l = pso.onehot $data.y2
$labels = ds.fromrows $l.A, $l.B
$labels.Reshape(2, 1, -1)

$minibatchDef = cntk.minibatchdef @{ input = $features; label = $labels } 20 .3

############################################################
# Build a model
############################################################

$HIDDEN_NODES = 2
$OUTPUT_CLASSES = 2

$in  = cntk.input 2 -Name "input"
$h   = cntk.dense $in $HIDDEN_NODES (cntk.henormal)
$h   = cntk.relu $in
$h   = cntk.dense $h $OUTPUT_CLASSES (cntk.glorotnormal)
$out = cntk.sigmoid $h

$label = cntk.input $OUTPUT_CLASSES -Name "label"

############################################################
# Start training
############################################################

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

cntk.starttraining $trainer $minibatchDef -MaxIteration 1000 -ProgressOutputStep 100

$trainer.Model().Save("$PSScriptRoot\logistic.dnn")
