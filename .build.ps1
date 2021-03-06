task . Compile, Build, ImportDebug, Test

Set-StrictMode -Version 4

Import-Module HorkerTemplateEngine
Import-Module psmath

############################################################
# Settings
############################################################

$SOURCE_PATH = "$PSScriptRoot\source\Horker.PSCNTK"
$SCRIPT_PATH = "$PSScriptRoot\scripts"

$MODULE_PATH = "$PSScriptRoot\pscntk"
$MODULE_PATH_DEBUG = "$PSScriptRoot\debug\pscntk"

$SOLUTION_FILE = "$PSScriptRoot\source\pscntk.sln"

$CNTK_VERSION = "2.6"

$OBJECT_FILES = @(
  "Cntk.Core.Managed-2.6.dll"
  "MsgPack.dll"
  "Horker.PSCNTK.dll"
  "Horker.PSCNTK.pdb"
)

$LIB_PATH = "$PSScriptRoot\lib\$CNTK_VERSION"

$TEMPLATE_INPUT_PATH = "$PSScriptRoot\templates"
$TEMPLATE_OUTPUT_PATH = "$PSScriptRoot\source\Horker.PSCNTK\Generated files"

#$HELP_INPUT =  "$SOURCE_PATH\bin\Release\Horker.Math.dll"
#$HELP_INTERM = "$SOURCE_PATH\bin\Release\Horker.Data.dll-Help.xml"
#$HELP_OUTPUT = "$MODULE_PATH\Horker.Data.dll-Help.xml"
#$HELPGEN = "$PSScriptRoot\vendor\XmlDoc2CmdletDoc.0.2.10\tools\XmlDoc2CmdletDoc.exe"

############################################################
# Helper cmdlets
############################################################

function New-Folder2 {
  param(
    [string]$Path
  )

  try {
    $null = New-Item -Type Directory $Path -EA Stop
    Write-Host -ForegroundColor DarkCyan "$Path created"
  }
  catch {
    Write-Host -ForegroundColor DarkYellow $_
  }
}

function Copy-Item2 {
  param(
    [string]$Source,
    [string]$Dest
  )

  try {
    Copy-Item $Source $Dest -EA Stop
    Write-Host -ForegroundColor DarkCyan "Copy from $Source to $Dest done"
  }
  catch {
    Write-Host -ForegroundColor DarkYellow $_
  }
}

function Remove-Item2 {
  param(
    [string]$Path
  )

  Resolve-Path $PATH | foreach {
    try {
      Remove-Item $_ -EA Stop -Recurse -Force
      Write-Host -ForegroundColor DarkCyan "$_ removed"
    }
    catch {
      Write-Host -ForegroundColor DarkYellow $_
    }
  }
}

############################################################
# Tasks
############################################################

task Compile {
  msbuild $SOLUTION_FILE /p:Configuration=Debug /nologo /v:minimal
  msbuild $SOLUTION_FILE /p:Configuration=Release /nologo /v:minimal
}

task Build {
  . {
    $ErrorActionPreference = "Continue"

    function Copy-ObjectFiles {
      param(
        [string]$targetPath,
        [string]$objectPath
      )

      New-Folder2 $targetPath

      Copy-Item2 "$SCRIPT_PATH\*" $targetPath
      $OBJECT_FILES | foreach {
        $path = Join-Path $objectPath $_
        Copy-Item2 $path $targetPath
      }
    }

    Copy-ObjectFiles $MODULE_PATH "$SOURCE_PATH\bin\Release"
    Copy-ObjectFiles $MODULE_PATH_DEBUG "$SOURCE_PATH\bin\Debug"

  }
}

#task BuildHelp `
#  -Inputs $HELP_INPUT `
#  -Outputs $HELP_OUTPUT `
#{
#  . $HELPGEN $HELP_INPUT
#
#  Copy-Item $HELP_INTERM $MODULE_PATH
#}

task Test Build, ImportDebug, {
  Invoke-Pester "$PSScriptRoot\tests"
}

task ImportDebug {
  Import-Module $MODULE_PATH_DEBUG -Force
}

task Clean {
  Remove-Item2 "$MODULE_PATH\*"
  Remove-Item2 "$MODULE_PATH_DEBUG\*"
}

task Templates {
#  .\tools\Extract-FunctionSignature.ps1

  dir $TEMPLATE_INPUT_PATH | foreach {
    $inFile = $_.FullName
    $outFile = Join-Path $TEMPLATE_OUTPUT_PATH ($_.Name -replace "\.template\.", ".")
    Get-Content $inFile | Invoke-TemplateEngine | Set-Content $outFile
  }
}

task CopyLib {
  $dir = "$MODULE_PATH\lib\$CNTK_VERSION"
  New-Folder2 $dir
  Copy-Item2 "$LIB_PATH\*.dll" $dir
  Copy-Item2 "$LIB_PATH\*.pdb" $dir

  $dir = "$MODULE_PATH_DEBUG\lib\$CNTK_VERSION"
  New-Folder2 $dir
  Copy-Item2 "$LIB_PATH\*.dll" $dir
  Copy-Item2 "$LIB_PATH\*.pdb" $dir
}

task CollectUnmanagedLib {
  Get-ChildItem -Recurse "$PSScriptRoot\source\packages" -Filter *.dll |
  where {
    $_.fullname -match $CNTK_VERSION -and $_.fullname -notmatch "Debug"
  } |
  foreach {
    Copy-Item2 $_.FullName $LIB_PATH
  }
}
