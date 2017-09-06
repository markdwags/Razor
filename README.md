# Razor

This is a fork of the Razor assistant from https://github.com/msturgill/razor. The goal of this fork is to
make the code base build and run with minimal effort.

# Building

This project is currently Windows only. You'll need Visual Studio 2017. The free community edition is fine.

You'll need to grab the UltimaSDK, which is included as a submodule. You can do this by running

~~~{.sh}
git submodule update --init
~~~

Then:

* Open Razor.sln
* Select your configuration (default to "Release")
* Hit F5 to build. The output is in bin/Win32/[Release|Debug]
* Run Razor.exe

