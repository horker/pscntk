Describe "Invoke()" {

  It "can evaluate the function" {

    $i = cntk.input 3
    $f = cntk.reducesum $i -Axis ([CNTK.Axis]::AllStaticAxes())

    $result = $f.Invoke(@{ $i = (1,2,3) })
    $ds = [Horker.PSCNTK.DataSource[float]]::FromValue($result)

    $ds.Shape.Dimensions | Should -be @(1, 1)
    $ds.Data | Should -Be @(6)
  }

}

Describe "Find()" {

  It "can obtain input variable by name" {

    $i = cntk.input 3 -Name "input"
    $f = cntk.reducesum $i -Axis ([CNTK.Axis]::AllStaticAxes())

    $in = $f.Find("input")

    $in | Should -beExactly $i
  }

}
