Set-StrictMode -Version Latest

Import-Module HorkerDataQuery
Import-Module psmath

$CIFAR10_FILES = "$PSScriptRoot\..\data\CIFAR-10\*.bin"

$FILES = Resolve-Path $CIFAR10_FILES
$RECORD_COUNT_PER_FILE = 10000

$RECORD_LENGTH = 3073
$LABEL_LENGTH = 1
$IMAGE_LENGTH = 3072

$MINIBATCH_SIZE = 50

[gc]::Collect()
$SQLITE_FILE = "$PSScriptRoot\cifar10.sqlite"
$null = New-Item $SQLITE_FILE -Force


############################################################
# Create tables
############################################################

function Create-Tables {
  param(
    [Data.Common.DbConnection]$conn
  )

  Invoke-DataQuery $conn "create table data (file int not null, label int not null, image blob($($IMAGE_LENGTH)) not null)"
}

############################################################
# Insert data to database table
############################################################

function Insert-Data {
  param(
    [Data.Common.DbConnection]$conn
  )

  $image = New-Object byte[] $IMAGE_LENGTH

  for ($f = 0; $f -lt $FILES.Count; ++$f) {
    $file = $FILES[$f]

    Write-Host "Reading file '$file'..."

    $data = [System.IO.File]::ReadAllBytes($file)

    if ($data.Length -ne $RECORD_COUNT_PER_FILE * $RECORD_LENGTH) {
      Write-Error "Wrong file format: $file"
      return
    }

    Write-Host "Inserting into table..."

    Invoke-DataQuery $conn "begin transaction"

    for ($i = 0; $i -lt $RECORD_COUNT_PER_FILE; ++$i) {
      $label = $data[$i * $RECORD_LENGTH]
      [Array]::Copy($data, $i * $RECORD_LENGTH + 1, $image, 0, $IMAGE_LENGTH)

      Invoke-DataQuery $conn "insert into data (file, label, image) values (@f, @l, @i)" @{ f = $f; l = $label; i = [byte[]]$image }
    }

    Invoke-DataQuery $conn "commit transaction"
  }
}

############################################################
# Randomize
############################################################

function Randomize-Data {
  param(
    [Data.Common.DbConnection]$conn
  )

  Write-Host "Randomize..."

  $count = (Invoke-DataQuery $conn "select count(*) Count from data").Count

  $order = (1..$count).Shuffle()

  $labels = New-Object "byte[]" ($MINIBATCH_SIZE * $LABEL_LENGTH)
  $images = New-Object "byte[]" ($MINIBATCH_SIZE * $IMAGE_LENGTH)

  Invoke-DataQuery $conn "begin transaction"

  for ($i = 0; $i -lt $count; $i += $MINIBATCH_SIZE) {
    Write-Host "$i " -NoNewLine
    for ($j = 0; $j -lt $MINIBATCH_SIZE; ++$j) {
      $batch = Invoke-DataQuery $conn "select rowid, label, image from data where rowid = @index" @{ index = $order[$i + $j] }
      $labels[$j] = $batch.label
      $batch.image.CopyTo($images, $j * $IMAGE_LENGTH)
    }
    Invoke-DataQuery $conn "insert into minibatches (label, image) values (@label, @image)" @{ label = [byte[]]$labels; image = [byte[]]$images }
  }

  Invoke-DataQuery $conn "commit transaction"
}

############################################################
# Run
############################################################

try {
  $conn = New-DataConnection $SQLITE_FILE

  Create-Tables $conn
  Insert-Data $conn
  Randomize-Data $conn
}
finally {
  Close-DataConnection $conn
}
