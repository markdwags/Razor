# Portability

This version of Razor is completely portable and can be installed in any location. All profiles, scripts, macros are all stored within subfolders of `Razor.exe`.

# Migrating Existing Profiles

To migrate/move Profiles, Scripts and Macros between Razor installations, simply close out of all instances of Razor and copy/paste the following folders/files from your old Razor location to the new location:

- `Profiles`
- `Macros`
- `Scripts`
- `counters.xml`
- `Razor.exe.config` (only if you made changes)

# Application Layout

## Folders

### Profiles

When you open Razor for the first time, a set of default settings are created.

!!! tip "Multiple Profiles"
    It's **highly recommend** that have you have 1 profile per character. If you use the same profile among multiple characters while logged in, you could potentially lose changes to your profile. It's doable, but requires an extra bit a caution to ensure you keep all your profiles synced up.

### Macros

Macros are stored in the `Macros` folder and saved with the `.macro` file extension. These files can be read using any basic text editor.

!!! tip "Absolute Target Variables"
    Macros are scared across Profiles. To avoid making multiple macros that do the same thing, be sure to review [Absolute Target Variables](help/macros.md#absolute-target-variables)

!!! warning
    Editing macros within the file can be difficult beyond simply copy/pasting due to syntax. If you'd like the ability to edit scripts in a text editor, check out the [Razor Scripting Engine](guide/index.md).

### Scripts

Scripts are stored in the `Scripts` folder and saved with the `.razor` file extension. These files can be read using any basic text editor.

!!! tip "Razor Scripting Engine"
    Razor has a built-in editor to help with syntax, color coding and auto-completion. Review the [Razor Scripting Engine](guide/index.md) for more details.

## Files

| File               | Descripton                                                                                                                                                                                                                                                  |
| ------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `animdata.csv`     | This file contains the list of animations used when filtering different mobiles in                                                                                                                                                                          |
| `counters.xml`     | This files contains the list of items that can be counted by Razor. While the use of the counter in the titlebar has been deprecated for the in-game counter bar available in ClassicUO it's still required for the macro and script system to count items. |
| `guidelines.def`   | This file contains the guardline definitions that display on the UOPS map                                                                                                                                                                                   |
| `overrides.def`    | This file contains overrides for specific static values in Razor. See file for details.                                                                                                                                                                     |
| `Razor.exe.config` | This file is the base configuration for Razor and contains a few settings, such as more organizer or restock agents, that require the application to be started when those values are changed.                                                              |
| `spells.def`       | This file contains the list of spells in game and if they are considered Harmful or Beneficial.                                                                                                                                                             |

!!! warning
    Files not listed here the related binaries required for Razor to work. They include files ending with `.dll, .lib, .exe, .exp, .pdb` and **_should not_** be part of restoring Razor files between installations.

## Configuration

| File                   | Description                                                                 |
| ---------------------- | --------------------------------------------------------------------------- |
| `Razor.exe.config`     | This configuration is used if you are using Razor with the OSI client       |
| `ClassicUO.exe.config` | This configuration is used if you are using Razor with the ClassicUO client |

You must manually edit that file if you want to set any of these values.

```xml
  <appSettings>
    <add key="UODataDir" value="C:\Games\UO" />
    <add key="UOClient" value="C:\Games\UO\client.exe" />
    <add key="LastPort" value="2593" />
    <add key="LastProfile" value="default" />
    <add key="LastServer" value="login.server.com" />
    <add key="LastServerId" value="0" />
    <add key="ClientEncrypted" value="1" />
    <add key="ServerEncrypted" value="0" />
    <add key="ShowWelcome" value="1" />
    <add key="UId" value="21613fcd" />
    <add key="MaxOrganizerAgents" value="20" />
    <add key="MaxBuyAgents" value="10" />
    <add key="MaxRestockAgents" value="10" />
    <add key="ImportProfilesAndMacros" value="true" />
    <add key="BackupPath" value=".\Backup" />
  </appSettings>
```

### Agents

If you need more Organizer, Buy or Restock agents, modify these values or add them to the ClassicUO config.

```xml
<add key="MaxOrganizerAgents" value="20" />
<add key="MaxBuyAgents" value="10" />
<add key="MaxRestockAgents" value="10" />
```

# Overhead Messages

For details on how to use the Overhead Messages feature, view the [help guide](help/displaycounters.md#overhead-messages).

# In-Game Commands

Razor supports the following in-game commands (not case-sensitive).

!!! note "Command Prefix"
    The command prefix with the OSI client is `-`.

If you are using ClassicUO, you must prefix each command with `>`.

For example, instead of `-where` you would type `>where`. This is due to ClassicUO commands starting with `-` and several UO commands for yell, whisper, guild use other symbols so Razor had find something unique.

- `help` - Displays all the in-game commands you see here.
- `adduseonce` - Creates a target that adds the item to the **UseOnce** list.
- `time` - Displays the current client system time.
- `where` - Displays your X/Y/Z coordinates according to Razor.

!!! tip
    Some shards support `[where` to get your location from the server.

- `ping` - Pings the server 5 times and returns the results.
- `echo` - Send a message to yourself. Useful for leaving a note to yourself when macroing, for example.
- `getserial` - Get information on the RPV you are viewing to verify authenticity.
- `rpvinfo` - Get information on the RPV you are viewing to verify authenticity.
- `macro [name]` - This will run a macro in-game. If you have a macro called `CreateRobes` you would type `macro CreateRobes`.
- `hue` - Display information about an item, include the hue.
- `item` - Same as `hue`.
- `info` - Display a gump that shows info about the item or mobile selected.
- `resync` - This will resync your client, the same way the resync hotkey does.
- `mobile` - Display some general information about a mobile (used for debugging)
- `weather [weather_type] [num_of_effects]` - Set the weather in the game. Weather will end automatically after 6 minutes, on newer clients it will end in 10 minutes.
  - `[weather_type]`:
    - `0` - It starts to rain
    - `1` - A fierce storm approaches
    - `2` - It begins to snow
    - `3` - A storm is brewing
  - `[num_of_effects]` - Max number on the screen (client restriction) is 70.
- `season [season_type]` - Sets the season. This is client side only.
  - `[season_type]`:
    - `0` - Spring
    - `1` - Summer
    - `2` - Fall
    - `3` - Winter
    - `4` - Desolation
- `set [variable]` - Sets a macro variable\*
- `waypoint [x] [y]` or `track off` - Sets a waypoint at a specific `X/Y` location. If you pass `off` it will remove it from the client.
- `sysmsgs` - Show the current system message buffer
- `boat` - Show in-game boat control gump
- `hotkeys` - Show in-game gump with used hot keys

# Legacy Versions

These versions were developed before ClassicUO and will only work with the standard OSI client.

!!! warning
    These versions are no longer supported and while they should work just fine they should be considered legacy.

| Razor Version                                                       | Hash                                                                       |
| ------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| [Razor v1.5.0.16](https://www.razorce.com/Razor_UOR_CE-1.5.0.16.zip) | `SHA256: 63D0B617FCE217C067A3270323C9E58B63F379F401B4224E0EA937DAA7871B8B` |
| [Razor v1.5.0.15](https://www.razorce.com/Razor_UOR_CE-1.5.0.15.zip) | `SHA256: 64916F16A72FDE5E9C17B3A180001A896E9472458C8DB69E09DC7E37D78A6B30` |
| [Razor v1.5.0.14](https://www.razorce.com/Razor_UOR_CE-1.5.0.14.zip) | `SHA256: 1D352F7814311FDBFC3EF16DEAD6664562C85B3817D953112F687099A98D104A` |
| [Razor v1.5.0.13](https://www.razorce.com/Razor_UOR_CE-1.5.0.13.zip) | `SHA256: 090D753820B791E115532E96703DE1650B4DB0CE88191355D0F65A5799A51571` |
| [Razor v1.5.0.12](https://www.razorce.com/Razor_UOR_CE-1.5.0.12.zip) | `SHA256: 1AC1DAFBBDEE3DBEB1D031E63CAF904D43B60A05E98CB83ECA4872F892BD4F36` |
| [Razor v1.5.0.11](https://www.razorce.com/Razor_UOR_CE-1.5.0.11.zip) | `SHA256: 5945E5F9D5C87FEF552881C319167BD4ED012AE01D31FA855449B034129F7225` |
| [Razor v1.5.0.10](https://www.razorce.com/Razor_UOR_CE-1.5.0.10.zip) | `SHA256: 80FACEE8DB005E5CB7A89EFEBEE4BEE2DA242C0BF9AFA31B20ADEBEC44ED7FEF` |
| [Razor v1.5.0.9](https://www.razorce.com/Razor_UOR_CE-1.5.0.9.zip)   | `SHA256: 63158C8987BA0E7FBBA5917018595617830CF7B72A699A50A34F79A943365EE0` |
| [Razor v1.5.0.8](https://www.razorce.com/Razor_UOR_CE-1.5.0.8.zip)   | `SHA256: 0D25D01C85CFC8BA51D4FFEEBF59A3DC23B2400850A4B41C613DFC50AFAD5487` |
| [Razor v1.5.0.7](https://www.razorce.com/Razor_UOR_CE-1.5.0.7.zip)   | `SHA256: BD239C8F10FB80C3D1F6D185557679A0FCCF0CE35B2DB6D726B0DB0DB8BE7B7A` |

## Experimental

This version of Razor is identical to version `1.5.0.16` except with one major change to how Razor figures our your position to address the Razor "desync" issue.

Some users have reported issues when logging in so that is why this version is marked as experimental. If you experience issues, please revert to 1.5.0.16.

| Razor Version                                                       | Hash                                                                       |
| ------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| [Razor v1.5.0.17](https://www.razorce.com/Razor_UOR_CE-1.5.0.17.zip) | `SHA256: D26E8B887FC26B94FB5B0C50530BE07A4393783CCBEAA9C3FD5B38171A857571` |

## Validate Checksum

- Windows (using Powershell)

```powershell
Get-FileHash '.\Razor-1.x.x.x.zip' -Algorithm SHA25
```

- Mac

```bash
shasum -a 256 /path/to/Razor-1.x.x.x.zip' -Algorithm SHA256
```

- Linux

```bash
sha256sum /path/to/Razor-1.x.x.x.zip
```

## Configuration File

The **Welcome Screen** will control some of these settings, but some settings can only be changed by editing `.\Razor.exe.config`.

```xml
<appSettings>
    <add key="PatchEncy" value="1" /> <!-- Patch client encryption -->
    <add key="ServerEnc" value="0" /> <!-- Use OSI encryption -->
    <add key="MaxOrganizerAgents" value="20" /> <!-- Override the default Organizer agent count -->
    <add key="MaxBuyAgents" value="10" /> <!-- Override the default Buy agent count -->
    <add key="MaxRestockAgents" value="10" /> <!-- Override the default Restock agent count -->
    <add key="ImportProfilesAndMacros" value="false" />  <!-- If true, will import macros and profiles from %AppData%\Razor -->
    <add key="LastProfile" value="Test" /> <!-- Last profile used in Razor -->
    <add key="LastServer" value="test.server.com" /> <!-- Last server connected to in Razor -->
    <add key="LastPort" value="2597" /> <!-- Last server port connected to in Razor -->
    <add key="LastServerId" value="1" /> <!-- Index of the server selected in the dropdown -->
    <add key="ShowWelcome" value="1" /> <!-- Show the Welcome Screen when loading Razor -->
    <add key="UOClient" value="D:\Games\UO\client.exe" /> <!-- Location to Ultima Online client -->
    <add key="UODataDir" value="D:\Games\UO" /> <!-- Location to Ultima Online data directory -->
</appSettings>

<!-- You can define a list of servers here by adding values between <Servers></Servers> -->
<Servers>
    <add key="Custom Server" value="server.someserver.com,2593" />
</Servers>
```
