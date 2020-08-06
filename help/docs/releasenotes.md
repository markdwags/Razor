# Razor Release Notes

Below is a list of the latest releases notes going back to the initial version

## 1.6.5.X (08/X/2020)

* Fixed issue with saving scripts in certain scenarios
* `targettype` will find mobiles and items by name
* Moved Friends tab to the main tab list
* Added stop command to scripts
* Removed unused dependency
* Set friend tag and hue per group
* Attack name overhead "new targets only" works on hotkeys
* Changed default encoding on profiles to UTF8 (cross platform support)
* Added the ability to set waypoints in-game
* Create a waypoint to the position of your corpse upon death
* Added '`backpack`' option to `dclicktype` and `targettype`
* Moved Filters to the main tab list
* Added Next/Prev Non-Friendly Targets
* Added `clearsysmsg`/`clearjournal` to scripts and macros

## 1.6.4.32 (05/17/2020)

* Fixed any errors related to systray and long titles
* Razor supports Linux again after fixing issues related to the new script editor
* Fixed issues with `dclicktype` and `targettype` range check (both Macros and Scripts)
* Fixed a few packet exception issues when logging in or casting spells
* Script engine optimizations (better error reporting, better timeout/pause handling)
* Fixed issues related to `waitfortarget`/`wft` not working correctly
* Fixed issues with nested if/else/endif
* Fixed the '`lasttarget`' variable
* Added '`hidden`' expression
* Fixed '`findtype`' expression
* Added range check to '`findtype`'
* Fixed righthand, lefthand and hand variables/aliases
* Field to filter scripts in the UI
* Added hotkey to add party members (`/add`)
* Added feature to convert macros to scripts (right-click macro and convert)
* Backup will include Razor Scripts
* Added support for Mystic and Masteries
* Show Attack message when using Attack Last Target hotkey

## 1.6.4.2 & v1.6.3.43 (03/16/2020)

This version introduces a new scripting engine that makes it easier to create and share macros.

