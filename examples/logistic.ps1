Set-StrictMode -Version Latest

Import-Module psmath

############################################################
# Prepare data
############################################################

$dataA = seq 1000 -NoSeq -func { (st.norm).gen() }, { (st.norm).gen() }, { "A" }
$dataB = seq 1000 -NoSeq -func { (st.norm 5).gen() }, { (st.norm 3).gen() }, { "B" }
$data = $dataA + $dataB

#$data | oxyscat -xname y0 -yname y1 -groupname y2 | show-oxyplot

$feature = ds.fromrows $data.y0, $data.y1
$feature.Reshape(2, 1, -1)

$trainFeature, $validateFeature = $feature.Split(.8)

$l = pso.onehot $data.y2
$label = ds.fromrows $l.A, $l.B
$label.Reshape(2, 1, -1)

$trainLabel, $validateLabel = $label.Split(.8)

$trainingData = cntk.minibatchsource @{ input = $trainFeature; label = $trainLabel }

$validateData = new-object "Collections.Generic.Dictionary[object, CNTK.Value]"
$validateData.Add("input", $validateFeature.ToValue())
$validateData.Add("label", $validateLabel.ToValue())

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

cntk.starttraining $trainer $trainingData $validateData 20 -MaxIteration 1000 -ProgressOutputStep 1

$out.Save("$PSScriptRoot\logistic.dnn")
