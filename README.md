# Razor: Ultima Online Assistant

This is a fork of the Ultima Online Razor Assistant originally developed by Zippy and eventually made public on [GitHub](https://github.com/msturgill/razor).

The purpose of this fork is continue development on Razor for [UO Renaissance](http://www.uorenaissance.com/) focusing on "quality of life" improvements to Razor while keeping the spirit of Razor intact.

# Changes

Below is a running list of completed changes in no specific order.

## Razor

* UI changes that keep Razor familiar but make it feel not as "smashed together" - [DEMO](https://i.imgur.com/DFvj8b4.gif)
* Remove the auto-update feature check
* Remove PayPal button on Razor splash
* Popup windows for input should auto-focus and not require clicking in the text field first
* Convert much of the old .NET Hashtables and Arraylists to hopefully improve with performance
* Remove the use the DateTime.Now and use DateTime.UtcNow for the internal mechanism (DateTime.Now is 10 times slower than DateTime.UtcNow) (Read more here)
* Dark Theme Mode
* Output stats/skills/counters/etc "real time" to JSON file (needs expanding, base code is there)
* Add -hue command
* Removed any requirement on %AppData% and the registry. Razor is now portable.

## General
* Completely remove the SmartCPU feature
* Add Clone Profile button to easily create a copy of an existing profile instead of having to use the default
* Add Checkbox to enable confirmation buttons on buttons that clear lists or delete things - [DEMO](https://imgur.com/DLieW6f.gif)
* Delete confirmation when you deleting a profile

## Options
* Removed dependency on InstallDir being in HKLM which should result in more people able to use Force Screen size

## More Options
* Force Resize should work better for some people

## Macros
* Copy macro to clipboard - [DEMO](https://imgur.com/N7hsKSu.gif)
* New button to create macro from clipboard - [DEMO](https://imgur.com/gMW15Iu.gif)
* Control-UP / Down to move macro line - [DEMO](https://imgur.com/ANlYWbF.gif)
* Rename macro from UI - [DEMO](https://imgur.com/Bf7UZDT.gif)
* Double click macro action opens Edit dialog instead of having to right-click and Edit.. - [DEMO](https://imgur.com/OTkN17u.gif)
* Adding a macro to a group should not require typing in the group name each time - [DEMO](https://imgur.com/DLieW6f.gif)
* AbsoluteTarget values tried to profile to a particular profile

## Arm/Dress
* Add confirmation box on Clear List

## Agents
* Define the number of agents you want in the config file
* Add confirmation box on all clear buttons (so tired of accidently clicking clear instead of clear hotbag) - [DEMO](https://imgur.com/uGpBVvn.gif)

## Hotkeys
* Filter by name to quickly find a hotkey - [DEMO](https://i.imgur.com/G5ZY0ZZ.gif)
* Add skill countdown (like criminal or bandaid counters)

A running list of changes and discussion can be found here:

http://uorforum.com/threads/improving-razor-razor-development.33134/
