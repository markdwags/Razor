# Help / Guides

If you're unable to find a solution using the guides here or you'd like to submit a feature request or bug report, you can do that several ways:

* [Razor Development Thread](http://uorforum.com/threads/improving-razor-razor-development.33134)
* [Razor Bug/Issue/Release Tracking Thread](http://uorforum.com/threads/improving-razor-razor-development.33134)

You can also join the official [UO:Renaissance](http://www.uorenaissance.com) IRC server and ask your questions there.

* If you have an IRC client, connect to `irc.chat4all.org` and `/join #Renaissance`
* To connect directly via your browser, [click here](http://www.uorenaissance.com/irc/?page=irc).

And lastly, if Discord is your thing, you can [join on Discord](https://discord.gg/ydgfHzK) for support.

## Client Lockups/Flashing

Some users have reported the UO client will lock up and/or "flash" when using Razor. If you experience this, please do the following (these steps are outlined in the [Installing Razor](index.md) section):

* Close out of **all** running instances of Ultima Online and Razor.
* Browse to your Ultima Online installation directory.
* Find client.exe, right-click and go to Properties.
* Under the **Compatibility tab**, check **"Run this program in compatibility mode for"** and select** "Windows XP (Service Pack 2)"**
* Under the same **Compatibility tab**, check **"Run this program as an administrator"**
* Relaunch Ultima Online using Razor and confirm issue is resolved.

## Razor UI is cutoff

If the Razor UI appears to be cutoff, you most likely have your default text size set to 125% instead of 100%.

***NOTE:*** This has been fixed since version **1.5.0.10**

### Windows 7

* Right click on a empty area of your desktop and choose **Personalize**.
* Click on the Display link at the bottom left corner.
* Select **Smaller - 100%**
* Click on the Apply button.

### Windows 10

* Right click on a empty area of your desktop and choose **Display Settings**.
* Click **Display** on the left
* Scroll down to **Scale and layout** and select **100% (Recommended)**

## Forcing Window/Screen Size

By default, the maximum window size with the classic Ultima Online client is `800x600`. Razor allows you to expand the window to make it larger. This version has addressed the issue that would prevent some people from being able to take advantage of this feature.

***NOTE: ***You aren't increasing the resolution of the game, you are setting a size for the window the client will render. If you make this size too large, you will get screen tearing and block boxes.

* Check Force Game Size on the More Options tab and enter your desired X and Y sizes.
    * Personally, I find 1000x800 is a good balance between black corners and size. 1180x990 is the largest without screen tearing.
* Open you Paperdoll (ALT-P) and click Options. Then click Apply.
* If your screen doesn't adjust, you can try to log out and back in.

## UOAM Location Tracking

If the location is wrong in UOAM (or not tracking at all), follow these steps:

* Go buy a sextant in game and use it
* In UOAM, click UO->Calibrate
* Enter your map, longitude and latitude and click OK

## Removing Gray Corners

While not a feature of Razor, I've been asked a few times about the gray corners. You can remove the naively in the UO client by doing the following:

* Log into your character and open your paperdoll
* Click **Options**
* Click the lower-left tab (Macro Options)
* Click **Add** and under Actions, select **"ToggleRangeColor"** and set it to some key combo you will never use since this is a one-time thing (ie. CTRL-SHIFT-P)
* Click Okay and hit the key combination you setup. The gray corners should disappear.

> ***Note:*** Black corners and tearing based on your forced resolution will still exist.