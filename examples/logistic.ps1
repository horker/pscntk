Set-StrictMode -Version Latest

Import-Module psmath

############################################################
# Prepare data
############################################################

$dataA = seq 1000 -NoSeq -func { (st.norm).gen() }, { (st.norm).gen() }, { "A" }
$dataB = seq 1000 -NoSeq -func { (st.norm 5).gen() }, { (st.norm 3).gen() }, { "B" }
$data = $dataA + $dataB

#$data | oxyscat -xname y0 -yname y1 -groupname y2 | show-oxyplot

$train = ds.fromrows $data.y0, $data.y1
$train.Reshape(2, 1, -1)

$l = pso.onehot $data.y2
$label = ds.fromrows $l.A, $l.B
$label.Reshape(2, 1, -1)

$source = cntk.minibatchsource @{ input = $train; label = $label }

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
# Prepare data
############################################################

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

cntk.starttraining $trainer $source 20 -MaxIteration 1000 -ProgressOutputStep 50
