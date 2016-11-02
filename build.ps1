##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was originally downloaded from https://github.com/cake-build/resources
# This version was download from https://github.com/larzw/Cake.Paket.Example
# It was modified to use paket (instead of NuGet) for dependency management.
# Feel free to change this file to fit your needs.
##########################################################################

<#
.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.
.DESCRIPTION
This Powershell script will download paket.exe if missing, 
install all your dependencies (including Cake) via paket.exe restore
and execute your Cake build script with the parameters you provide.
.PARAMETER Paket
The relative path to the .paket directory.
.PARAMETER Cake
The relative path to Cake.exe.
.PARAMETER Tools
The relative path to the Cake tools directory.
.PARAMETER Addins
The relative path to the Cake addins directory.
.PARAMETER Modules
The relative path to the Cake modules directory.
.PARAMETER ScriptArgs
Remaining arguments are added here.
.LINK
http://cakebuild.net
#>

[CmdletBinding()]
Param(
    [ValidatePattern('.paket$')]
    [string]$Paket = ".\.paket",
    [ValidatePattern('Cake.exe$')]
    [string]$Cake = ".\packages\Cake\Cake.exe",
    [string]$Tools = ".\packages",
    [string]$Addins = $Tools,
    [string]$Modules = $Tools,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

Write-Verbose -Message "Using paket for dependency management..."

# Set enviornment variables
$ToolsFullPath = Resolve-Pat $Tools
$AddinsFullPath = Resolve-Pat $Addins
$ModulesFullPath = Resolve-Pat $Modules
$ENV:CAKE_PATHS_TOOLS =  $ToolsFullPath
$ENV:CAKE_PATHS_ADDINS = $AddinsFullPath
$ENV:CAKE_PATHS_MODULES = $ModulesFullPath

# Make sure the .paket directory exits
$PaketFullPath = Resolve-Path $Paket
if(!(Test-Path $PaketFullPath)) {
    Throw "Could not find .paket directory at $PaketFullPath"
}
Write-Verbose -Message "Found .paket in PATH at $PaketFullPath"

# If paket.exe does not exits then download it using paket.bootstrapper.exe
$PAKET_EXE = Join-Path $PaketFullPath "paket.exe"
if (!(Test-Path $PAKET_EXE)) {   
    # If paket.bootstrapper.exe exits then run it.
    $PAKET_BOOTSTRAPPER_EXE = Join-Path $PaketFullPath "paket.bootstrapper.exe"
    if (!(Test-Path $PAKET_BOOTSTRAPPER_EXE)) {
        Throw "Could not find paket.bootstrapper.exe at $PAKET_BOOTSTRAPPER_EXE"
    }
    Write-Verbose -Message "Found paket.bootstrapper.exe in PATH at $PAKET_BOOTSTRAPPER_EXE"

    # Download paket.exe
    Write-Verbose -Message "Running paket.bootstrapper.exe to download paket.exe"
    Invoke-Expression $PAKET_BOOTSTRAPPER_EXE
        
    if (!(Test-Path $PAKET_EXE)) {
        Throw "Could not find paket.exe at $PAKET_EXE"
    }
}
Write-Verbose -Message "Found paket.exe in PATH at $PAKET_EXE"

# Install the dependencies
Write-Verbose -Message "Running paket.exe restore"
Invoke-Expression "$PAKET_EXE restore"

# Make sure that Cake has been installed.
$CAKE_EXE = Resolve-Path $Cake
if (!(Test-Path $CAKE_EXE)) {
    Throw "Could not find Cake.exe at $CAKE_EXE"
}
Write-Verbose -Message "Found Cake.exe in PATH at $CAKE_EXE"

# Start Cake
Write-Host "Running build script..."
Invoke-Expression "$Cake $ScriptArgs"
exit $LASTEXITCODE