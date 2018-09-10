param(
  [string]$ModelPath
)

Import-Module ..\pscanvas\pscanvas

$model = cntk.load $ModelPath

1..100 | foreach {
  $result = $model.invoke(@{ noise = (math.rand -1 1 100) }).ToDataSource()
  $result.Reshape(1, 28, 28)
  $result.ToBitmap("Gray", $true)
} | out-canvas
