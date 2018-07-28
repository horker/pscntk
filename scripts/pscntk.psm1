#requires -PSEdition Desktop

Set-StrictMode -Version 4

############################################################
# Import unmanaged DLLs
############################################################

$LIB_DIR = "$PSScriptRoot\lib"

[Horker.PSCNTK.UnmanagedDllLoader]::Load($LIB_DIR)

############################################################
# Extension methods
############################################################

$METHOD_LIST = @(
  [PSCustomObject]@{
    TargetClass = "CNTK.Function"
    ClassInfo = [Horker.PSCNTK.FunctionMethods]
    MethodNames = @(
      "Get"
      "Invoke"
      "AsTree"
    )
  }

  [PSCustomObject]@{
    TargetClass = "CNTK.Value"
    ClassInfo = [Horker.PSCNTK.ValueMethods]
    MethodNames = @(
      "AsString"
      "ToDataSource"
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
