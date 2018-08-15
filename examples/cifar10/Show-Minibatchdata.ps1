$m = idq "$PSScriptRoot\cifar10.sqlite" "select image from minibatches limit 1"

$d = cntk.datasource -DataType byte $m.image (32, 32, 3, -1)
$d = $d.Transpose(2, 0, 1, 3)

seq 0 50 | foreach {
  $s = $d.Slice($_, ($_ + 1))
  $s.ToBitmap("RGB", $false)
} | out-canvas
