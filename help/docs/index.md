![razor](images/razor.png)

# Razor Community Edition

[![Build status](https://ci.appveyor.com/api/projects/status/1815mo6is2t8fl1o?svg=true)](https://ci.appveyor.com/project/markdwags/razor)
[![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

Razor is a free tool designed to help with simple tasks while playing Ultima Online.

This guide was written for the **Razor Community Edition**.

For more information about this version of Razor, read the [history](#history), review the [release notes](./releasenotes.md) and [download](./download.md) Razor.

# Overview

## Project Goal

The goal of this project was to revive and continue development and maintenance of the abandoned Razor project, focusing on *quality of life* improvements while attempting to keep the spirit and vision the original developers had for Razor by not driving down the path of advanced automation that's found in other UO assistants.

## History

Razor was originally designed by Bryan Pass, known as Zippy in the RunUO community as a replacement to UOAssist. Based on commit notes, active development (new features, bug fixes) on Razor ceased some time in the early 2010's with the source code being released in 2014.

The code initially didn't include `Crypt.dll` and `Loader.dll` which are required to fully integrate with the UO client. At some point, the code for those projects released into the same [GitHub repo](https://github.com/msturgill/razor).

The original project was last updated May 2nd, 2014 which was simply an update from .NET 2.0 to .NET 4.0 and while over 50 forks exist on GitHub, none of them have been active or have made significant changes except for a few exceptions:

* `Jaedan` (which this version is based on) who updated the original Razor project to compile and work in Visual Studio 2017 and made improvements to `Crypt.dll` that enabled this project to move forward.
* `SaschaKP` who made several performance changes from generic to non-generic collections that I incorporated in the first release.

I have been actively maintaining this project since early April 2018 and based this version off of `1.4.0.3`, which was simply the version `1.0.13.13` updated to .NET 4.0.

Another closed source version of Razor exists (the `1.0.14.x` versions) that was maintained by a private shard that made some enhancements, notably around targeting. This version of Razor has long since incorporated the majority of changes you can find in that version.

In June 2019 integration into [ClassicUO](https://github.com/andreakarasho/ClassicUO) was officially established.

In March 2020 the [Razor Scripting Engine](http://www.razorce.com/guide/) was added to make it easier to read, create, edit and share Razor scripts while staying true to the project's state goals.

## Community Edition

When I started this project back in early 2018, nearly all the [feedback, ideas, discussion](https://www.uorforum.com/threads/improving-razor-razor-development.33134/) and [testing](https://www.uorforum.com/threads/razor-1-5-bug-issue-release-tracking.33405/) came from the [UO:Renaissance](http://www.uorenaissance.com) community, one of the only freeshards where the rules only allow for the use of Razor. The original **UO:R Community Edition** name was used to not only distinguish between other versions of Razor that were available but give credit to a community that provided so much support early on.

As the popularity of this version grew, especially after the implementation of ClassicUO support, it became widely used across the whole UO freeshard community, from small private shards to some of the largest shards in the community and continues to be built on the feedback and contributions from all different corners of the community.

With that said, I find it more appropriate to simply call this version the **Razor Community Edition**.

Thank you to everyone across the whole UO freeshard community who has contributed to this version of Razor in some way.

If you'd like contribute, see the [CONTRIBUTING](https://github.com/markdwags/Razor/blob/master/CONTRIBUTING.md) file for more information.

# Help

If you're unable to find a solution using the information here or you'd like to submit a feature request or bug report, use the following resources.

* Submit a feature/bug/issue on our [official Razor GitHub Repo](https://github.com/markdwags/Razor/issues).
* Join us in #razor on Discord (we use the the official [ClassicUO](https://www.classicuo.eu/) Discord server)

[![Chat on Discord status](https://img.shields.io/discord/458277173208547350.svg?logo=discord)](https://discord.gg/VdyCpjQ)

For more information about the Razor Scripting Engine, go [here](http://www.razorce.com/guide/).

# Other

All work is released under the GPLv3 license. This project does not distribute any copyrighted game assets. In order to run this application you'll need to legally obtain a copy of the Ultima Online Classic Client.

See the [LICENSE](https://github.com/markdwags/Razor/blob/master/LICENSE.md) file for details.
