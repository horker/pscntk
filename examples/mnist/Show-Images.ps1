$MNIST_IMAGE_FILE = "$PSScriptRoot\..\..\data\MNIST\train-images.idx3-ubyte"

$data = [System.IO.File]::ReadAllBytes($MNIST_IMAGE_FILE)

$data = $data.Slice(@(16, 0)) # Skip header

$bitmaps = @()

for ($i = 0; $i -lt 200; ++$i) {
  # Preprocess
  $image = $data.Slice(@(($i * 28 * 28), (($i + 1) * 28 * 28)))
  $image = $image.Scale(0, 255, 255, 0)

  # Convert to bitmap
  $d = cntk.datasource $image (1, 28, 28) -DataType double
  $bitmaps += $d.ToBitmap("GrayScale", $false)
}

$bitmaps | out-canvas
