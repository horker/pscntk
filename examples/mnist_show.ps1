$MNIST_IMAGE_FILE = "$PSScriptRoot\..\data\MNIST\train-images.idx3-ubyte"

$data = [System.IO.File]::ReadAllBytes($MNIST_IMAGE_FILE)

$data = $data.Slice(@(16, 0)) # Skip header

$bitmaps = @()
for ($i = 0; $i -lt 200; ++$i) {
  $image = $data.Slice(@(($i * 28 * 28), (($i + 1) * 28 * 28)))
  $image = $image.ElementNegate().ElementAdd(255)
  $scaled = $image.Scale(0, 255, 1, 0)
  $d = New-Object Horker.PSCNTK.DataSource[double] $image, ([int[]](1, 28, 28))
  $bitmaps += [Horker.PSCNTK.DataSourceToBitmap[double]]::Do($d, "GrayScale", $false)
}

$bitmaps | out-canvas
