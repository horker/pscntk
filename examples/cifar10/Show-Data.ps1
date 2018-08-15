param(
  [int]$Label = 0
)

$bitmaps = @()

$bitmaps = idq "$PSScriptRoot\cifar10.sqlite" "select image from data where label = $Label limit 100" | foreach {
  $image = $_.image
  $d = cntk.datasource $image (32, 32, 3) -DataType byte
  $d = $d.Transpose(2, 0, 1)
  $d.ToBitmap("RGB", $false)
}

$bitmaps | out-canvas
