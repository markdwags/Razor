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

| File | Descripton |
| ---- | -----------|
| animdata.csv | This file contains the list of animations used when filtering different mobiles in |
| counters.xml | This files contains the list of items that can be counted by Razor. While the use of the counter in the titlebar has been deprecated for the in-game counter bar available in ClassicUO it's still required for the macro and script system to count items. |
| guidelines.def | This file contains the guardline definitions that display on the UOPS map|
| Razor.exe.config | This file is the base configuration for Razor and contains a few settings, such as more organizer or restock agents, that require the application to be started when those values are changed.|
| spells.def | This file contains the list of spells in game and if they are considered Harmful or Beneficial. |

!!! warning
    Files not listed here the related binaries required for Razor to work. They include files ending with `.dll, .lib, .exe, .exp, .pdb` and **_should not_** be part of restoring Razor files between installations.

# Overhead Messages

For details on how to use the Overhead Messages feature, view the [help guide](help/displaycounters.md#overhead-messages).