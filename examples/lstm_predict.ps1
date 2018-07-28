Set-StrictMode -Version Latest

Import-Module psmath
#Import-Module oxyplotcli

$f = cntk.restore "$PSScriptRoot\lstm.cntkmodel"

$seq = seq 0 1000 .5 -func { (math.sin $x) * (math.abs (math.sin ($x / 100))) }
$seq = $seq.y0

$result = New-Object Collections.Generic.List[float]

for ($i = 0; $i -lt $seq.Length - 20; ++$i) {
  $subseq = $seq.Slice(@($i, ($i + 20)))

  $output = $f.Invoke(@{ input = (ds $subseq 1, 20, 1) })

  $result.Add($output.ToArray()[0])
}

oxyline -x (0..($seq.Length - 20)) -y $result | show-oxyplot
