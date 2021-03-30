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
| `overrides.def`    | This file contains overrides for specific static values in Razor. See file for details.|
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
    - `4` - Desolation (Feluccia)
- `set [variable]` - Sets a macro variable\*
- `waypoint [x] [y]` or `track off` - Sets a waypoint at a specific `X/Y` location. If you pass `off` it will remove it from the client.
- `sysmsgs` - Show the current system message buffer
- `boat` - Show in-game boat control gump
- `hotkeys` - Show in-game gump with used hot keys
