<%
Set-StrictMode -Version latest

$definitions = [cntk.cntklib] | gm -static | % Definition | sls "CNTK\.Function" | % { $_ -split ", static" -replace "\s*static\s*" | sort -desc | select -first 1 }

$sig = New-Object Collections.Generic.List[PSObject]

foreach ($s in $definitions) {
  $m = $s -match "^CNTK.Function (\w+)\(([^)]+)\)$"
  if (!$m) {
    Write-Host "Not matched: $s"
    continue
  }

  $name = $matches[1]
  $paramlist = $matches[2]

  if ($name -eq "OptimizedRNNStack") {
    continue
  }

  $params = $paramlist -split ",\s*"

  $defs = $params | foreach {
    $type, $var = $_ -split "\s+"

    $var = $var.Substring(0, 1).ToUpper() + $var.Substring(1)
    $defaultValue = $null
    if ($type -eq "string" -and $var -eq "name") {
      $defaultValue = '""'
    }

    $cast = ""
    if ($type -eq "uint32") {
      $type = "int"
      $cast = "(uint)"
    }

    [PSCustomObject]@{
      Type = $type
      Variable = $var
      CastVariable = $cast + $var
      DefaultValue = $defaultValue
    }
  }

  $result = [PSCustomObject]@{
    Name = $name
    Params = @($defs)
  }

  $sig.Add($result);
}
-%>
using System;
using System.Management.Automation;

namespace Horker.PSCNTK {
<%
foreach ($s in $sig) {
-%>

    [Cmdlet("New", "CNTK<% $s.Name %>")]
    [Alias("cntk.<% $s.Name.ToLower() %>")]
    public class NewCNTK<% $s.Name %> : PSCmdlet
    {
<%
  for ($i = 0; $i -lt $s.Params.Length; ++$i) {
    $p = $s.Params[$i]
-%>
        [Parameter(Position = <% $i %>, Mandatory = <% if ($p.DefaultValue) { %>false<% } else { %>true<% } %>)]
        public <% $p.Type %> <% $p.Variable %><% if ($p.DefaultValue) { " = " + $p.DefaultValue } %>;

<%
  }
-%>
        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.<% $s.Name %>(<% $s.Params.CastVariable -join ", " %>);
            WriteObject(result);
        }
    }
<%
}
-%>
}
