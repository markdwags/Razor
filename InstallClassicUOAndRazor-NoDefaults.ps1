<#
.SYNOPSIS
  A basic Powershell script to download ClassicUO and Razor
  and configure the basic settings.
.NOTES
  Version:        1.3
  Author:         Quick
  Creation Date:  5/1/2019
#>

# Set TLS to be v1.2 or Invoke-WebRequest will fail
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Set the directory to be the one the command was issued in
$WorkingDir = $(Get-Location).Path

Write-Host "Install ClassicUO & Razor - v1.3" -ForegroundColor Yellow
Write-Host "Author: Quick (https://github.com/markdwags/Razor)`n" -ForegroundColor Yellow

# Check if they are running 64 bit version of Windows
if ((Get-WmiObject Win32_OperatingSystem).OSArchitecture.Contains("64") -ne $true) {
    Write-Host "************************" -ForegroundColor Red
    Write-Host "ClassicUO requires that you have a 64bit OS. It will fail to run without it." -ForegroundColor Red    
    Write-Host "************************" -ForegroundColor Red

    Write-Host "`nPress ENTER to exit or close this window`n" -ForegroundColor Red
    Read-Host

    Exit
}

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

# Setup for the ability to install the latest release or the latest developer preview
$LatestCUO = ""
$LatestCUODownloadUrl = ""
$LatestCUOFileName = "ClassicUO-dev-preview-release.zip"
$LatestCUOReleaseUpdated = Get-Date
$LatestCUODevPreviewUpdated = Get-Date

$LatestRazor = ""
$LatestRazorDownloadUrl = ""
$LatestRazorFileName = "Razor-dev-preview.zip"
$LatestRazorReleaseUpdated = Get-Date
$LatestRazorDevPreviewUpdated = Get-Date

$ForceDevPreview = $false

# Get the latest version info for ClassicUO
# https://api.github.com/repos/andreakarasho/ClassicUO/releases
try {
    $response = Invoke-RestMethod -Uri "https://api.github.com/repos/andreakarasho/ClassicUO/releases" -Method Get

    $LatestCUO = $response[0].tag_name
    $LatestCUODownloadUrl = $response[0].assets.browser_download_url
    $LatestCUOFileName = $response[0].assets.name
    $LatestCUOReleaseUpdated = $response[0].assets.updated_at

    $LatestCUODevPreviewUpdated = $response[1].assets.updated_at

    #Write-Host "Latest ClassicUO Release: $LatestCUO" -ForegroundColor Cyan
} 
catch {        
    Write-Host "************************" -ForegroundColor Red
    Write-Host "Unable to get the latest Razor release version, will only download dev preview" -ForegroundColor Red
    Write-Host "************************" -ForegroundColor Red

    $ForceDevPreview = $true
}   

# Get the latest version info for Razor
# https://api.github.com/repos/markdwags/Razor/releases
try {
    $response = Invoke-RestMethod -Uri "https://api.github.com/repos/markdwags/Razor/releases" -Method Get

    $LatestRazor = $response[1].tag_name
    $LatestRazorDownloadUrl = $response[1].assets.browser_download_url
    $LatestRazorFileName = $response[1].assets.name
    $LatestRazorReleaseUpdated = $response[1].assets.updated_at

    $LatestRazorDevPreviewUpdated = $response[0].assets.updated_at

    #Write-Host "Latest Razor Release: $LatestRazor`n" -ForegroundColor Cyan
} 
catch {        
    Write-Host "************************" -ForegroundColor Red
    Write-Host "Unable to get the latest Razor release version, will only download dev preview" -ForegroundColor Red
    Write-Host "************************" -ForegroundColor Red   

    $ForceDevPreview = $true
}

# Use UOR defaults?
$useDefaults = $false

if ($WorkingDir.ToLower().StartsWith("c:\program files")) {
    Write-Host "************************" -ForegroundColor Red
    Write-Host "It's recommended that you DO NOT install this into 'C:\Program Files' or 'C:\Program Files (x86)'" -ForegroundColor Red
    Write-Host "This will force you to run ClassicUO as an Administator, which isn't necessary. You also might experience of file related permission issues" -ForegroundColor Red
    Write-Host "This script won't stop you, but it really recommends that you pick another folder like 'C:\Games\ClassicUO' or simply 'C:\ClassicUO'" -ForegroundColor Red
    Write-Host "************************" -ForegroundColor Red

    Read-Host
}

