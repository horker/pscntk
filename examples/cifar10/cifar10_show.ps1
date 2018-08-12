

$bitmaps = @()

$bitmaps = idq "$PSScriptRoot\cifar10.sqlite" "select image from data where label = 0 limit 100" | foreach {
  $image = $_.image
  $d = New-Object Horker.PSCNTK.DataSource[byte] $image, (32, 32, 3)
  $d = $d.Transpose(2, 0, 1)
  [Horker.PSCNTK.DataSourceToBitmap[byte]]::Do($d, "RGB", $false)
}

$bitmaps | out-canvas
