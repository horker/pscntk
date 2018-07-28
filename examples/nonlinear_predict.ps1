Set-StrictMode -Version latest

Import-Module psmath

$model = cntk.restore "$PSScriptRoot\nonlinear.cntkmodel"

$data = seq 1000 -NoSeq -func { (st.unif -.5 1.5).gen() }, { (st.unif -1.5 2.5).gen() }

$xy = $data | foreach {
  $d = $_

  $class = $model.Invoke(@{ "input" = ($d.y0, $d.y1) }).ToArray() | math.softmax

  if ($class[0] -gt $class[1]) {
    $c = "A"
  }
  else {
    $c = "B"
  }
  [PSCustomObject]@{ x = $d.y0; y = $d.y1; class = $c }
}

$xy | oxyscat -xname x -yname y -groupname class | show-oxyplot
