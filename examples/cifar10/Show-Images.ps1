Set-StrictMode -Version Latest

Import-Module psmath

$FILE = "$PSScriptRoot\cifar10_test.txt"
$MINIBATCH_SIZE = 100

$sampler = cntk.ctfsampler $FILE $MINIBATCH_SIZE $false

$batch = $sampler.GetNextBatch()

$images = $batch["input"].data.ToDataSource()
$images.Reshape(32, 32, 3, $MINIBATCH_SIZE)
$images = $images.Transpose(2, 0, 1, 3)

$bitmaps = 0..($MINIBATCH_SIZE - 1) | foreach {
  $images.Slice($_, ($_ + 1)).ToBitmap("RGB", $true)
}

$bitmaps | out-canvas

$labels = $batch["label"].data.ToDataSource()
$labels.Reshape(10, $MINIBATCH_SIZE)

$l = New-Object double[] $MINIBATCH_SIZE
for ($i = 0; $i -lt $MINIBATCH_SIZE; ++$i) {
  $l[$i] = $labels.Slice($i, ($i + 1)).ToArray().Argmax()
}

mat $l -Columns 10 -Transpose
