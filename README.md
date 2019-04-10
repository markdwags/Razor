![Razor Logo](https://imgur.com/jTtHLVF.png)

# Razor: An Ultima Online Assistant

Razor is a free tool designed to help with simple tasks while playing Ultima Online.

## UO:R Community Edition

The purpose and goal of this project is to revive and continue development of the abandoned Razor project and focus on "quality of life" improvements while keeping the spirit of Razor intact.

For release notes, download links and other information go to http://www.uor-razor.com.

This version is currently being refactored to work with [ClassicUO](https://github.com/andreakarasho/ClassicUO).

## History

Razor was originally designed by Bryan Pass, known as Zippy in the RunUO community as a replacement to UOAssist. Based on commit notes, active development on Razor ceased some time in the early 2010's with the source code being released in 2014.

The code initially didn't include Crypt.dll and Loader.dll which are required to full integration with UO. At some point, the code was released into the same GitHub repo.

This version of Razor is based off of 1.4.0.3, which is simply version 1.0.13.13 updated from .NET 2.0 to .NET 4.0. Another version of Razor exists (the 1.0.14.x versions) and is/was maintained by another private shard that make some enhancements, notable around targeting. This version of Razor has incorporated the majority of changes you can find in those versions.

The original project was last updated May 2nd, 2014 and while over 50 forks exist on GitHub, none have been active or have made significant changes except for a few exceptions.

  * **Jaedan** (which this version is based on) who updated the project to compile and work in Visual Studio 2017 and made improvements to Crypt.dll that enabled this project to move forward.
  * **uoitalia** who made several performance changes from generic to non-generic collections that I incorporated in the first release.

## Installing

This version of Razor doesn't use the Windows Registry or %AppData%\Razor to store configuration, macros and profiles. In other words, it's portable. To install, follow these simple steps:

  * Download the latest release
  * Extract into any location (ie: C:\UO\Razor)
  * (Optional) If you have an existing version of Razor, copy the Profiles and Macros folders from %AppData%\Razor into the folder you extracted the zip above to. If you have custom counters, also copy counters.xml over.
  * Right-Click on the Razor shortcut or Razor.exe, go to Properties. On the Compatibility tab, select Run this program in compatibility mode for and select Windows XP SP2.
  * Run Razor either via the shortcut or clicking the Razor.exe directly.
  * Razor should prompt to Run As Administrator. You must allow this for Razor to function correctly.  

If you need more help with Razor, [please go here](http://uor-razor.com/help) and review the documentation.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests.

## License

This work is released under the GPLv3 license. This project does not distribute any copyrighted game assets. In order to run this application you'll need to legally obtain a copy of the Ultima Online Classic Client. See the [LICENSE.md](LICENSE.md) file for details.