# Lets just talk about what we're doing
Write-Host "This script will either install ClassicUO & Razor for the first time or update your existing versions.`n"

Write-Host "This script will perform the following steps, so please review first:`n" -ForegroundColor Red

Write-Host "1. Download the latest ClassicUO release or dev preview build to: $WorkingDir" -ForegroundColor Green
Write-Host "2. Download the latest Razor release or dev preview to: $WorkingDir\Razor" -ForegroundColor Green
Write-Host "3. Help with setting up the initial settings.json file if it doesn't exist, or confirm your existing one is configured correctly for Razor`n" -ForegroundColor Green

# Let's see if we're updating or installing for the first time
if ($(Test-Path "$WorkingDir\ClassicUO.exe" -PathType Leaf) -eq $False) {    
    $proceed = Read-Host -Prompt "> [NEW INSTALL] Would you like to continue and install into '$($WorkingDir)'? [y or n]"
}
else {    
    Write-Host "** NOTE: When upgrading, your ClassicUO and Razor profiles/macros/settings will NOT be touched.`n" -ForegroundColor Cyan
    $proceed = Read-Host -Prompt "> [UPGRADE] Would you like to continue and upgrade currently installed in '$($WorkingDir)'? [y or n]"

    if ($proceed -ne "Y" -Or $proceed -ne "y") { Exit }
    
    $backup = Read-Host -Prompt "> [UPGRADE] Would you like backup your existing installation? [y or n]"

    if ($backup -eq "Y" -Or $backup -eq "y") { 
        try {
            $backupDate = $(Get-Date).ToString("mmddyy-hhmmss")
            Write-Host "Creating ZIP backup of $WorkingDir" -ForegroundColor Green
    
            $exclude = @("*.zip")
            $filesToBackup = Get-ChildItem -Path $WorkingDir -Exclude $exclude
            Compress-Archive -Path $filesToBackup -DestinationPath "$WorkingDir\backup_$backupDate.zip" -CompressionLevel Fastest
            Write-Host "..done | Created $WorkingDir\backup_$backupDate.zip" -ForegroundColor Green
        }
        catch {
            Write-Host "************************" -ForegroundColor Red
            Write-Host "Unable to create backup, most likely a file in use." -ForegroundColor Red
            Write-Host "************************" -ForegroundColor Red   
        }        
    }    
}

if ($proceed -ne "Y" -Or $proceed -ne "y") { Exit }

# Set default
$downloadOption = "1"

if ($ForceDevPreview -eq $false) {
    Write-Host "`nYou have the option to download either the latest official release or the latest developer preview builds of ClassicUO and Razor.`n"
    Write-Host "1) The latest developer preview of ClassicUO and Razor. This is the default and recommended option.`n" -ForegroundColor Yellow 

    $span = $(New-TimeSpan -Start $LatestCUODevPreviewUpdated -End $(Get-Date))
    Write-Host "* ClassicUO Dev Preview | Updated $($span.Days)d $($span.Hours)h $($span.Minutes)m $($span.Seconds)s ago" -ForegroundColor Magenta 

    $span = $(New-TimeSpan -Start $LatestRazorDevPreviewUpdated -End $(Get-Date))
    Write-Host "* Razor Dev Preview | Updated $($span.Days)d $($span.Hours)h $($span.Minutes)m $($span.Seconds)s ago`n" -ForegroundColor Magenta 

    Write-Host "2) The latest official release of ClassicUO and Razor. For those who are willing to wait and want a bit more stability in their life.`n" -ForegroundColor Yellow   

    $span = $(New-TimeSpan -Start $LatestCUOReleaseUpdated -End $(Get-Date))
    Write-Host "* ClassicUO Release ($LatestCUO) | Updated $($span.Days)d $($span.Hours)h $($span.Minutes)m $($span.Seconds)s ago" -ForegroundColor Magenta 

    $span = $(New-TimeSpan -Start $LatestRazorReleaseUpdated -End $(Get-Date))
    Write-Host "* Razor Release ($LatestRazor) | Updated $($span.Days)d $($span.Hours)h $($span.Minutes)m $($span.Seconds)s ago`n" -ForegroundColor Magenta 
    
    $downloadOption = Read-Host -Prompt "> Which version would you like to download? [1 or 2]"
}

