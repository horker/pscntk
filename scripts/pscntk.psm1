#requires -PSEdition Desktop
Set-StrictMode -Version 4

############################################################
# Unmanaged DLLs to import
############################################################

# The order of the files is significant

$script:NATIVE_LIBS = @(
  "cublas64_90.dll"
  "cudart64_90.dll"
  "cudnn64_7.dll"
  "curand64_90.dll"
  "cusparse64_90.dll"
  "libiomp5md.dll"
  "nvml.dll"
  "opencv_world310.dll"
  "zip.dll"
  "zlib.dll"

  "mklml.dll"
  "mkldnn.dll"

  "Cntk.Math-2.5.1.dll"
  "Cntk.PerformanceProfiler-2.5.1.dll"

  "Cntk.Core-2.5.1.dll"

  "Cntk.Deserializers.Binary-2.5.1.dll"
  "Cntk.Deserializers.HTK-2.5.1.dll"
  "Cntk.Deserializers.Image-2.5.1.dll"
  "Cntk.Deserializers.TextFormat-2.5.1.dll"
  "Cntk.Composite-2.5.1.dll"
  "Cntk.Core.CSBinding-2.5.1.dll"
)

$LIB_DIR = "$PSScriptRoot\lib"

############################################################
# Import unmanaged DLLs
############################################################

$loader = add-type -pass -name Dll -memberDefinition @"
  [DllImport("kernel32.dll", CharSet=CharSet.Unicode)]
  public static extern IntPtr LoadLibrary(string dllToLoad);
"@

foreach ($l in $NATIVE_LIBS) {
  $file = Join-Path $LIB_DIR $l
  $pDll = $loader::LoadLibrary($file)

  if ($pDll -eq [IntPtr]::Zero) {
    Write-Error "Failed to load $file"
  }
}

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
