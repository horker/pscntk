#requires -PSEdition Desktop

Set-StrictMode -Version 4

############################################################
# Import unmanaged DLLs
############################################################

$LIB_DIR = "$PSScriptRoot\lib"

[Horker.PSCNTK.UnmanagedDllLoader]::Load($LIB_DIR)

############################################################
# Load PowerShell modules
############################################################

. "$PSScriptRoot\gru.ps1"
. "$PSScriptRoot\permequiv.ps1"

############################################################
# Extension methods
############################################################

$METHOD_LIST = @(
  [PSCustomObject]@{
    TargetClass = "CNTK.Function"
    ClassInfo = [Horker.PSCNTK.FunctionPSMethods]
    MethodNames = @(
      "Find"
      "Invoke"
      "AsTree"
      "AsTreeWithValues"
      "ToDot"
      "GetNodeInfo"
    )
  }

  [PSCustomObject]@{
    TargetClass = "Horker.PSCNTK.WrappedFunction"
    ClassInfo = [Horker.PSCNTK.FunctionPSMethods]
    MethodNames = @(
      "Find"
      "Invoke"
      "AsTree"
      "AsTreeWithValues"
      "ToDot"
      "GetNodeInfo"
    )
  }

  [PSCustomObject]@{
    TargetClass = "CNTK.Value"
    ClassInfo = [Horker.PSCNTK.ValuePSMethods]
    MethodNames = @(
      "AsString"
      "ToDataSource"
      "ToArray"
    )
  }

  [PSCustomObject]@{
    TargetClass = "CNTK.NDArrayView"
    ClassInfo = [Horker.PSCNTK.NDArrayViewPSMethods]
    MethodNames = @(
      "AsString"
    )
  }
)

foreach ($l in $METHOD_LIST) {
  $ci = $l.ClassInfo
  $target = $l.TargetClass
  foreach ($m in $l.MethodNames) {
    $mi = $ci.GetMethod($m)
    Update-TypeData -TypeName $target -MemberName $m -MemberType CodeMethod -Value $mi -Force
  }
}

############################################################
# Settings
############################################################

[CNTK.CNTKLib]::SetTraceLevel("Error")
