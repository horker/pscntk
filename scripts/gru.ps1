Set-StrictMode -Version 4

function New-CNTKGruCell {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [Horker.PSCNTK.WrappedVariable]$Operand,

        [Parameter(Position = 1, Mandatory = $false)]
        [double]$DropoutRate = [double]::NaN,

        [Parameter(Position = 2, Mandatory = $false)]
        [switch]$LayerNormalization = $false,

        [Parameter(Position = 3, Mandatory = $false)]
        [switch]$Stabilize = $false,

        [Parameter(Position = 4, Mandatory = $false)]
        [double]$Steepness = 4,

        [Parameter(Position = 5, Mandatory = $false)]
        [CNTK.DeviceDescriptor]$Device = [CNTK.DeviceDescriptor]::UseDefaultDevice(),

        [Parameter(Position = 6, Mandatory = $false)]
        [string]$Name = "gru"
    )

    # z_t = sigmoid(W_z x [h_t-1, x_t])
    # r_t = sigmoid(W_r x [h_t-1, x_t])
    # hhat_t = tanh(W x [r_t * h_t-1, x_t])
    # h_t = (1 - z_t) * h_t-1 + z_t * hhat_t

    if ($Operand.Shape.Rank -eq 1) {
        $dim = $Operand.Shape.Dimensions[0]
        $xt = $Operand
    }
    else {
        $dim = $Operand.Shape.TotalSize
        $xt = cntk.reshape $Operand $dim
    }

    if ($Stabilize) {
        function script:st($x) { cntk.stabilize $x $Steepness }
    }
    else {
        function script:st($x) { $x }
    }

    $dim = $Operand.Shape.Dimensions[0]
    $ht1 = cntk.placeholder $dim -WithSequenceAxis

    $wz = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotUniform) -Device $Device -Name ($Name + "/z/weight")
    # Initial value must be large
    $bz = cntk.parameter $dim -Initializer (cntk.init.constant 1) -Device $Device -Name ($Name + "/z/bias")

    $wr = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotUniform) -Device $Device -Name ($Name + "/r/weight")
    $br = cntk.parameter $dim -Initializer (cntk.init.constant 0) -Device $Device -Name ($Name + "/r/bias")

    $w = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotUniform) -Device $Device -Name ($Name + "/weight")
    $b = cntk.parameter $dim -Initializer (cntk.init.constant 0) -Device $Device -Name ($Name + "/bias")

    if ($LayerNormalization) {
        $ht1_ln = cntk.meanVarianceNormalization $ht1 #-Epsilon 1e-5
        $xt_ln = cntk.meanVarianceNormalization $xt -Epsilon 1e-5
    }
    else {
        $ht1_ln = $ht1
        $xt_ln = $xt
    }

    $zt = cntk.sigmoid ((cntk.times $wz (cntk.splice @((st $ht1_ln), (st $xt_ln)) 0)) + $bz)
    $rt = cntk.sigmoid ((cntk.times $wr (cntk.splice @((st $ht1_ln), (st $xt_ln)) 0)) + $br)
    $hhatt = cntk.tanh ((cntk.times $w (cntk.splice @((st ($rt * $ht1_ln)), (st $xt_ln)) 0)) + $b)

    if ($DropoutRate -ne [double]::NaN) {
        $hhatt = cntk.dropout -DropoutRate $DropoutRate $hhatt
    }

    $ht = (1 - $zt) * $ht1 + $zt * $hhatt

    $ht.SetName($Name)

    @($ht, $ht1)
}

function New-CNTKGru {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [Horker.PSCNTK.WrappedVariable]$Operand,

        [Parameter(Position = 1, Mandatory = $false)]
        [Horker.PSCNTK.WrappedVariable]$InitialState = $null,

        [Parameter(Position = 2, Mandatory = $false)]
        [double]$DropoutRate = [double]::NaN,

        [Parameter(Position = 3, Mandatory = $false)]
        [switch]$LayerNormalization = $false,

        [Parameter(Position = 4, Mandatory = $false)]
        [switch]$Stabilize = $false,

        [Parameter(Position = 5, Mandatory = $false)]
        [double]$Steepness = 4,

        [Parameter(Position = 6, Mandatory = $false)]
        [switch]$ReturnSequences,

        [Parameter(Position = 7, Mandatory = $false)]
        [CNTK.DeviceDescriptor]$Device = [CNTK.DeviceDescriptor]::UseDefaultDevice(),

        [Parameter(Position = 8, Mandatory = $false)]
        [string]$Name = "gru"
    )

    $cell, $ht1 = New-CNTKGruCell $Operand -DropoutRate $Dropoutrate -LayerNormalization:$LayerNormalization -Stabilize:$Stabilize -Steepness $Steepness -Device $Device -Name $Name
    $cell.ReplacePlaceholders(@{ $ht1 = (cntk.pastValue $cell $InitialState) })

    if (!$ReturnSequences) {
        $cell = cntk.sequence.last $cell
        $cell.SetName($Name)
    }

    $cell
}

Set-Alias cntk.gru New-CNTKGru
