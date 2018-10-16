Set-StrictMode -Version Latest

Import-Module psmath
Import-Module oxyplotcli

$SEQLEN = 100

$f = cntk.restore "$PSScriptRoot\sinewave.model"

$initial = seq 0 ($SEQLEN * .1) .1 -func { math.sin $x }

$predicted = New-Object Collections.Generic.List[float]
$predicted.AddRange([float[]]$initial.y0)

$in = New-Object float[] $SEQLEN

for ($i = 0; $i -lt 500; ++$i) {

  $predicted.CopyTo(($predicted.Count - $SEQLEN), $in, 0, $SEQLEN)
#  Write-Host (($in | foreach { $_.ToString(".##") }) -join " ")

  $output = $f.Invoke(@{ input = $in })

  $predicted.Add($output.ToArray()[0])
}

$predicted = $predicted.ToArray()
$m = oxymodel
oxyline -x (0..99) -y $predicted.Slice(@(0, 100)) -Color Red -AddTo $m
oxyline -x (99..($predicted.Count - 1)) -y $predicted.Slice(@(99, 0)) -AddTo $m
$m | Show-OxyPlot
