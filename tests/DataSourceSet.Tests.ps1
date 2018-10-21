Describe "Set-CNTKDataSourceSet/Get-CNTKDataSourceSet" {

  $file = "$PSScriptRoot\temp.msgpack"

  It "can save and load a DataSourceSet" {
      $a = cntk.dataSource (1, 2, 3, 4) (2, 2, 1)
      $b = cntk.dataSource (10, 20, 30, 40, 50, 60) (2, 3, 1)
      $dss = cntk.dataSourceSet @{ a = $a; b = $b }

      Set-CNTKDataSourceSet $file $dss

      $result = Get-CNTKDataSourceSet $file

      $result.Features.Count | Should -Be 2

      $d = $result["a"]
      $d | Should -BeOfType [Horker.PSCNTK.IDataSource[float]]
      $d.Shape.Dimensions | Should -Be (2, 2, 1)
      $d.Data | Should -Be (1, 2, 3, 4)
  }

  It "can save and load multiple DataSourceSets" {
      $a = cntk.dataSource (1, 2, 3, 4) (2, 2, 1)
      $dss = cntk.dataSourceSet @{ a = $a }
      Set-CNTKDataSourceSet $file $dss

      $b = cntk.dataSource (10, 20, 30, 40, 50, 60) (2, 3, 1)
      $dss = cntk.dataSourceSet @{ b = $b }
      Add-CNTKDataSourceSet $file $dss

      $results = Get-CNTKDataSourceSet $file

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
}
