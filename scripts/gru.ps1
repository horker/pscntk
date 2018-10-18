Set-StrictMode -Version 4

function New-CNTKGruCell {
    [CmdletBinding()]
    param(
        [Horker.PSCNTK.WrappedVariable]$X,
        [double]$DropoutRate = 0.0,
        [CNTK.DeviceDescriptor]$Device = [CNTK.DeviceDescriptor]::UseDefaultDevice(),
        [string]$Name = ""
    )

    # z_t = sigmoid(W_z x [h_t-1, x_t])
    # r_t = sigmoid(W_r x [h_t-1, x_t])
    # hhat_t = tanh(W x [r_t * h_t-1, x_t])
    # h_t = (1 - z_t) * h_t-1 + z_t * hhat_t

    if ($X.Shape.Rank -eq 1) {
        $dim = $X.Shape.Dimensions[0]
        $xt = $X
    }
    else {
        $dim = $X.Shape.TotalSize
        $xt = cntk.reshape $X $dim
    }

    if ($DropoutRate -ne 0.0) {
        $xt = cntk.dropout $xt $DropoutRate
    }

    $dim = $X.Shape.Dimensions[0]
    $ht1 = cntk.placeholder $dim

    $wz = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotuniform) -Device $Device
    $bz = cntk.parameter $dim -Initializer (cntk.init.constant 1) # must be a large value

    $wr = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotuniform) -Device $Device
    $br = cntk.parameter $dim -Initializer (cntk.init.constant 0)

    $w = cntk.parameter ($dim, ($dim * 2)) -Initializer (cntk.init.glorotuniform) -Device $Device
    $b = cntk.parameter $dim -Initializer (cntk.init.constant 0)

    $htxt = cntk.splice @($ht1, $xt) 0
    $zt = cntk.sigmoid ((cntk.times $wz $htxt) + $bz)
    $rt = cntk.sigmoid ((cntk.times $wr $htxt) + $br)
    $hhatt = cntk.tanh ((cntk.times $w (cntk.splice @(($rt * $ht1), $xt) 0)) + $b)
    $ht = (1 - $zt) * $ht1 + $zt * $hhatt

    $ht.SetName($Name)

    @($ht, $ht1)
}

function New-CNTKGru {
    [CmdletBinding()]
    param(
        [Horker.PSCNTK.WrappedVariable]$X,
        [Horker.PSCNTK.WrappedVariable]$InitialState = $null,
        [double]$DropoutRate = 0.0,
        [switch]$ReturnSequences,
        [CNTK.DeviceDescriptor]$Device = [CNTK.DeviceDescriptor]::UseDefaultDevice(),
        [string]$Name = ""
    )

    $cell, $ht1 = New-CNTKGruCell $X -DropoutRate $DropoutRate -Device $Device
    $cell.ReplacePlaceholders(@{ $ht1 = (cntk.pastvalue $cell $InitialState) })

    if (!$ReturnSequences) {
        $cell = cntk.sequence.last $cell
    }

    $cell.SetName($Name)
    $cell
}

Set-Alias cntk.gru New-CNTKGru
