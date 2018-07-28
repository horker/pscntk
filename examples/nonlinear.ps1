Set-StrictMode -Version latest

Import-Module psmath
#Import-Module oxyplotcli

############################################################
# Prepare data
############################################################

$dataA = seq 0 (math.pi) 0.01 -func { (math.sin $x) + (st.norm 0 .1).gen() }, { (math.cos $x) + (st.norm 0 .1).gen() }, { "A" }
$dataB = seq (math.pi) (2 * (math.pi)) 0.01 -func { .7 + (math.sin $x) + (st.norm 0 .1).gen() }, { 1 + (math.cos $x) + (st.norm 0 .1).gen() }, { "B" }
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

$INPUT_DIM = 2
$OUTPUT_CLASSES = 2
$HIDDEN_NODES = 100

$in  = cntk.input $INPUT_DIM -Name "input"

$n = $in
$n = cntk.dense $n $HIDDEN_NODES (cntk.henormal)
$n = cntk.relu $n
$n = cntk.dense $n $OUTPUT_CLASSES (cntk.glorotnormal)
$n = cntk.sigmoid $n
$out = $n

$label = cntk.input $OUTPUT_CLASSES -Name "label"

############################################################
# Start training
############################################################

$learner = cntk.momentumsgd $out .01 .5

$trainer = cntk.trainer $out $label BinaryCrossEntropy ClassificationError $learner

cntk.starttraining $trainer $minibatchdef -MaxIteration 10000 -ProgressOutputStep 100

$out.Save("$PSScriptRoot\nonlinear.cntkmodel")
