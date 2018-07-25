Set-StrictMode -Version 4

############################################################
# DLLs to import
############################################################

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

$script:MANAGED_LIBS = @(
  "Cntk.Core.Managed-2.5.1.dll"
)

$LIB_DIR = "$PSScriptRoot\lib"

############################################################
# Import dlls
############################################################

$loader = add-type -pass -name Dll2 -memberDefinition @"
  [DllImport("kernel32.dll", CharSet=CharSet.Unicode)]
  public static extern IntPtr LoadLibrary(string dllToLoad);
"@

# Native

foreach ($l in $NATIVE_LIBS) {
  $file = Join-Path $LIB_DIR $l
  $pDll = $loader::LoadLibrary($file)

  if ($pDll -eq [IntPtr]::Zero) {
    Write-Error "Failed to load $file"
  }
}

# Managed

foreach ($l in $MANAGED_LIBS) {
  $file = Join-Path $LIB_DIR $l
  [System.Reflection.Assembly]::LoadFrom($file)
}

#$METHOD_LIST = @(
#  [PSCustomObject]@{
#    ClassInfo = [Horker.Math.ArrayMethods.AdditionalMethods]
#    MethodNames = @(
#      "Split"
#      "Shuffle"
#      "DropNa"
#      "DropNaN"
#    )
#  }
#)
#
#foreach ($l in $METHOD_LIST) {
#  $ci = $l.ClassInfo
#  foreach ($m in $l.MethodNames) {
#    $mi = $ci.GetMethod($m)
#    Update-TypeData -TypeName System.Array -MemberName $m -MemberType CodeMethod -Value $mi -Force
#  }
#}
