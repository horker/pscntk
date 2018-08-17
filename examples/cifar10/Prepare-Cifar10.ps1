Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

$CIFAR10_FILES = "$PSScriptRoot\..\..\data\CIFAR-10\*.bin"
$FILES = Resolve-Path $CIFAR10_FILES

$ALL_FILE = "$PSScriptRoot\cifar10_all.txt"
$TRAIN_FILE = "$PSScriptRoot\cifar10_train.txt"
$TEST_FILE = "$PSScriptRoot\cifar10_test.txt"

$RECORD_COUNT_PER_FILE = 10000

$TEST_SAMPLES = 1000

$LABEL_LENGTH = 1
$IMAGE_LENGTH = 32 * 32 * 3
$RECORD_LENGTH = $LABEL_LENGTH + $IMAGE_LENGTH

############################################################
# Convert data to CNTK Text Format
############################################################

$builder = Open-CNTKTextFormatBuilder $ALL_FILE

try {
  $image = New-Object byte[] $IMAGE_LENGTH

  for ($f = 0; $f -lt $FILES.Count; ++$f) {

    $file = $FILES[$f]
    Write-Host "Reading file '$file'..."

    $data = [System.IO.File]::ReadAllBytes($file)

    if ($data.Length -ne $RECORD_COUNT_PER_FILE * $RECORD_LENGTH) {
      Write-Error "Wrong file format: $file"
      return
    }

    Write-Host "Writing into CTF file..."

    for ($i = 0; $i -lt $RECORD_COUNT_PER_FILE; ++$i) {

      $label = $data[$i * $RECORD_LENGTH]
      $builder.AddOneHotSample("label", 10, $label)

      [Array]::Copy($data, $i * $RECORD_LENGTH + 1, $image, 0, $IMAGE_LENGTH)
      $builder.AddDenseSample("input", [float[]]$image.Scale(0, 255, 0, 1))

      $builder.NextLine();
    }
  }
}
finally {
  Close-CNTKTextFormatBuilder $builder
}

############################################################
# Divide into training and test files
############################################################

# TODO: Use faster way

Write-Host "Dividing into training file..."

$total = $RECORD_COUNT_PER_FILE * $FILES.Count

Get-Content $ALL_FILE -Total ($total - $TEST_SAMPLES) | Set-Content $TRAIN_FILE

Write-Host "Dividing into validation file..."

Get-Content $ALL_FILE -Tail $TEST_SAMPLES | Set-Content $TEST_FILE
