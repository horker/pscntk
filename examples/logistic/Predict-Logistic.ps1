Set-StrictMode -Version latest

Import-Module psmath
Import-Module oxyplotcli

$model = cntk.restore "$PSScriptRoot\logistic.model"

$data = seq 1000 -NoSeq -func { (st.uniform 0 2).gen() }, { (st.uniform 0 4).gen() }

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
