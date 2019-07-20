<#
.SYNOPSIS
  A basic Powershell script to download ClassicUO and Razor
  and configure the basic settings.
.NOTES
  Version:        1.1
  Author:         Quick
  Creation Date:  5/1/2019
#>

Write-Host "Install ClassicUO & Razor - v1.1" -ForegroundColor Yellow
Write-Host "Author: Quick (https://github.com/markdwags/Razor)`n" -ForegroundColor Yellow

# Confirm they have PowerShell 5.1 installed
if ($PSVersionTable.PSVersion.Major -ne 5 -And $PSVersionTable.PSVersion.Minor -ne 1) {
    Write-Host "************************" -ForegroundColor Red
    Write-Host "This script requires that you are running PowerShell 5.1" -ForegroundColor Red
    Write-Host "You can download the installation package here: https://www.microsoft.com/en-us/download/details.aspx?id=54616" -ForegroundColor Red
    Write-Host "************************" -ForegroundColor Red

    Write-Host "`nPress ENTER to exit or close this window`n" -ForegroundColor Red
    Read-Host

    Exit
}

# Use UOR defaults?
$useDefaults = $false

# Set the directory to be the one the command was issued in
$WorkingDir = $(Get-Location).Path

# Lets just talk about what we're doing
Write-Host "You can use this script to install ClassicUO & Razor for the first time, or to update your existing installation to the latest developer preview builds.`n"

Write-Host "** NOTE: If you are upgrading, your ClassicUO and Razor profiles/macros will not be touched.`n" -ForegroundColor Cyan

Write-Host "This script will perform the following steps, so please review first:`n" -ForegroundColor Red

Write-Host "1. Download the latest ClassicUO dev preview build to: $WorkingDir" -ForegroundColor Green
Write-Host "2. Download the latest Razor dev preview to: $WorkingDir\Razor" -ForegroundColor Green
Write-Host "3. Help with setting up the initial settings.json file if it doesn't exist, or confirm your existing one is configured correctly for Razor`n" -ForegroundColor Green

$proceed = Read-Host -Prompt "> Would you like to continue with the process above? [y or n]"

if ($proceed -ne "Y" -Or $proceed -ne "y") { Exit }

# Set TLS to be v1.2 or Invoke-WebRequest will fail
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# First, let's get the latest ClassicUO build from GitHub
Write-Host "Downloading the latest dev build of ClassicUO.." -ForegroundColor Cyan
Invoke-WebRequest -Uri "https://github.com/andreakarasho/ClassicUO/releases/download/ClassicUO-dev-preview/ClassicUO-dev-preview-release.zip" -OutFile "$WorkingDir\ClassicUO-dev-preview-release.zip" -Verbose
Write-Host "..done`n" -ForegroundColor Cyan

# Now download the latest Razor build from GitHub
Write-Host "Downloading the latest dev build of Razor.." -ForegroundColor Cyan
Invoke-WebRequest -Uri "https://github.com/markdwags/Razor/releases/download/Razor-dev-preview/Razor-dev-preview.zip" -OutFile "$WorkingDir\Razor-dev-preview.zip" -Verbose
Write-Host "..done`n" -ForegroundColor Cyan

# Extract both ClassicUO and Razor into the folder this script is running in
Write-Host "Extracting ClassicUO and Razor"
Expand-Archive -Path "$WorkingDir\ClassicUO-dev-preview-release.zip" -DestinationPath "$WorkingDir\" -Force
Expand-Archive -Path "$WorkingDir\Razor-dev-preview.zip" -DestinationPath "$WorkingDir\Razor\" -Force
Write-Host "..done`n"

# For some users, the files are "Blocked" and require to be unblock to run Razor with CUO correctly. Unblock both folders just in case.
Write-Host "Unblocking ClassicUO and Razor files.."
Get-ChildItem -Path "$WorkingDir\" | Unblock-File
Get-ChildItem -Path "$WorkingDir\Razor\" | Unblock-File
Write-Host "..done`n"

