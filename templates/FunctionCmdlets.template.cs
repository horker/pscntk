<% # -*- ft=ps1 -*-
Set-StrictMode -Version latest

$funcs = Import-CliXml "tools\cntkfuncs.clixml"
#$funcs = Import-CliXml "tools\dupfuncs.clixml"

$TYPE_MAP = @{
  "Axis&" = "CNTK.Axis"
  "bool" = "bool"
  "const Axis&" = "CNTK.Axis"
  "const bool" = "bool"
  "const Constant&" = "CNTK.Constant"
  "const FunctionPtr&" = "WrappedFunction"
  "const NDArrayViewPtr&" = "CNTK.NDArrayView"
  "const NDShape&" = "int[]"
  "const std::pair<size_t, int>&" = "CNTK.PairSizeTInt"
  "const std::vector<Axis>&" = "CNTK.Axis[]"
  "const std::vector<bool>&" = "bool[]"
  "const std::vector<int>&" = "int[]"
  "const std::vector<size_t>&" = "UInt32[]"
  "const std::vector<Variable>&" = "CNTK.Variable[]"
  "const std::wstring&" = "string"
  "const Variable&" = "WrappedVariable"
  "DataType" = "CNTK.DataType"
  "double" = "double"
  "float" = "float"
  "int" = "int"
  "PaddingMode" = "CNTK.PaddingMode"
  "ParameterCloningMethod" = "CNTK.ParameterCloningMethod"
  "PoolingType" = "CNTK.PoolingType"
  "size_t" = "UInt32"
  "std::vector<float>" = "float[]"
  "unsigned long" = "UInt32"
  "Variable&" = "WrappedVariable"
}

$CAST_MAP = @{
  "CNTK.Axis[]" = "CNTK.AxisVector"
  "bool[]" = "CNTK.BoolVector"
  "float[]" = "CNTK.FloatVector"
  "UInt32[]" = "CNTK.SizeTVector"
  "CNTK.Variable[]" = "CNTK.VariableVector"
}

$VALUE_MAP = @(
  @{ Re = '^{\s*(\d)+\s*}$'; To = 'new int[] { $1 }' }
  @{ Re = '^{\s*(true|false)\s*}$'; To = 'new bool[] { $1 }' }
  @{ Re = '^([\d\.]+)$'; To = '$1' }
  @{ Re = '^(true|false)$'; To = '$1' }
  @{ Re = '^L"(.*)"(\*/)?$'; To = '"$1"' }
  @{ Re = '^ParameterCloningMethod::(\w+)$'; To = 'CNTK.ParameterCloningMethod.$1' }
  @{ Re = '^SentinelValueForAutoSelectRandomSeed$'; To = 'Constants.SentinelValueForAutoSelectRandomSeed' }
  @{ Re = '^SentinelValueForInferParamInitRank$'; To = 'Constants.SentinelValueForInferParamInitRank' }
  @{ Re = '^DefaultParamInitScale$'; To = 'Constants.DefaultParamInitScale' }
  @{ Re = '^DefaultParamInitOutputRank$'; To = 'Constants.DefaultParamInitOutputRank' }
  @{ Re = '^DefaultParamInitFilterRank$'; To = 'Constants.DefaultParamInitFilterRank' }
)

function Convert-Value {
  param([string]$value)

  if ([string]::IsNullOrEmpty($value)) {
    return $value
  }

  foreach ($vm in $VALUE_MAP) {
    if ($value -match $vm.Re) {
      return $value -replace $vm.Re, $vm.To
    }
  }

  Write-Error "Unknown value: '$value'"
}

function ConvertTo-TitleCase {
  param([string]$s)

  $t = $s.Substring(0, 1).ToUpper()
  $s -replace "^.", $t
}

-%>
using System;
using System.Management.Automation;

// DO NOT EDIT
// This file was automatically generated at <% (Get-Date).ToString("yyyy/MM/dd HH:mm:ss") %>

namespace Horker.PSCNTK {
<%
foreach ($func in $funcs) {

  # Name
  if ($func.Ns -eq "Sequence") {
    $name = "Sequence" + $func.Name
  }
  else {
    $name = $func.Name
  }

  # Aliases
  $alias = [Char]::ToLower($func.Name[0]) + $func.Name.Substring(1)
  if ($func.Ns -eq "Sequence") {
    $alias = "sequence." + $alias
  }

  if ($alias -match "initializer$") {
    $alias = $alias -replace "initializer$", ""
    if ($alias -match "^(he|glorot|xavier|normal|truncated|uniform)") {
      $alias = ($alias -replace "^", "init."), $alias
    }
    else {
      $alias = $alias -replace "^", "init."
    }
  }
  $alias = $alias | foreach { "cntk." + $_ }

  # OutputType
  switch ($func.ReturnType) {
    "FunctionPtr" { $outputType = "Horker.PSCNTK.WrappedFunction" }
    "ParameterInitializer" { $outputType = "CNTK.CNTKDictionary" }
    default { Write-Error "Unknown return type: $($func.ReturnType)" }
  }
-%>

    [Cmdlet("New", "CNTK<% $name %>")]
    [Alias("<% $alias -join '", "' %>")]
    [OutputType(typeof(<% $outputType %>))]
    public class NewCNTK<% $name %> : PSCmdlet
    {
<%
  $arglist = @()
  for ($i = 0; $i -lt $func.Args.Count; ++$i) {
    $arg = $func.Args[$i]
    $type = $TYPE_MAP[$arg.Type]
    if ($null -eq $type) {
      Write-Error "Unknown argument type: $($arg.Type)"
    }

    $variable = ConvertTo-TitleCase $arg.Variable
    if ("CNTK.DataType" -eq $type) {
      $arg.Value = "CNTK.DataType.Float"
      $value = "CNTK.DataType.Float"
    }
    else {
      $value = Convert-Value $arg.Value
    }

    $casted = $variable
    if ($CAST_MAP.ContainsKey($type)) {
      $casted = "new " + $CAST_MAP[$type] + "(" + $variable + ")"
    }

    $arglist += $casted
-%>
        [Parameter(Position = <% $i %>, Mandatory = <% if ([string]::IsNullOrEmpty($arg.Value)) { %>true<% } else { %>false<% } %>)]
        public <% $type %> <% $variable %><% if (![string]::IsNullOrEmpty($value)) { %> = <% $value } %>;

<%
  }
-%>
        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.<% $name %>(<% $arglist -join ", " %>);
<%
  if ($func.ReturnType -eq "FunctionPtr") {
-%>
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
<%
  } else {
-%>
            WriteObject(result);
<%
  }
-%>
        }
    }
<%
}
-%>
}
