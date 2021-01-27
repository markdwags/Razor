# Download Razor

This version of Razor is portable. All files (Profiles, Macros, etc) are contained within the same folder as Razor and can be transferred with a simple copy/paste. Simply download the version you're looking for and extract into a new folder and run it from there.

To learn more about each version, view the [release notes](releasenotes.md).

!!! warning "Client Support"
    It's recommended that you use the latest version of the [ClassicUO](https://github.com/andreakarasho/ClassicUO) client with this version of Razor, however it should work with the original 5.x and 7.x clients though support for any bugs may be limited.

    When using ClassicUO, some features in Razor are disabled since they either won't work and/or may cause conflicts with ClassicUO.

!!! tip "Razor Installer"
    Use the [Razor Installer](../install/windows/) to get Razor and ClassicUO installed with a single click.

## Razor with ClassicUO Support

| Razor Version                                                                |
| ---------------------------------------------------------------------------- |
| [Razor v1.6.8.15](https://github.com/markdwags/Razor/releases/tag/v1.6.8.15) |
| [Razor v1.6.7.10](https://github.com/markdwags/Razor/releases/tag/v1.6.7.10) |
| [Razor v1.6.6.15](https://github.com/markdwags/Razor/releases/tag/v1.6.6.15) |
| [Razor v1.6.5.16](https://github.com/markdwags/Razor/releases/tag/v1.6.5.16) |
| [Razor v1.6.4.32](https://github.com/markdwags/Razor/releases/tag/v1.6.4.32) |
| [Razor v1.6.4.2](https://github.com/markdwags/Razor/releases/tag/v1.6.4.2)   |
| [Razor v1.6.2.23](https://github.com/markdwags/Razor/releases/tag/1.6.2.23)  |
| [Razor v1.6.1.32](https://github.com/markdwags/Razor/releases/tag/v1.6.1.32) |
| [Razor v1.6.0.57](https://github.com/markdwags/Razor/releases/tag/v1.6.0.57) |

### Developer Preview

This is built automatically after each commit to master, so it may contain bugs and/or unfinished features but is generally considered stable.

| Link                                                                                         | Description                                                                                                                                    |
| -------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| [Razor Developer Preview](https://github.com/markdwags/Razor/releases/tag/Razor-dev-preview) | This is built automatically after each commit to master, so it may contain bugs and/or unfinished features but is generally considered stable. |

## Razor with OSI Client Only Support

| Razor Version                                                                | Hash |
| ---------------------------------------------------------------------------- | ---- |
|[Razor v1.5.0.16](http://www.razorce.com/Razor_UOR_CE-1.5.0.16.zip)|`SHA256: 63D0B617FCE217C067A3270323C9E58B63F379F401B4224E0EA937DAA7871B8B`|
|[Razor v1.5.0.15](http://www.razorce.com/Razor_UOR_CE-1.5.0.15.zip)|`SHA256: 64916F16A72FDE5E9C17B3A180001A896E9472458C8DB69E09DC7E37D78A6B30`|
|[Razor v1.5.0.14](http://www.razorce.com/Razor_UOR_CE-1.5.0.14.zip)|`SHA256: 1D352F7814311FDBFC3EF16DEAD6664562C85B3817D953112F687099A98D104A`|
|[Razor v1.5.0.13](http://www.razorce.com/Razor_UOR_CE-1.5.0.13.zip)|`SHA256: 090D753820B791E115532E96703DE1650B4DB0CE88191355D0F65A5799A51571`|
|[Razor v1.5.0.12](http://www.razorce.com/Razor_UOR_CE-1.5.0.12.zip)|`SHA256: 1AC1DAFBBDEE3DBEB1D031E63CAF904D43B60A05E98CB83ECA4872F892BD4F36`|
|[Razor v1.5.0.11](http://www.razorce.com/Razor_UOR_CE-1.5.0.11.zip)|`SHA256: 5945E5F9D5C87FEF552881C319167BD4ED012AE01D31FA855449B034129F7225`|
|[Razor v1.5.0.10](http://www.razorce.com/Razor_UOR_CE-1.5.0.10.zip)|`SHA256: 80FACEE8DB005E5CB7A89EFEBEE4BEE2DA242C0BF9AFA31B20ADEBEC44ED7FEF`|
|[Razor v1.5.0.9](http://www.razorce.com/Razor_UOR_CE-1.5.0.9.zip)|`SHA256: 63158C8987BA0E7FBBA5917018595617830CF7B72A699A50A34F79A943365EE0`|
|[Razor v1.5.0.8](http://www.razorce.com/Razor_UOR_CE-1.5.0.8.zip)|`SHA256: 0D25D01C85CFC8BA51D4FFEEBF59A3DC23B2400850A4B41C613DFC50AFAD5487`|
|[Razor v1.5.0.7](http://www.razorce.com/Razor_UOR_CE-1.5.0.7.zip)|`SHA256: BD239C8F10FB80C3D1F6D185557679A0FCCF0CE35B2DB6D726B0DB0DB8BE7B7A`|

### Experimental

This version of Razor is identical to version `1.5.0.16` except with one major change to how Razor figures our your position to address the Razor "desync" issue.

Some users have reported issues when logging in so that is why this version is marked as experimental. If you experience issues, please revert to 1.5.0.16.

| Razor Version                                                                | Hash |
| ---------------------------------------------------------------------------- | ---- |
|[Razor v1.5.0.17](http://www.razorce.com/Razor_UOR_CE-1.5.0.17.zip)|`SHA256: D26E8B887FC26B94FB5B0C50530BE07A4393783CCBEAA9C3FD5B38171A857571`|

# Validate Checksum

* Windows (using Powershell)

```powershell
Get-FileHash '.\Razor-1.x.x.x.zip' -Algorithm SHA25
```

* Mac

```bash
shasum -a 256 /path/to/Razor-1.x.x.x.zip' -Algorithm SHA256
```

* Linux
  
```bash
sha256sum /path/to/Razor-1.x.x.x.zip
```
