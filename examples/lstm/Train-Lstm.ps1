Set-StrictMode -Version Latest

Import-Module psmath
#Import-Module oxyplotcli

$SEQLEN = 100

############################################################
# Prepare data
############################################################

$seq = seq 0 5000 .1 -func { (math.sin $x) + (st.norm 0 .05).gen() }

oxyline -x $seq.x[0..100] -y $seq.y0[0..100] -markertype circle | show-oxyplot

$features = $seq.y0.Slice(@(0, -1))
$features = cntk.datasource $features 1, -1, 1
$features = $features.GetSubsequences($SEQLEN)

$labels = $seq.y0 | select -skip $SEQLEN
$labels = cntk.datasource $labels 1, 1, -1

$minibatchDef = cntk.minibatchdef @{ input = $features; labels = $labels } $SEQLEN .05

############################################################
# Build a model
############################################################

$in = cntk.input 1 -Name input

$n = $in
$n = cntk.rnnstack $n 300 1
$n = cntk.dense $n 1 (cntk.glorotuniform)
$out = $n

$label = cntk.input 1 -Name labels -DynamicAxes (cntk.axis.defaultbatch)

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label SquaredError SquaredError $learner

cntk.starttraining $trainer $minibatchDef -MaxIteration 10000 -ProgressOutputStep 100

$out.Save("$PSScriptRoot\lstm.model")
