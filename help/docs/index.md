![razor](images/razor.png)

# Razor (UO:R Community Edition)

[![Build status](https://ci.appveyor.com/api/projects/status/1815mo6is2t8fl1o?svg=true)](https://ci.appveyor.com/project/markdwags/razor)
[![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

Razor is a free tool designed to help with simple tasks while playing Ultima Online.

This guide was written for the **Razor UO:Renaissance Community Edition**.

For more information about this version of Razor, read the [history](#history), review the [release notes](./releasenotes.md) and [download](./download.md) Razor.

# Overview

## Project Goal

The goal of this project is to revive and continue development and maintenance of the abandoned Razor project, focusing on *quality of life* improvements while attempting to keep the spirit and vision the original developers had for Razor and not driving down the path of advanced automation and scripting that's found in other UO assistants.

## History

Razor was originally designed by Bryan Pass, known as Zippy in the RunUO community as a replacement to UOAssist. Based on commit notes, active development (new features, bug fixes) on Razor ceased some time in the early 2010's with the source code being released in 2014.

The code initially didn't include `Crypt.dll` and `Loader.dll` which are required to fully integrate with the UO client. At some point, the code for those projects released into the same [GitHub repo](https://github.com/msturgill/razor).

The original project was last updated May 2nd, 2014 which was simply an update from .NET 2.0 to .NET 4.0 and while over 50 forks exist on GitHub, none of them have been active or have made significant changes except for a few exceptions:

* `Jaedan` (which this version is based on) who updated the project to compile and work in Visual Studio 2017 and made improvements to Crypt.dll that enabled this project to move forward.
* `SaschaKP` who made several performance changes from generic to non-generic collections that I incorporated in the first release.

I have been actively maintaining this project since early April 2018 and based this version off of `1.4.0.3`, which was simply the version `1.0.13.13` updated to .NET 4.0.

Another version of Razor exists (the `1.0.14.x` versions) and is/was maintained by another private shard that make some enhancements, notable around targeting. This version of Razor has long since incorporated the majority of changes you can find in that version.

In June 2019 integration into [ClassicUO](https://github.com/andreakarasho/ClassicUO) was officially established.

## UO:R Community Edition

When I started this project back in early 2018, nearly all the [feedback, ideas, discussion](http://www.uorforum.com/threads/improving-razor-razor-development.33134/) and [testing](http://www.uorforum.com/threads/razor-1-5-bug-issue-release-tracking.33405/) has came from the [UO:Renaissance](http://www.uorenaissance.com) community where the rules there only allow for the use of Razor and so that name was used to not only distinguish between other versions of Razor that are available but give credit to a community that provided so much support early on.

Since then, this version of Razor was updated to support the ClassicUO client with feedback and contributions coming from  all different corners of the freeshard UO community -- from large to small shards.

Thank you to all the folks across the whole community who have contributed in some way towards creating this version of Razor. If you'd like contribute, see the [CONTRIBUTING](https://github.com/markdwags/Razor/blob/master/CONTRIBUTING.md) file for more information.

**TL;DR:** If you want to use this version of Razor, regardless of the Ultima Online server you play on, this version should work. This version isn't tied to any specific shard. These updates to Razor are for all the Ultima Online Community to use and benefit from. Play UO on the shard that gives you the most enjoyment.

For me, that shard is [UO:Renaissance](http://www.uorenaissance.com).

# Help

If you're unable to find a solution using the information here or you'd like to submit a feature request or bug report, use the following resources.

* Submit a feature/bug/issue on our [official Razor GitHub Repo](https://github.com/markdwags/Razor/issues).
* Join us in #razor on Discord (this is the official [ClassicUO](https://www.classicuo.eu/) Discord server)

[![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

For more information about the Razor Scripting Engine, go [here](http://www.uor-razor.com/guide/).

# Other

All work is released under the GPLv3 license. This project does not distribute any copyrighted game assets. In order to run this application you'll need to legally obtain a copy of the Ultima Online Classic Client.

See the [LICENSE](https://github.com/markdwags/Razor/blob/master/LICENSE.md) file for details.
