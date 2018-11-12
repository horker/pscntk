Set-StrictMode -Version 5

# ref. M. Zaheer et al. "Deep Sets"
# https://papers.nips.cc/paper/6931-deep-sets

function New-CNTKPermutationEquivariant {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [Horker.PSCNTK.WrappedVariable]$Operand,

        [Parameter(Position = 1, Mandatory = $true)]
        [int]$OutFeatureDim,

        [Parameter(Position = 2, Mandatory = $false)]
        [CNTK.CNTKDictionary]$Initializer = $null,

        [Parameter(Position = 3, Mandatory = $false)]
        [string]$Name = ""
    )

    if ($Operand.Shape.Rank -ne 2) {
        throw (New-Object ArgumentException "Input of New-PermutationEquivariant should have two dimensions")
    }

    if ($null -eq $Initializer) {
        $Initializer = cntk.init.glorotUniform
    }

    $featureDim = $Operand.Shape[0]
    $setSize = $Operand.Shape[1]

    $lambda = cntk.parameter -Dimensions ($OutFeatureDim, $featureDim) -Initializer $Initializer
    $gamma = cntk.parameter -Dimensions ($OutFeatureDim, $featureDim) -Initializer $Initializer

    $columns = New-Object Horker.PSCNTK.WrappedVariable[] $setSize
    for ($i = 0; $i -lt $setSize; ++$i) {
        $c = New-Object Horker.PSCNTK.WrappedVariable[] $setSize
        for ($j = 0; $j -lt $setSize; ++$j) {
            if ($i -eq $j) {
                $c[$j] = $lambda
            }
            else {
                $c[$j] = $gamma
            }
        }
        $columns[$i] = cntk.splice $c -Axis 0
    }

    $weight = cntk.splice $columns -Axis 1

    $n = cntk.times $weight (cntk.reshape $Operand ($featureDim * $setSize))
    $n = cntk.reshape $n ($OutFeatureDim, $setSize)
    $n.SetName($Name)

    $n
}

Set-Alias cntk.permutationEquivariant New-CNTKPermutationEquivariant
Set-Alias cntk.permEquiv New-CNTKPermutationEquivariant
