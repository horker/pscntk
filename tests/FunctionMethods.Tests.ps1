Describe "Invoke()" {

  It "can take arguments by variable object and array literal" {

    $i = cntk.input 3
    $f = cntk.reducesum $i -Axis (cntk.axis.allstatic)

    $result = $f.Invoke(@{ $i = (1,2,3) }).ToDataSource()

    $result.Shape.Dimensions | Should -be @(1, 1)
    $result.Data | Should -Be @(6)
  }

  It "can take arguments by variable name and array literal" {

    $i = cntk.input 3 -Name "input"
    $f = cntk.reducesum $i -Axis (cntk.axis.allstatic)

    $result = $f.Invoke(@{ "input" = (1,2,3) }).ToDataSource()

    $result.Shape.Dimensions | Should -be @(1, 1)
    $result.Data | Should -Be @(6)
  }


  It "can take arguments by minibatch" {

    $i = cntk.input 3 -Name "input"
    $f = cntk.reducesum $i -Axis (cntk.axis.allstatic)

    $s = cntk.noisesampler -Name "input" -Shape 3 -Min -1 -Max 1 -MinibatchSize 1
    $b = $s.GetNextBatch()

    $result = $f.Invoke($b).ToDataSource()

    $result.Shape.Dimensions | Should -be @(1, 1)
    $result.Data | math.round -digits 5 | Should -Be @(($b.Features["input"].data.ToDataSource().ToArray().Sum() | math.round -digits 5))
  }

}

Describe "Find()" {

  It "can obtain input variable by name" {

    $i = cntk.input 3 -Name "input"
    $f = cntk.reducesum $i -Axis (cntk.axis.allstatic)

    $in = $f.Find("input")

    $in | Should -beExactly $i
  }

}
