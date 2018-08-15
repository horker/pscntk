Set-StrictMode -Version Latest

Import-Module psmath
Import-Module oxyplotcli

$SEQLEN = 100

$f = cntk.restore "$PSScriptRoot\lstm.model"

$initial = seq 0 ($SEQLEN * .1) .1 -func { math.sin $x }

$predicted = New-Object Collections.Generic.List[float]
$predicted.AddRange([float[]]$initial.y0)

$in = New-Object float[] $SEQLEN

for ($i = 0; $i -lt 5; ++$i) {

  $predicted.CopyTo(($predicted.Count - $SEQLEN), $in, 0, $SEQLEN)
  Write-Host (($in | foreach { $_.ToString(".##") }) -join " ")

  $output = $f.Invoke(@{ input = (cntk.datasource $in 1, $SEQLEN, 1) })

  $predicted.Add($output.ToArray()[0])
}

oxyline -x (0..($predicted.Count - 1)) -y $predicted | show-oxyplot
