Set-StrictMode -Version latest

Import-Module psmath
Import-Module oxyplotcli

$model = cntk.restore "$PSScriptRoot\moon.model"

$data = seq 1000 -NoSeq -func { (st.uniform -.5 1).gen() }, { (st.uniform -1 2).gen() }

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