# If the settings.json file for ClassicUO doesn't exist, run ClassicUO so it auto-generates
if ($(Test-Path "$WorkingDir\settings.json" -PathType Leaf) -eq $False) {

    # Launch ClassicUO to generate default file
    Write-Host "Couldn't find settings.json, launching ClassicUO to create a new one.`n" -ForegroundColor Red
    Write-Host "*** PLEASE CLICK THE OK WHEN PROMPTED BY CLASSICUO ***"  -ForegroundColor Red
    Start-Process -FilePath "$WorkingDir\ClassicUO.exe" -Wait

    Write-Host "Setting up settings.json`n" -ForegroundColor Green

    # User input for the various data points for settings.json  
    if ($useDefaults -eq $true) {
        $serverhost = "login.uorenaissance.com"
        $port = 2593  
        $client = "5.0.8.3"
    }
    else {

        do {
            $serverhost = Read-Host -Prompt "> Enter the host/ip of the UO server (ie: server.name.com)"
        } until ([string]::IsNullOrEmpty($serverhost) -eq $false)
  
        do {
            $port = Read-Host -Prompt "> Enter the port of the UO server (ie: 2593)"
        } until ([string]::IsNullOrEmpty($port) -eq $false)

    
        do {
            $client = Read-Host -Prompt "> Enter the client version you are using (ie: 7.0.0.0 or 5.0.0.0)"  
        } until ([string]::IsNullOrEmpty($client) -eq $false)  

        
    }

    do {
        $path = Read-Host -Prompt "> Enter the path to your Ultima Online files (ie: C:\Ultima Online)" 

        # Do a basic check to ensure its the right folder
        if ($(Test-Path "$path\art.mul" -PathType Leaf) -eq $false) {
            Write-Host "** That folder doesn't contain art.mul, a core Ultima Online client file. You most likely didn't enter the correct folder. Please use the folder that contains UO art/sound files." -ForegroundColor Red
            $path = ""
        }

    } until ([string]::IsNullOrEmpty($path) -eq $false)
  
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
    Write-Host "`nSaving input to settings.json`n" -ForegroundColor Green
    $cuoSettings | ConvertTo-Json -Depth 32 | Set-Content "$WorkingDir\settings.json"

}
else {

    Write-Host "settings.json exists, confirming setting for Razor points to $WorkingDir\Razor\Razor.exe" -ForegroundColor Green

    $cuoSettings = Get-Content "$WorkingDir\settings.json" -Raw | ConvertFrom-Json

    if ($cuoSettings."plugins"[0] -ne "$WorkingDir\Razor\Razor.exe") {
    
        Write-Host "Razor path not set correctly, updating settings.json" -ForegroundColor Red
        # Set plugin location to point to Razor
        $cuoSettings."plugins"[0] = "$WorkingDir\Razor\Razor.exe"

        $cuoSettings | ConvertTo-Json -Depth 32 | Set-Content "$WorkingDir\settings.json"
    }
    else {
        Write-Host "Razor path is correctly, no changes made" -ForegroundColor Green
    }

}

# Let's create a shortcut on the Desktop if one doesn't exist
if ($(Test-Path "$Home\Desktop\ClassicUO.lnk" -PathType Leaf) -eq $False) {
    $createShortcut = Read-Host -Prompt "> Would you like create a shortcut on your Desktop? [y or n]`n"

    if ($createShortcut -eq "Y" -Or $createShortcut -eq "y") {
        $WshShell = New-Object -comObject WScript.Shell
        $shortcut = $WshShell.CreateShortcut("$Home\Desktop\ClassicUO.lnk")
        $shortcut.TargetPath = "$WorkingDir\ClassicUO.exe"
        $shortcut.WorkingDirectory = "$WorkingDir"
        $shortcut.Save()
    }  
}

Write-Host "The latest developer version of ClassicUO and Razor have been installed or updated.`n" -ForegroundColor Green

# Should we launch ClassicUO now?
$launch = Read-Host -Prompt "> Would you like to launch ClassicUO with Razor? [y or n]"

if ($launch -eq "Y" -Or $launch -eq "y") {
    Write-Host "Launching ClassicUO..." -ForegroundColor Cyan
    Start-Process -FilePath "$WorkingDir\ClassicUO.exe"
}

Write-Host "`nPress ENTER to exit or close this window`n" -ForegroundColor Green
Read-Host