For more information, visit the [Razor Scripting Engine Guide](http://www.uor-razor.com/guide/).

!!! note
    This is a hotfix for anyone having a CultureNotFoundException error on Windows 7 which 1.6.4.2 fixed.

* Introduction of the Razor Scripting Engine (thanks to everyone for your time testing and providing feedback)
* New Sound & Music Tab: Filter out any game sound, trigger music and listen to any sound
* Added while loop to the macro system
* Fixed issue with auto-screenshot not working with players with invalid characters in their name
* Enforce your min light level on login
* You can extract specific words from the system message to display in the overhead messages (…{1}. {2}, etc for the first, second word)
* Added Set Macro Variable Target Action to macros that will pause until you set a variable (same behavior as "Set Last Target")
* Razor's Last Object (hotkey and macro/scripts) should work as expected
* `>hue` and `>item` commands are a bit more informative
* You can copy/paste friends list when you aren't actually logged in
* Added option to have next/prev friend highlight friend in Friends tab
* The Discord overlay shouldn't cause any access violations
* SysTray will go away on close (instead of leaving ghost ones behind)

## 1.6.2.23 (12/18/19)

* Changed interrupt order to start shirt, shoes, pants
* Fixed Edit button on Overhead Message UI
* Capture compressed gumps for WaitForGump
* Additional UI SafeActions
* Added ability to filter out mobiles from being targeted
* Added `if skill` to macros
* Fixed invalid cast error on `if counter`
* Fixes a bug were when changing a character in same account, using different profiles (not default), causes a sudden lockdown of razor window, that also locks ClassicUO from * loading, causing a crash after a while.
* Double clicking Overhead Message macro action now opens Input box for editing message instead of Pause action menu.
* Bandage timer will restart if bandage is applied before it ends
* Using 'Add Target' under the friends tab will give you a target
* Renaming a macro removes the old entry from the hotkey list
* Added Set HotKey button to macro UI
* Added `>set` command to set macro variables in-game
* Prioritize key that triggered event, include number row in SDL keymap
* For ClassicUO, dont trigger Hotkey.OnKeyDown if key is a modifier key
* UOPS displays Trammel and TerMur correctly
* Fixed issue with GumpReponses with text entries not loading
* Bandage timer will stop on "You did not stay close enough to heal your target.
* Bandage timer will reset if you bandage yourself before finishing
* Remove packet block on 0xF0 when using ClassicUO to allow party tracking in WorldMap
* Added mobile filter for daemons

## 1.6.1.32 (08/05/19)

* Next/Previous will only set Beneficial when using Next/Prev Friend
* Changed Razor commands from - to > (ie: -hue is >hue)
* Next/Previous Target will check if the target is a friend
* Next/Previous Friendly can be set to only set Beneficial
* Next/Previous Non-Friendly can be set to only set Harmful
* Fix to how names are handled per mobile
* Pet & Boat commands now use the speech override hue
* Enabled the seasons dropdown when using ClassicUO
* Fixed ContextMenu not recording & execution
* AbsoluteTargetVariables and DoubleClickVariables have been merged into MacroVariables.
* When adding an Overhead Message, if it doesn't exist in the list you will be prompted to add it anyway
* Fixed issue with Friends and Dress lists not always loading when using ClassicUO
* Improved the bandage timer to pick up additional start/stop messages
* Set Last Beneficial & Set Last Harmful now have the proper system message
* Friend hotkeys now display under a new "Friends" hotkey category
* Buff/Debuff overhead messages can be filtered and hues changed
* Added check to prevent multiple single-click flag checks
* Additional SafeActions added to the UI
* Added {name} as an option for the target indicator
* Added the ability to edit the bandage start/stop messages
* Fixed issue using the delete key when removing Friends
* Updated the screenshot capture on death (option to add a delay, and to capture self death or death of others)

## 1.6.0.57 (06/28/19)

After 57 builds, Razor now fully supports ClassicUO. Thanks to KaRaShO for his amazing work on this client. Thanks to Jaedan, DarkLotus, and everyone who helped test Razor and provided feedback.

* Enabled support for ClassicUO and standard OSI client
* Enabled cross-platform support (specifically Linux)
* Several optimzations, tweaks, and overall improvements to Razor.
* Queue Last/Self default to disabled
* Auto-queue object delay default to disabled
* Clean Config.cs formatting
* Merged UltimaSDK
* Add Grab HotKey & Set Grab Hotbag
* New Next/Prev Hotkeys
* Added Criminal, Murderer and Innocent Next/Prev hotkeys
* Add Next/Previous for Friendly, Gray and Non-Friendly
* Refactored the Next/Prev code
* Added target sub-categories in HotKeys
* Restored {mana} and {stam}
* Added Toggle War and Toggle Peace only hotkeys
* Added < and > to if macro actions
* Added {maxhp}, {maxstam} and {maxmana} as variable options in the IfActions in macros
* Added a Greater Heal/Cure hotkey, relabeled Heal/Cure Hotkey to "Smart Heal/Cure"
* Support SmartLastTarget in AttackLastTarget
* Added Set Beneficial and Harmful hotkeys
* Added option to disable auto-open door when hidden
* Added use item for Right or Left hotkeys
* Added option to disable play/finished macro message
* Next/Prev will ignore ghosts and blessed mobiles (just like random and closest)
* Added ability to insert overhead messages in macros
* Hotkey enable/disable messages will use warning or default Razor message hues. This should make it easier to see/read when you're hitting the hotkeys.
* Next/Prev friend won't set them as a combatant
* Added Insert Wait For Target for macros
* Added option under Display/Counters to display bandage timer overhead or in system messages
* Removed Friends Agent and added new Friends Groups
* Create groups of friends
* Enable/disable groups
* Copy/paste friends in and out of Razor
* Implemented filtering of repeating messages for system, Razor and overhead messages
  
## 1.5.0.17 (03/17/19)

This version completely changes how Razor handles movement. Prior to this release, Razor would parse movement packets to determine your current X/Y/Z. During certain in-game actions, such as running when casting, this would sometimes cause you to desync. This is when the server thinks you're at a certain X/Y/Z and Razor thinks you're on another.

Razor will instead start a timer in the client that checks if the position has changed in client memory. When your position changes, it will send a message to notify Razor.

This way, Razor is no longer in the business of reading movement packets which should prevent you from ever desyncing.

### Razor

* Razor should no longer "desync" (usually experienced by people with lower pings).
* Crypt.dll has been cleaned up and optimized. You should notice Razor and UO start up quicker.
* If Razor has an issue verifying an animation for the filter dropdown, it will just load it by default.

### Macros

* The "Waiting.. Timeout" message when you're running a macro is easier to read.

### HotKeys

* If you use the "Add All Mobiles To Friends" it won't add the same person twice.

## 1.5.0.16 (03/04/19)

### Razor

* Changed the position of the Quit button on the Welcome screen.
* Removed packet and AVI recording since the feature has been sunset by modern applications and technology. If you need to watch or record packet videos, please use and older version. If you want to record videos, check out OBS.

### Options->Speech & Messages

* Added an Edit button to the Overhead message window to edit a message.
* Added an option to show [Friend] overhead if that mobile is on your friends list.

### Options->Targeting & Queues

* Fixed issue with Next/Prev Humanoid still targeting friends even when the checkbox was checked.

### Options->Additional Options

* Combined "Always show stealth steps" and "Count stealth steps" into one checkbox. The Now counting stealth steps. message has been removed since it didn't have any logic to only fire off when stealth was successful. Utilize the Overhead message feature if you want to see something overhead on a successful stealth.
* You can now customize the step message overhead.
* The Static Wall and label checkbox will now load/save properly.
  
### Skills

* Added an option to show skill/stat gains overhead.

## 1.5.0.15 (02/17/19)

### Razor

* Revamped the main Razor UI: As the feature set has grown, space has become limited. This initially led me down the road of expanding the window based on the top, but this started to prove to not be a good long term solution. I tried to keep the look and feel as close to Razor as possible.
* Removed window adjusting size based on the tab. It's now a static size.
* The "Options" tab now has sub-tabs with features broken into smaller categories.
* The "More Options" tab has been removed and condensed into "Options"
* The "Display/Counters" tab has been broken into two sub-tabs.
* The "Macros" tab has been broken into two sub-tabs, instead of that little button to expand the window.
* Numerous other slight adjustments.
* Revamp of the Welcome UI: Just to go along with the main UI revamp.
* More easily manage servers from the welcome UI
* Warning if your UO Client Directory and UO Data Directory don't match (often a source of issues for people)

### Macros

* Razor now will record and play prompt actions (ie: renaming runes, renaming runebooks, etc).
* Re-targeting a mobile on the DoubleClick action (ie: a horse) will work instead of having to re-record the double-click action.

### HotKeys

* Added a hotkey to add all the mobiles in range to your friends list.
* Added hotkeys for all pet commands (all follow, all kill, etc). This does the exact same thing as a standard macro recorded saying the command.
* The following targeting hotkeys exist in another version of Razor (1.0.14.10) that was updated in 2017. They've been added in this version.
    * Closest Criminal Monster
    * Closest Innocent Monster
    * Closest Murderer Humanoid
    * Closest Murderer Monster
    * Random Criminal Monster
    * Random Innocent Monster
    * Random Murderer Humanoid
    * Random Murderer Monster
    * Next Monster Target
    * Previous Monster Target
* The following targeting hotkeys are new to this version of Razor and did not exist in previous versions.
    * Closest Friendly Humanoid
    * Closest Friendly Monster
    * Closest Non-Friendly Humanoid
    * Closest Non-Friendly Monster
    * Random Friendly Humanoid
    * Random Friendly Monster
    * Random Non-Friendly Humanoid
    * Random Non-Friendly Monster
    * Closest Mobile
    * Random Mobile
  
### General

* When you change your profile, the animation filter list will properly reload.
  
### Options->Targeting & Queues

* Added option to change the text of the target indictor.
* Added option to prevent Next/Prev target hotkeys from targeting friends.

## 1.5.0.14 (01/16/19)

### Display/Counters

* Added a damage tracker feature that can be enabled/disabled via a hotkey that will track the following:
    * DPS (Damage Per Second)
    * Final DPS
    * Max DPS
    * Total Damage Dealt
    * Total Damage Taken
    * Max Single Damage Dealt (and to which mobile)
    * Max Single Damage Taken
    * Top 5 Damage Dealt By Name (ie: if you kill dragons for an hour, it will give you the total damage you dished out to all those dragons)
* When you stop the damage tracker, the stats above will be displayed in your client and journal.
* Updated the UO Title Bar to accept the following variables to display the tracked values above: {dps}, {maxdps}, {maxdamagedealt}, {maxdamagetaken}, {totaldamagedealt}, {totaldamagetaken}
* You can change the text in Razor's title bar just as you can with the UO title bar.


### General

* Added the ability to toggle Dragons and Drakes to use a different graphic. This will have the same effect as those who use the "artmods" to replace the large dragon graphic will a small dragon or some other graphic completely (Blackthrone is a popular one). In other words, only you will see the graphic change.
* Minor UI tweaks you may or may not notice.

### Hot Keys

* Added a hotkey that will only enable hotkeys (not toggle on/off).
* Added a hotkey that will only disable hotkeys (not toggle on/off).

### Maps

* Added UOAssist API support back into Crypt so Ultima Mapper and other applications that use that will work with this version of Razor.
* Added buttons to quickly open UOAM or Ultima Mapper from the Maps tab.

### Macros

* Added the ability to filter macros for easy searching.

### More Options

* You can now set individual hue colors for overhead messages.
* Damage indicators in UO don't show up in your journal. Added an option to have damage dealt and damage taken display overhead or in the lower-left which will have it log to your journal.

## 1.5.0.13 (01/01/19)

### More Options

* Added option that will turn magic fields/walls into static tiles.
* Updated the attack name overhead feature to include the names of targets acquired via hotkeys.
* Added option to display a text target indicator over a newly acquired target.
* You can no longer open up multiple container label and overhead message windows.

### Macros

* Along with AbsoluteTarget variables, you can now create DoubleClick variables. NOTE: The goal of these macro variables is allow macros to be shared across multiple characters without requiring cloned copies of the macros.
* You can retarget to Lift and LiftByType actions (instead of having to record, re-lift new item, stop recording, delete old action)
* Pausing a macro when one isn't running will no longer cause a crash.
* Copy/pasted macro lines won't appear "linked" and will work across other macros.

### Maps

* The boat control will now let you set relative location. Right-click on the direction you're going to set the relative location.

## 1.5.0.12 (12/25/18)

### Razor

* Fixed the issue that was causing the "filter exceptions" popup
* The -ping command will display the correct value for the average
* Entering the wrong -command will no longer result in an error popup
* The previous version reverted Razor from using the updated visual style back to the older looking style due to a Windows patch last month (KB4459942). This has been fixed.

### Macros

* Fixed the issue with Absolute Target Variables not saving correctly in a macro

### Agents

* Like the Organizer Agent, you can no longer enter mulitple items into a Restock Agent
* Double-clicking on the list when viewing the Restock or Buy agents will popup the dialog to change the value (same behavior as clicking Edit)

## 1.5.0.11 (11/20/18)

### Razor

* Razor will check the value of UODataDir in your config before looking to the registry, making it now fully portable and able to handle computers with multiple versions of UO installed. This should fix missing images in the titlebar on counters for some people.
* Razor will try to be a little smarter when determining if it's offscreen and reset itself if it is.

### More Options

* If your container has a non-generic name (ie: holiday bags, special chests), you can set an alias so the container label will work on it.
* Added an option to automatically block trades
* Added an option to auto-accept party invites from friends
* You can set an min/max light level. Be sure you have your Light Filter unchecked before using this feature.

### Macros

* Double-click GumpResponse to check button ID
* Added Use Last Gump Response to macro options (ie: to update a macro using a runebook)

### Hot Keys

* Added a hotkey to interrupt spell casting (same effect as having a macro that picked up and equipped a pair of shoes).
* You can now capture data from large and small BOD deeds. The file will be saved in a .csv file (BODs.csv) in the root path of Razor.
  
## 1.5.0.10 (11/2/18)

### Razor

* Razor won't display extra systray icons
* Manually typing in a server name will save it correctly to the config
* Razor will correctly handle different sized maps
* Razor will import your macros, profiles and counter.xml from %AppData%\Razor when you run it the first time and the ImportProfilesAndMacros is set to true in the config.
* Razor will ignore if Window's font is zoomed (Windows 7 is 125% by default on some machines)
* -ping command will give a litle more detail

### Options

* You can now set the season in Razor that will display in the client. This will only change what you see.

### More Options

You can now assign container labels to containers (chests, bags, etc) in UO. This label can be colored coded and set to help identify what is in the container.
Overhead messages can be changed from Unicode to ASCII (more classic look)

### Macros

* Absolute Targets will now save in the profile instead of a separate file.
* Double-Clicking "If", Comments or "For Loops" should open the correct dialog
* Added ability to edit speech in macros
* Potential fix to WaitForGump action not working
* After you add a macro via the clipboard, it will desolate your clipboard.

### Agents

* Create a new agent IgnoreList, this allows you to add people to the list and all messages (regular, yell, whisper, emotes), except for spells, will be blocked from displaying in UO. This includes being blocked from going into Razor's internal SysMessage queue to be used for macros.

### Hot Keys

* Add UseOnce Container should no longer crash Razor
* Added Previous Target hotkeys to match the Next Target hotkeys
* Organizer and Restock hotkeys sorting has been changed to be more clear (this will require mapping those hotkeys)
* Instead of trying to figure out the monsters using mobtypes.txt, I changed the logic of targeting monsters to target anything that isn't human. This should prevent scenarios where it wouldn't target a monster using an animal model.

### Advanced

* The backup profile/macros button should now work if a certain folder is missing.

### Display/Counters

* {skill} timer will start when using skills directly in the client

## 1.5.0.9 (9/26/18)

### Map

* Captured MIBs display on screen
* Boat uses your Razor speech text
* Fixed Stop from adding slow or one
* Fixed slow left from not responding (stubborn tillerman)

### Display/Counters

* Added {buffsdebuffs} to display bar with timer countdown
* Fixed {followers} and {maxfollowers}
* Gold tracker now shows total minutes {goldtotalmin}
* Gold tracker will start when you pick up your first gold pile if enabled
* Added {uptime} to show how long you've been connected

### Options

* Added option to block auto-opening corpses twice in a session

### More Options

* Buffs and debuffs can now be displayed over your head

### Hotkeys

* Added Scavenger Enable/Disable, Scavenger Set Hot Bag and Scavenger Add Target
* Added HotKey to trigger Gold tracker on or off

### Advanced

* Fixed Advanced tab to show which features are enabled

### Razor

* Default Razor config will be taskbar instead of systray

### Macros

* Added option to do a range check on TargetByType
* Added option to do a range check on DoubleClickTargetType
* Fixed the WalkAction macro to no longer cause massive desync issues

### Screenshots

* You can now upload screenshots to Imgur from Razor for easy sharing. All images are uploaded anonymously and are not linked to any Imgur account.
Razor will keep a history of images uploaded, and store the delete hash so they can be deleted.

## 1.5.0.8 (8/29/18)

### Razor

* I had a talk with the tillerman and he agreed to respond to your boat commands.

### Map

* Party members who are out of range will continue to update their position on your map.
* When using "Smart Always On Top", JMap will react like Razor and UOPS do when you unfocus the current UO client.
* Added HotKey that will toggle JMap visible/hidden.
* Fixed issue with MIB coords sometimes being negative.
* Updating the X/Y changes now update markers correctly.
* Marker X/Y accept proper inputs.
* New MIB icons for mib x/y coords.
* Fixed rendering offset with some markers
* Mouse coords should now display correctly.
* TMap and MIB Markers should now highlight their corresponding tile when hovered.

### Options

* Object delay has a checkbox. Unchecking it will set your delay to 0. You can still leave it enabled and enter 0.

## 1.5.0.7 (8/24/18)

This version introduces a brand new UO mapping system referred to as JMap (designed by Jimmy the Hand), that will hopefully give those who use Razor's current map system system more functionality while giving those who use UOAM an option built directly into Razor. JMap is still in active development and this is the first public release. Please report any issues on the development thread.

!!! warning
    This is a legacy feature that was removed at the request of the original developer

This version also includes a new Overhead system that lets you trigger messages that are in the lower-left to appear overhead.

### Razor

* Updated the main UI to use the modern Windows theme if it's enabled. Looks much nicer now.
* Sound filters for bulls, dragons and chickens.
* Updated UI size and text size for all input forms.
* Moved packet logging location to Razor directory
* Added Save button to Profile (previously you could only save your profile by exiting out of UO completely)
* Additional updates to Razor to make it fully portable. If the registry keys for UO are missing, it will default and use the UODataDir defined in the config. You should be able to copy/paste your UO folder and Razor to any location now.

### Map

* Enabled JMap
* Added stop and a close button to the Boat Control Window
* Added MIB capture feature. When you open an MIB, it will write the X/Y coordinates to a file that can display in JMap.

### Hot Keys

* Added "Set Sell HotBag" hotkey
* Added "Add UseContainer" hotkey
  
### Display/Counters

* Added gold per second, minute and hour to titlebar. This can be enabled per session and tracked via adding {gps}, {gpm}, {gph} and {totalgold} to your display bar
* Fixed skill timer ({skill} in the displaybar) to be more accurate
  
### More Options

* Added Overhead Message UI
* Updated Force Screen Size checkbox to tell you to open options and click Apply instead of logging out and back in
* Added option to put stealth count overhead
  
## 1.5.0.6 (7/18/18)

* Added light level bar to change in game
* Unchecking the light filter will reset it
* Added boat control UI and functionality (similar to UO Rudder)
* Added initial Map tab for new map update, most features are not enabled
* Added a hotkey to only show mobiles
* Added a hotkey to only show corpses
* Target Closest Gray Monster will include dire wolves and hellhounds.
* Option to disable put self/last/clear target messages from being overhead
* Added option to display stun messages overhead
* Updated bundled counter.xml to include more useful items
* Added feature to log your skill gains to a CSV file per character
* Sound filters for bulls and dragons
* Removed some responses from the -mobile command that didn't show data
  
## 1.5.0.5 (5/18/18)
* Updated the -mobile command to have some additional information
* Add IsSeaMonster to each Mobile object
* Fixed the Target Closest Gray Monster and Random Gray Monster to include Sea Monsters (sea serpents, etc)
  
## 1.5.0.4

* Fixed issue with macros not opening up in the editor if they were in categories.
* Added ability to step through a macro when playing (ie. for debugging)
* Stepping through a macro now works from "Play From Here"
* Added a button to disable SmartCPU for testing purposes
* Added Hotkey to Pause current macro
* Added the -item command to do the same as -hue
  
## 1.5.0.3

* "Fixed" issue with missing Z axis on player. Compiling Crypt.dll with Optimizations would break it. Need more research but a current work around.
* Additional check on disabling SmartCPU
* Additional generic to non-generic array changes
* Auto-open doors is default true in new profiles
* Added by type protection based on @uoitalia's code
* Added Target Next Enemy Humanoid
* Added a checkbox to force different TargetByType targets. In other words, don't target the same type twice.
* Added -clearitems and -resync commands. -clearitems is experimental to see if it addresses issues where items out of range are picked up
* Added Cyclops and Titans sound filter
* Deer Sound filter works correctly
* Fixed issue with the Hotkeys filter no resetting when you left the tab and came back
* You can open a macro externally to your default editor

## 1.5.0.2
* Added Target Random Grey Monster
* Added Target Random Enemy Monster
* Added Target Closest Grey Monster
* Added Target Closest Enemy Monster
  
## 1.5.0.1
* Fixed issue with organizer agents not saving correctly (and thus not loading correctly)
* Fixed issue with not being able to delete an absolute target when you had 1 left
* Razor will remember the last server you picked
* Additional validation on the OK button to be sure you selected a data dir and UO client
* Added a few issues with getting PacketFilter errors on values that didn't exist.
  
## 1.5.0.0

### Razor

* Remove the auto-update feature check
* Remove PayPal button on Razor splash
* Popup windows for input should auto-focus and not require clicking in the text field first
* Convert much of the old .NET Hashtables and Arraylists to hopefully improve with performance
* Remove the use the DateTime.Now and use DateTime.UtcNow for the internal mechanism (DateTime.Now is 10 times slower than DateTime.UtcNow)
* Added -hue command
* Removed dependancy on InstallDir being in HKLM which should result in more people able to use Force Screen size. GetInstallDirectory now returns the folder the Razor.exe is running in.
* Removed all dependancy on the HMCU for the registry. All settings are in App.config and all Profiles and Macros are expected to be the sub folder of Razor.exe
* Additional UI adjustments

### General

* Completely remove the SmartCPU feature
* Add Clone Profile button to easily create a copy of an existing profile instead of having to use the default
* Add Checkbox to enable confirmation buttons on buttons that clear lists or delete things
* Delete confirmation when you deleting a profile
  
### Options

* Add {circle} to the Spell Format option to display, if you want, the circle the spell was casted from.

### Macros

* Copy macro to clipboard
* New button to create macro from clipboard
* Control-UP / Down to move macro line
* Rename macro from UI
* Double click macro action opens Edit dialog instead of having to right-click and Edit..
* Adding a macro to a group should not require typing in the group name each time
  
### Arms/Dress

* Add confirmation box on Clear List
  
### Agents

* Add confirmation box on all clear buttons (so tired of accidently clicking clear instead of clear hotbag)

### Hotkeys

* Filter by name to quickly find a hotkey