<%
Set-StrictMode -Version latest

$EXCLUDES = @(
  "OptimizedRNNStack"
  "Crop"
  "Reshape"
  "ToSequence"
)

$definitions = [cntk.cntklib] | gm -static | % Definition | sls "CNTK\.Function"

$sig = New-Object Collections.Generic.List[PSObject]

foreach ($s in $definitions) {

  $overloads = $s -split ", static " -replace "static ", ""
  $longest = ([object[]]$overloads.OrderByDescending({ $args[0].Length }))[0]

  # Examine the range of the number of arguments

  $measures = $overloads | foreach{ ($_ -split ",").Length } | measure-object -min -max
  $minArgs = $measures.Minimum
  $maxArgs = $measures.Maximum

  $m = $longest -match "^CNTK\.Function (\w+)\(([^)]+)\)$"
  if (!$m) {
    Write-Host "Not matched: $longest"
    continue
  }

  $name = $matches[1]
  $paramlist = $matches[2]

  if ($EXCLUDES -contains $name) {
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
    }
  }

  $result = [PSCustomObject]@{
    Name = $name
    Params = @($defs)
    MinArgs = $minArgs
    MaxArgs = $maxArgs
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
        [Parameter(Position = <% $i %>, Mandatory = <% if ($i -ge $s.MinArgs) { %>false<% } else { %>true<% } %>)]
        public <% $p.Type %> <% $p.Variable %>;

<%
  }
-%>
        protected override void EndProcessing()
        {
            var argCount = MyInvocation.BoundParameters.Count;
<%
    for ($i = $s.MinArgs; $i -le $s.MaxArgs; ++$i) {
      $args = $s.Params[0..($i - 1)].CastVariable -join ", "
-%>

            if (argCount == <% $i %>)
            {
              var result = CNTK.CNTKLib.<% $s.Name %>(<% $args %>);
              WriteObject(result);
              return;
            }
<%
    }
-%>
        }
    }
<%
}
-%>
}
