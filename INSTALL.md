# Installation

These steps assume that you already have obtained and installed the Ultima Online game asset files in the local system and want to install ClassicUO and Razor for the first time.

## Install: New ClassicUO with Razor Install

* Create a new folder on your local computer (ie: C:\Games\ClassicUO)
* Open PowerShell prompt and go to folder you created
* Execute the following PowerShell commands

```powershell
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString(
'https://raw.githubusercontent.com/markdwags/Razor/master/InstallClassicUOAndRazor-NoDefaults.ps1'))
```

* Follow the on-screen prompts to set up the ClassicUO settings.json file first the first time.

## Install: Existing ClassicUO and/or Standard Client

These steps assume that you already have obtained and installed the Ultima Online game asset files in the local system and are already using ClassicUO or the standard client.

### Standard Client & ClassicUO Client

* Download the latest release
* Extract into any location (ie: C:\UO\Razor)
* (Optional) If you have an existing version of Razor, copy the Profiles and Macros folders from %AppData%\Razor into the folder you extracted the zip above to. If you have custom counters, also copy counters.xml over.

### If using the standard client

* Right-Click on the Razor shortcut or Razor.exe, go to Properties. On the Compatibility tab, select Run this program in compatibility mode for and select Windows XP SP2.
* Run Razor either via the shortcut or clicking the Razor.exe directly.
* Razor should prompt to Run As Administrator. You must allow this for Razor to function correctly.  

### If using ClassicUO

* Go to the folder where you have ClassicUO installed and edit settings.json.

```json
  "plugins": [
    ".\\Assistant\\Razor.dll"
  ]
```

* Find the section above and edit the path to point to where you extract Razor in the above step. For example, if I extracted it to **C:\UO\Razor** my settings.json would look like this:

```json
  "plugins": [
    "C:\\UO\\Razor\\Razor.exe"
  ]
```

* When you launch ClassicUO, Razor will load up during the startup process.

## Help

If you need more help with using Razor, [please go here](http://uor-razor.com/help) and review the documentation or use the link above to join Discord.