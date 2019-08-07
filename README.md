![Razor Logo](https://imgur.com/jTtHLVF.png)

[![Build status](https://ci.appveyor.com/api/projects/status/1815mo6is2t8fl1o?svg=true)](https://ci.appveyor.com/project/markdwags/razor) [![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

 # Razor: An Ultima Online Assistant

Razor is a free tool designed to help with simple tasks while playing Ultima Online.

## UO:R Community Edition

The purpose and goal of this project is to revive and continue development of the abandoned Razor project and focus on "quality of life" improvements without driving down the path of automation and advanced scripting that is available in other assistants.

This project also attempts to keep the spirit and vision of the original Razor intact the original developers had for Razor.

For release notes, download links and other information go to http://www.uor-razor.com.

This version currently supports both the standard Ultima Online client (5.x & 7.x) and the open source [ClassicUO](https://github.com/andreakarasho/ClassicUO).

## History

Razor was originally designed by Bryan Pass, known as Zippy in the RunUO community as a replacement to UOAssist. Based on commit notes, active development on Razor ceased some time in the early 2010's with the source code being released in 2014.

The code initially didn't include Crypt.dll and Loader.dll which are required to full integration with UO. At some point, the code was released into the same [GitHub repo](https://github.com/msturgill/razor).

This version of Razor is based off of 1.4.0.3, which is simply version 1.0.13.13 updated from .NET 2.0 to .NET 4.0. Another version of Razor exists (the 1.0.14.x versions) and is/was maintained by another private shard that make some enhancements, notable around targeting. This version of Razor has incorporated the majority of changes you can find in those versions.

The original project was last updated May 2nd, 2014 and while over 50 forks exist on GitHub, none have been active or have made significant changes except for a few exceptions.

  * [Jaedan](https://github.com/jaedan) (which this version is based on) who updated the project to compile and work in Visual Studio 2017 and made improvements to Crypt.dll that enabled this project to move forward.
  * [uoitalia](https://github.com/uoitalia) who made several performance changes from generic to non-generic collections that I incorporated in the first release.

## Versions

All version of Razor doesn't use the Windows Registry or %AppData%\Razor to store configuration, macros and profiles. In other words, it's portable.

* **1.5.0.0** to **1.5.0.16** only support the standard Ultima Online client.
* **1.5.0.17** has major changes to how Razor determines player position and only supports the standard Ultima Online client.
* **1.6.x.x** support both the standard client and [ClassicUO](https://github.com/andreakarasho/ClassicUO).

## Install: New ClassicUO with Razor Install

These steps assume that you already have obtained and installed the Ultima Online game asset files in the local system and want to install ClassicUO and Razor for the first time.

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

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests.

## License

This work is released under the GPLv3 license. This project does not distribute any copyrighted game assets. In order to run this application you'll need to legally obtain a copy of the Ultima Online Classic Client. See the [LICENSE](LICENSE) file for details.
