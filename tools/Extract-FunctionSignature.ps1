param(
  [string]$File = "$PSScriptRoot\CNTKLibrary.h"
)

# Location:
# CNTK\Source\CNTKv2LibraryDll\API\CNTKLibrary.h

Set-StrictMode -Version Latest

############################################################
# Tokenization
############################################################

$lines = Get-Content $File

$tokens = New-Object Collections.Generic.List[object]
$comment = @()

foreach ($line in $lines) {

  if ($line -match "^\s*///\s*(.+)") {
    $comment += $matches[1]
    continue
  }

  if ($comment.Length -gt 0) {
    $token = "<comment>" + ($comment -join "`r`n")
    $tokens.Add($token)
    $comment = @()
  }

  $line = $line -replace "/\*.+\*/", " "

  $tokens.Add("<bol>")

  foreach ($t in ($line -split "\s+|(\(|\)|&|<|>|,|\.|:+|;|//)")) {
    if ([string]::IsNullOrEmpty($t)) {
      continue;
    }

    if ($t -eq "//") {
      break
    }

    $tokens.Add($t)
  }
}

#$tokens

############################################################
# Extract Signatures
############################################################

$script:pos = 0

function Next {
  param([int]$Steps = 1)

  if ($script:pos -ge $tokens.Count) {
    "<eof>"
  }
  else {
    $script:pos += $Steps
    $tokens[$script:pos - 1]
  }
}

function Peek {
  param([int]$Lookahead = 0)

  $p = $script:pos + $Lookahead
  if ($p -ge $tokens.Count) {
    "eof"
  }
  else {
    $tokens[$p]
  }
}

function Read-AngleBracket {
  param($arg)

  $arg.Add("<")

  $t = Next
  for (; $t -ne ">"; $t = Next) {
    if ($t -eq "<bol>") {
      continue
    }
    if ($t -eq "<") {
      Read-AngleBracket $arg
    }
    else {
      $arg.Add($t)
    }
  }

  $arg.Add(">")
}

$eof = $false
$arg = New-Object Collections.Generic.List[object]

$funcs = New-Object Collections.Generic.List[object]

while (!$eof) {
  $t = Next
#  Write-Host -NoNewLine "($($t) $($t)) "

  if ($t -eq "<eof>") {
    $eof = $true
  }
  elseif ($t -eq "<bol>") {
    if ((((Peek 0) -eq "CNTK_API" -or (Peek 0) -eq "inline") -and (Peek 1) -eq "FunctionPtr" -and (Peek 3) -eq "(") -or
        (((Peek 0) -eq "CNTK_API" -or (Peek 0) -eq "inline") -and (Peek 1) -eq "ParameterInitializer" -and (Peek 2) -match "Initializer$" -and (Peek 3) -eq "(")) {
      $func = [PSCustomObject]@{ Name = (Peek 2); ReturnType = (Peek 1); Args = (New-Object Collections.Generic.List[object]) }
      $arg.Clear()

      $t = Next 5
      for (; $t -ne ")"; $t = Next) {
        if ($t -eq "<bol>") {
          continue
        }
        if ($t -eq ",") {
          $func.Args.Add($arg.ToArray())
          $arg.Clear()
        }
        elseif ($t -eq "<") {
          Read-AngleBracket $arg
        }
        else {
          $arg.Add($t)
        }
      }
      if ($arg.Count -gt 0) {
        $func.Args.Add($arg.ToArray())
      }

      if ($func.Args.Count -gt 0) {
        $funcs.Add($func)
      }
    }
  }
}

############################################################
# Exclude mismatch
############################################################

$EXCLUDES = @(
  "AsBlock"
  "Atan"
  "BroadcastAs"
  "Clone"
  "CloneFlattened"
  "CustomProxyOp"
  "EyeLike"
  "First"
  "Gather"
  "IsFirst"
  "IsLast"
  "Last"
  "NCELoss"
  "operator+"
  "OptimizedRNNStack"
  "ReplacePlaceholder"
  "ReplacePlaceholders"
  "Scatter"
  "StraightThrough"
  "Tan"
  "Unpack"
  "Where"
)

$funcs = $funcs | where { $EXCLUDES -NotContains $_.Name }

############################################################
# Build arguments
############################################################

foreach ($func in $funcs) {
  $arglist = New-Object Collections.Generic.List[object]
  foreach ($arg in $func.Args) {
    $a = $arg -join " "
    $a = $a -replace "\s*,\s*", ", "
    $a = $a -replace "\s*::\s*", "::"
    $a = $a -replace "\s*<\s*", "<"
    $a = $a -replace "\s*>\s*", ">"
    $a = $a -replace "(>+)", "`$1 "
    $a = $a -replace "\s*&\s*", "& "
    $a = $a -replace "& &", "&&"
    $type, $value = $a -split "\s*=\s*"
    $value = $value -replace "\s*", ""
    $var = $null
    if ($type -match "^(.+)\s+([^\s]+)$") {
      $type = $matches[1]
      $var = $matches[2]
    }

    $a = [PSCustomObject]@{ Type = $type; Variable = $var; Value = $value }
    $arglist.Add($a)
  }

  $func.Args = $arglist.ToArray()
}

############################################################
# Save output
############################################################

$dup = $funcs | group Name | where { $_.Count -gt 1 }
$dup | select -expand Group | sort Name | Export-CliXml "$PSScriptRoot\dupfuncs.clixml"

$dup = $dup | select -expand Name
$funcs = $funcs | where { $dup -NotContains $_.Name }

$funcs | sort Name | Export-Clixml "$PSScriptRoot\cntkfuncs.clixml"
