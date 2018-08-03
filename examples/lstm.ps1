Set-StrictMode -Version Latest

#Import-Module psmath
#Import-Module oxyplotcli

############################################################
# Prepare data
############################################################

$seq = seq 0 1000 .5 -func { (math.sin $x) * (math.abs (math.sin ($x / 100))) + (st.norm 0 .01).gen() }

#oxyline -x $seq.x -y $seq.y0 | show-oxyplot

$features = $seq.y0.Slice(@(0,-1))
$features = cntk.datasource $features 1, -1, 1
$features = $features.GetSubsequences(20)

$labels = $seq.y0 | select -skip 20
$labels = cntk.datasource $labels 1, 1, -1

$minibatchDef = cntk.minibatchdef @{ input = $features; labels = $labels } 20 .7

############################################################
# Build a model
############################################################

$in = cntk.input 1 -Name input
$h = $in
$h = cntk.rnnstack $h 100 1
$h = cntk.dense $h 1 (cntk.glorotuniform)
$out = $h

$label = cntk.input 1 -Name labels -DynamicAxes @([CNTK.Axis]::DefaultBatchAxis())

############################################################
# Training
############################################################

$learner = cntk.momentumsgd $out .01 .9

$trainer = cntk.trainer $out $label SquaredError SquaredError $learner

cntk.starttraining $trainer $minibatchDef -MaxIteration 5000 -ProgressOutputStep 100

$out.Save("$PSScriptRoot\lstm.cntkmodel")
