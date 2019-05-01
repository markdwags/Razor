<#
.SYNOPSIS
  A basic Powershell script to download ClassicUO and Razor
  and configure the basic settings.
.NOTES
  Version:        1.0
  Author:         Quick
  Creation Date:  5/1/2019
#>

Write-Host "InstallClassicUOAndRazor.ps1"
Write-Host "v1.0"
Write-Host "Author: Quick (https://github.com/markdwags/Razor)"
Write-Host ""

# Set the directory to be the one the command was issued in
$WorkingDir = $(Get-Location).Path

Write-Host "This script will install the latest developer versions of ClassicUO and Razor. It can be ran to install for the first time, or to ensure your current versions of ClassicUO and Razor are on the latest developer preview builds.

NOTE: This script will perform the following steps, so please review first:

1) Download the latest ClassicUO dev preview build to: $WorkingDir
2) Download the latest Razor dev preview to: $WorkingDir\Razor\
3) Help with setting up the initial settings.json file if it doesn't exist

WARNING: This script will overwrite ALL files related to ClassicUO and Razor. Backup this folder if you want to preserve this install.

"
$proceed = Read-Host -Prompt "Would you like to continue with the process above? [y or n]"

if ($proceed -ne "Y" -Or $proceed -ne "y") { Exit }

# Set TLS to be v1.2 or Invoke-WebRequest will fail
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# First, let's get the latest ClassicUO build from GitHub
Write-Host "Downloading the latest dev build of ClassicUO.."
Invoke-WebRequest -Uri "https://github.com/andreakarasho/ClassicUO/releases/download/ClassicUO-dev-preview/ClassicUO-dev-preview-release.zip" -OutFile "$WorkingDir\ClassicUO-dev-preview-release.zip" -Verbose
Write-Host "..done"

# Now download the latest Razor build from GitHub
Write-Host "Downloading the latest dev build of Razor.."
Invoke-WebRequest -Uri "https://github.com/markdwags/Razor/releases/download/Razor-dev-preview/Razor-dev-preview.zip" -OutFile "$WorkingDir\Razor-dev-preview.zip" -Verbose
Write-Host "..done"

# Extract both ClassicUO and Razor into the folder this script is running in
Write-Host "Extracting ClassicUO and Razor"
Expand-Archive -Path "$WorkingDir\ClassicUO-dev-preview-release.zip" -DestinationPath "$WorkingDir\" -Force
Expand-Archive -Path "$WorkingDir\Razor-dev-preview.zip" -DestinationPath "$WorkingDir\Razor\" -Force
Write-Host "..done"

# For some users, the files are "Blocked" and require to be unblock to run Razor with CUO correctly. Unblock both folders just in case.
Write-Host "Unblocking ClassicUO and Razor files.."
Get-ChildItem -Path "$WorkingDir\" | Unblock-File
Get-ChildItem -Path "$WorkingDir\Razor\" | Unblock-File
Write-Host "..done"

# If the settings.json file for ClassicUO doesn't exist, run ClassicUO so it auto-generates
if ($(Test-Path "$WorkingDir\settings.json" -PathType Leaf) -eq $False) {

  # Launch ClassicUO to generate default file
  Write-Host "Couldn't find settings.json, launching ClassicUO to create a new one."
  Write-Host "*** PLEASE CLICK THE OK WHEN PROMPTED BY CLASSICUO ***"
  Start-Process -FilePath "$WorkingDir\ClassicUO.exe" -Wait

  Write-Host "Setting up settings.json"  

  # User input for the various data points for settings.json

  do {
    $serverhost = Read-Host -Prompt "> Enter the host/ip of the UO server (ie: server.name.com)"
  } until ([string]::IsNullOrEmpty($serverhost) -eq $false)

  do {
    $port = Read-Host -Prompt "> Enter the port of the UO server (ie: 2593)"
  } until ([string]::IsNullOrEmpty($port) -eq $false)

  do {
    $path = Read-Host -Prompt "> Enter the path to your Ultima Online files (default: C:\Ultima Online)" 
  } until ([string]::IsNullOrEmpty($path) -eq $false)

  do {
    $client = Read-Host -Prompt "> Enter the client version you are using (ie: 7.0.0.0 or 5.0.0.0)"  
  } until ([string]::IsNullOrEmpty($client) -eq $false)

  # Update the default settings.json file to point to where we installed Razor
  $cuoSettings = Get-Content "$WorkingDir\settings.json" -Raw | ConvertFrom-Json

  # Set the values based on the input from above
  $cuoSettings."ip" = $serverhost
  $cuoSettings."port" = [int]$port
  $cuoSettings."ultimaonlinedirectory" = $path
  $cuoSettings."clientversion" = $client

  # Set plugin location to point to Razor
  $cuoSettings."plugins"[0] = "$WorkingDir\Razor\Razor.exe"

  # Save the settings back to the file
  Write-Host "Saving input to settings.json"  
  $cuoSettings | ConvertTo-Json -Depth 32 | Set-Content "$WorkingDir\settings.json"

} else {

  Write-Host "settings.json exists, no changes made"

}

Write-Host "The latest developer version of ClassicUO and Razor are installed."

# Let's create a shortcut on the Desktop if one doesn't exist
if ($(Test-Path "$Home\Desktop\ClassicUO.lnk" -PathType Leaf) -eq $False) {
  $createShortcut = Read-Host -Prompt "Would you like create a shortcut on your Desktop? [y or n]"

  if ($createShortcut -eq "Y" -Or $createShortcut -eq "y") {
    $WshShell = New-Object -comObject WScript.Shell
    $shortcut = $WshShell.CreateShortcut("$Home\Desktop\ClassicUO.lnk")
    $shortcut.TargetPath = "$WorkingDir\ClassicUO.exe"
    $shortcut.Save()
  }  
}

# Should we launch ClassicUO now?
$launch = Read-Host -Prompt "Would you like to launch ClassicUO with Razor? [y or n]"

if ($launch -eq "Y" -Or $launch -eq "y") {
  Write-Host "Launching ClassicUO.exe"
  Start-Process -FilePath "$WorkingDir\ClassicUO.exe"
}

Write-Host "Press ENTER to exit or close this window"
Read-Host
