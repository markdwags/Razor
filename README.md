![Razor Logo](https://imgur.com/jTtHLVF.png)

[![Build status](https://ci.appveyor.com/api/projects/status/1815mo6is2t8fl1o?svg=true)](https://ci.appveyor.com/project/markdwags/razor) [![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

# Razor: An Ultima Online Assistant

Razor is a free tool designed to help with simple tasks while playing Ultima Online.

## UO:R Community Edition

The goal of this project is to revive and continue development and maintenance of the abandoned Razor project while focusing on "quality of life" improvements without driving down the path of automation and advanced scripting that is found in other UO assistants.

This project also attempts to keep the spirit and vision the original developers had for Razor.

It's recommended that you use the [ClassicUO](https://github.com/andreakarasho/ClassicUO) client with this version of Razor, however it will work with the original 5.x and 7.x clients.

For release notes, download links and other information go to http://www.uor-razor.com

## History

Razor was originally designed by Bryan Pass, known as Zippy in the RunUO community as a replacement to UOAssist. Based on commit notes, active development on Razor ceased some time in the early 2010's with the source code being released in 2014.

The code initially didn't include Crypt.dll and Loader.dll which are required to full integration with UO. At some point, the code was released into the same [GitHub repo](https://github.com/msturgill/razor).

This version of Razor is based off of 1.4.0.3, which is simply version 1.0.13.13 updated from .NET 2.0 to .NET 4.0. Another version of Razor exists (the 1.0.14.x versions) and is/was maintained by another private shard that make some enhancements, notable around targeting. This version of Razor has incorporated the majority of changes you can find in those versions.

The original project was last updated May 2nd, 2014 and while over 50 forks exist on GitHub, none have been active or have made significant changes except for a few exceptions.

  * [Jaedan](https://github.com/jaedan) (which this version is based on) who updated the project to compile and work in Visual Studio 2017 and made improvements to Crypt.dll that enabled this project to move forward.
  * [uoitalia](https://github.com/uoitalia) who made several performance changes from generic to non-generic collections that I incorporated in the first release.

Since April 2018, this project has been been active and for 10 months implemented many quality of life changes (see 1.5.0.x [release notes](http://www.uor-razor.com/#releasenotes)) and in February 2019 integration into [ClassicUO](https://github.com/andreakarasho/ClassicUO) was established.

Toss a coin to your [contributors](https://github.com/markdwags/Razor/graphs/contributors).

## Versions

All version of Razor doesn't use the Windows Registry or %AppData%\Razor to store configuration, macros and profiles. In other words, it's portable.

* **1.5.0.0** to **1.5.0.16** only support the standard Ultima Online client.
* **1.5.0.17** has major changes to how Razor determines player position and only supports the standard Ultima Online client.
* **1.6.x.x** support both the standard client and [ClassicUO](https://github.com/andreakarasho/ClassicUO).

## Razor Scripting Engine

Introduced in 1.6.3.x, Razor supports a command-based script language based on the lexer and parser by designed by [Jaedan](https://github.com/jaedan).

**NOTE:** This is not an implementation of the Steam or Razor Enhanced scripting languages or functionality into Razor. The scripting engine attempts to maintain existing Razor functionality while offering quality of life improvements over the existing macro system.

For a comprehensive guide on the language and syntax, visit the [Razor Scripting Guide](http://www.uor-razor.com/guide/).

## Installation & Help

For information on installing Razor, please read [INSTALL](INSTALL.md).

If you need more help with using Razor, [please go here](http://www.uor-razor.com/help) and review the documentation and/or join Discord and ask there.

[![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

## Contributing & Code of Conduct

Please read [CONTRIBUTING](CONTRIBUTING.md) for more information on how to contribute to this project.

Please note we have a [Code of Conduct](CODE_OF_CONDUCT.md), please follow it in all your interactions with this project.

## License

This work is released under the GPLv3 license. This project does not distribute any copyrighted game assets. In order to run this application you'll need to legally obtain a copy of the Ultima Online Classic Client. See the [LICENSE](LICENSE.md) file for details.
