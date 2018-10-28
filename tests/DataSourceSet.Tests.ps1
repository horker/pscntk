Describe "Export/Import-CNTKMsgPack" {

  $file = "$PSScriptRoot\temp.msgpack"

  It "can export and import a DataSourceSet in MsgPack" {
      $a = cntk.dataSource (1, 2, 3, 4) (2, 2, 1)
      $b = cntk.dataSource (10, 20, 30, 40, 50, 60) (2, 3, 1)
      $dss = cntk.dataSourceSet @{ a = $a; b = $b }

      Export-CNTKMsgPack $dss $file

      $result = Import-CNTKMsgPack $file

      $result.Features.Count | Should -Be 2

      $d = $result["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 2, 1)
      $d.Data | Should -Be (1, 2, 3, 4)
  }

  It "can append multiple DataSourceSets to the file" {
      $a = cntk.dataSource (1, 2, 3, 4) (2, 2, 1)
      $dss = cntk.dataSourceSet @{ a = $a }
      Export-CNTKMsgPack $dss $file

      $b = cntk.dataSource (10, 20, 30, 40, 50, 60) (2, 3, 1)
      $dss = cntk.dataSourceSet @{ b = $b }
      Export-CNTKMsgPack $dss $file -Append

      $results = Import-CNTKMsgPack $file

      $results.Count | Should -Be 2

      $d = $results[0]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 2, 1)
      $d.Data | Should -Be (1, 2, 3, 4)

      $d = $results[1]["b"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 3, 1)
      $d.Data | Should -Be (10, 20, 30, 40, 50, 60)
  }

  It "can split DataSourceSets" {
      $a = cntk.dataSource (1..14) (2, 7)
      $dss = cntk.dataSourceSet @{ a = $a }
      Export-CNTKMsgPack $dss $file -SplitSize 3

      $results = Import-CNTKMsgPack $file

      $results.Count | Should -Be 3

      $d = $results[0]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 3)
      $d.Data | Should -Be (1..6)

      $d = $results[1]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 3)
      $d.Data | Should -Be (7..12)

      $d = $results[2]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 1)
      $d.Data | Should -Be (13, 14)
  }

  It "can omit fraction" {
      $a = cntk.dataSource (1..14) (2, 7)
      $dss = cntk.dataSourceSet @{ a = $a }
      Export-CNTKMsgPack $dss $file -SplitSize 3 -OmitFraction

      $results = Import-CNTKMsgPack $file

      $results.Count | Should -Be 2

      $d = $results[0]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 3)
      $d.Data | Should -Be (1..6)

      $d = $results[1]["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 3)
      $d.Data | Should -Be (7..12)
  }
}
