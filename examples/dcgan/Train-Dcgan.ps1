# MNIST DCGAN
#
# This example is written based on:
# https://github.com/Microsoft/CNTK/blob/release/2.2/Tutorials/CNTK_206B_DCGAN.ipynb

# Execute the following script beforehand to prepare training data:
# examples\mnist\Prepare-Mnist.ps1

Set-StrictMode -Version Latest

############################################################
# Configuration
############################################################

Set-CNTKRandomSeed 1234

$MNIST_CACHE_FILE = "$PSScriptRoot\..\mnist\mnist_data.bin"

$IMAGE_SHAPE = 28, 28, 1
$NOISE_SHAPE = 100

$MINIBATCH_SIZE = 128
$TOTAL_EPOCH = 1000

############################################################
# Data
############################################################

$data = cntk.datasourceset -Path $MNIST_CACHE_FILE

$noiseSampler = cntk.noisesampler "noise" $NOISE_SHAPE $MINIBATCH_SIZE -Min -1.0 -Max 1.0

$imageSampler = cntk.sampler $data -MinibatchSize $MINIBATCH_SIZE
$compositeSampler = cntk.compositesampler $imageSampler, $noiseSampler

############################################################
# Models
############################################################

function New-Generator {
  param(
    [CNTK.Variable]$In,
    [string]$Name
  )

  $init = cntk.normal 0.02

  $n = $In

  # h0: 100 -> 1024
  $n = cntk.dense $n 1024 -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.relu $n

  # h1: 1024 -> 7 x 7 x 128
  $n = cntk.dense $n (7, 7, 128) -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.relu $n

  # h1: 7 x 7 x 128 -> 14 x 14 x 128
  $n = cntk.convtrans2d $n (5, 5) 128 (2, 2) $true -OutputShape (14, 14, 128) -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.relu $n

  # h2: 14 x 14 x 128 -> 28 x 28 x 1
  $n = cntk.convtrans2d $n (5, 5) 1 (2, 2) $true -OutputShape (28, 28, 1) -Initializer $init
  $n = cntk.sigmoid $n

  $n.SetName($Name)
  $n
}

function New-Discriminator {
  param(
    [CNTK.Variable]$In,
    [string]$Name
  )

  $init = cntk.normal 0.02

  $n = $In

  # h0: 28 x 28 x 1 -> 12 x 12 x 1
  $n = cntk.conv2d $n -FilterShape (5, 5) -NumFilters 1 -Strides (2, 2) -Padding $false -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.leakyrelu $n 0.2

  # h1: 12 x 12 x 1 -> 4 x 4 x 64
  $n = cntk.conv2d $n -FilterShape (5, 5) -NumFilters 64 -Strides (2, 2) -Padding $false -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.leakyrelu $n 0.2

  # h2: 4 x 4 x 64 -> 1024
  $n = cntk.dense $n 1024 -Initializer $init
  $n = cntk.batchnorm $n -Spatial
  $n = cntk.leakyrelu $n 0.2

  # h3: 1024 -> 1
  $n = cntk.dense $n 1 -Activation sigmoid -Initializer $init

  $n.SetName($Name)
  $n
}

# build graphs

$Z = cntk.input $NOISE_SHAPE -Name "noise"
$X_real = cntk.input $IMAGE_SHAPE -Name "input"

$X_fake = New-Generator $Z "G_output"
$D_real = New-Discriminator $X_real "D_output"

$D_fake = $D_real.Clone("Share", @{ $X_real = $X_fake.Output });

############################################################
# Loss functions
############################################################

$G_loss = 1 - (cntk.log $D_fake)

$D_loss = - ((cntk.log $D_real) + (cntk.log (1 - $D_fake)))

############################################################
# Training
############################################################

$LEARNING_RATE = .0002
$MOMENTUM = .5

$G_learner = cntk.adam $X_fake $LEARNING_RATE $MOMENTUM
$G_trainer = cntk.trainer $X_fake -LossFunction $G_loss -Learners $G_learner
$G_session = cntk.trainingsession $G_trainer $noiseSampler
$G_enum = $G_session.GetEnumerator()

$D_learner = cntk.adam $D_real $LEARNING_RATE $MOMENTUM
$D_trainer = cntk.trainer $D_real -LossFunction $D_loss -Learners $D_learner
$D_session = cntk.trainingsession $D_trainer $compositeSampler
$D_enum = $D_session.GetEnumerator()

$lastEpoch = 1
while ($D_session.Epoch -lt $TOTAL_EPOCH) {
  # k = 2
  $null = $D_enum.MoveNext()
  $null = $D_enum.MoveNext()
  $null = $G_enum.MoveNext()

  $epoch = $D_session.Epoch
  if ($epoch -gt $lastEpoch -and $epoch % 10 -eq 0) {
    $lastEpoch = $epoch
    $G_session | select @{ n="Name"; e={ "Generator" } }, Epoch, Iteration, @{ n="Loss"; e={ $_.Loss.ToString("0.#####") }}, Elapsed
    $D_session | select @{ n="Name"; e={ "Discriminator" } }, Epoch, Iteration, @{ n="Loss"; e={ $_.Loss.ToString("0.#####") }}, Elapsed

    if ($epoch % 100 -eq 0) {
      $X_fake.Save("$PSScriptRoot\dcgan$($epoch).model")
    }
  }
}