if ($downloadOption -eq "1") {
    # Let's get the dev preview version 

    # First, let's get the latest ClassicUO build from GitHub
    Write-Host "Downloading the latest dev build of ClassicUO.." -ForegroundColor Cyan
    Invoke-WebRequest -Uri "https://github.com/andreakarasho/ClassicUO/releases/download/ClassicUO-dev-preview/ClassicUO-dev-preview-release.zip" -OutFile "$WorkingDir\ClassicUO-dev-preview-release.zip" -Verbose
    Write-Host "..done`n" -ForegroundColor Cyan    

    # Now download the latest Razor build from GitHub
    Write-Host "Downloading the latest dev build of Razor.." -ForegroundColor Cyan
    Invoke-WebRequest -Uri "https://github.com/markdwags/Razor/releases/download/Razor-dev-preview/Razor-dev-preview.zip" -OutFile "$WorkingDir\Razor-dev-preview.zip" -Verbose
    Write-Host "..done`n" -ForegroundColor Cyan

    $LatestCUOFileName = "ClassicUO-dev-preview-release.zip"
    $LatestRazorFileName = "Razor-dev-preview.zip"
}
elseif ($downloadOption -eq "2") {
    # Let's get the latest release version
    
    Write-Host "Downloading the latest release version $LatestCUO of ClassicUO.." -ForegroundColor Cyan
    Invoke-WebRequest -Uri $LatestCUODownloadUrl -OutFile "$WorkingDir\$LatestCUOFileName" -Verbose
    Write-Host "..done`n" -ForegroundColor Cyan

    Write-Host "Downloading the latest release version $LatestRazor of Razor.." -ForegroundColor Cyan
    Invoke-WebRequest -Uri $LatestRazorDownloadUrl -OutFile "$WorkingDir\$LatestRazorFileName" -Verbose
    Write-Host "..done`n" -ForegroundColor Cyan
}
else {
    Write-Host "That wasn't an option. Something isn't right." -ForegroundColor Red

    Exit
}

# Extract both ClassicUO and Razor into the folder this script is running in
Write-Host "Extracting ClassicUO and Razor"
Expand-Archive -Path "$WorkingDir\$LatestCUOFileName" -DestinationPath "$WorkingDir\" -Force
Expand-Archive -Path "$WorkingDir\$LatestRazorFileName" -DestinationPath "$WorkingDir\Razor\" -Force
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
    Write-Host "*** PLEASE CLICK THE OK WHEN PROMPTED BY CLASSICUO ***`n"  -ForegroundColor Red
    Start-Process -FilePath "$WorkingDir\ClassicUO.exe" -Wait

    if ($(Test-Path "$WorkingDir\settings.json" -PathType Leaf) -eq $False) {
        Write-Host "*** settings.json failed to generate, something isn't right ***`n"  -ForegroundColor Red
        Exit
    }

    Write-Host "Setting up ClassicUO's 'settings.json' file`n" -ForegroundColor Green

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
            Write-Host "** $path doesn't contain art.mul, a core Ultima Online client file. That must not be the correct folder. Please use the folder that contains the UO art/sound files." -ForegroundColor Red
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
    Write-Host "`nWriting new values to 'settings.json'`n" -ForegroundColor Yellow

    Write-Host "- IP: $($cuoSettings."ip")" -ForegroundColor DarkYellow
    Write-Host "- Port: $($cuoSettings."port")" -ForegroundColor DarkYellow
    Write-Host "- Ultima Online Directory: $($cuoSettings."ultimaonlinedirectory")" -ForegroundColor DarkYellow
    Write-Host "- Client Version: $($cuoSettings."clientversion")" -ForegroundColor DarkYellow

    $cuoSettings | ConvertTo-Json -Depth 32 | Set-Content "$WorkingDir\settings.json"

    Write-Host "`n..done`n" -ForegroundColor Yellow
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
else {
    Write-Host "Found shortcut.`n" -ForegroundColor Green    
}

Write-Host "The latest developer version of ClassicUO and Razor have been installed or updated.`n" -ForegroundColor Green

# Should we launch ClassicUO now?
$launch = Read-Host -Prompt "> Would you like to launch ClassicUO with Razor? [y or n]"

if ($launch -eq "Y" -Or $launch -eq "y") {
    Write-Host "Launching ClassicUO..." -ForegroundColor Cyan
    Start-Process -FilePath "$WorkingDir\ClassicUO.exe"
}

Write-Host "`nPress ENTER to exit or close this window" -ForegroundColor Green
Read-Host
