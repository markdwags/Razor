# UOAssist API (UOA API)

Razor supports the **UO Assist API** to allow basic integration into Razor. This information was pulled from [here](http://web.archive.org/web/20100419122932/http://www.tugsoft.com/uoassist/thirdpartydev.asp). It has been slightly modified for formatting purposes.

## Messages Received

Messages that Razor will process  (WM_USER = 0x0400 or 1024)

### Request Data  (WM_USER+200)

This is used to register your application with Razor.  Once your application is registered Razor will begin sending your application the messages described below.

* Parameters: WPARAM = The window handle where you want messages sent.  LPARAM = flags  ( 1 = get house notifications, NOTE all notification with a lesser notify value than the house notification will go to all windows that have requested information)
* Returns 2 on unhook, 1 on success, 0 on failure (failure means all slots full) .  
!!! note
    If you have registered a window and wish to unregister just send the message with the window handle again. 

### Count Resources (WM_USER+201)

This will prompt Razor to begin counting resources. 

* Parameters:  NONE
* Returns 1 on successful start or 0 on stop.

!!! note
    If resources are already being counted the count will be stopped.

    Currently it will always return 0 - this MAY be changed in the future but for now it will be zero

### Get Coords (WM_USER+202)

Gets the current location of the character.

* Parameters:  NONE
* Returns: X in the loword and Y in the hiword.  (x=LOWORD(retVal); y=HIWORD(retVal);)
* 
!!! note
    You MUST use SendMessage for the return value to be valid.

### Get Skill (WM_USER+203)

Gets the current skill level of specified skill.

* Parameters: WPARAM = skill number ( 0 based )  LPARAM (when 0 the return will be the 'display' value, when 1 it will be the 'base' value, 2 = get lock value, 3 = get skill name  ).
* Returns:  Skill level * 10.  Meaning if the skill level was 19.4 you would get 194 returned.  Or if lock value was requested the return is 0 = up, 1 = down, 2 = locked.  If the skill name is requested the value is a global atom that you will need to free.

!!! note
    You MUST use SendMessage for the return value to be valid.

### Get Stat (WM_USER+204)

Gets the stat and its related attribute.

* Parameters: WPARAM = stat to get   (str = 0, int = 1, dex = 2, weight = 3, 4 = Max HP, 5 = tithing points)
* Returns: Requested stat (str,int,dex, weight, max hp) in the loword and (hp,mana,stam, max weight, current hp) in hiword.   You MUST use SendMessage for the return value to be valid.

### Set Active Macro (WM_USER + 205)

!!! tip
    This is not enabled in Razor

Sets the active macro

* Parameters: WPARAM = macro (zero based  0-7)

### Play macro (WM_USER+206)

Plays/Stops active macro

* Parameters: None
* Returns: 0 - macro stopped  1 - macro playing

### Display text (WM_USER+207)

Displays text on the game screen

* Parameters: WPARAM loword = color  hiword = (0- over characters head 1- system message)
                    LPARAM = global atom that contains string - Razor frees the atom after using it (See Windows SDK on ATOMS)
* Returns 0 - failure either the user isn't logged in or the atom wasn't found,    1 - success

!!! tip
    Razor will only read strings 255 characters or less, I didn't actually test to see if uo can handle strings that long so if you plan to use long strings you should test that out first.

### Request house/boat information (WM_USER+208)

Requests the information on house(s)/boat(s) in the nearby area.

* Parameters: None
* Returns:  0 - failure (not logged into game) 1 - success

!!! tip
    You should make sure you are set to receive updates from Razor first.

### Add command line command (WM_USER+209)

Add a command line command to Razor.  When the user types the command (prefix is '-') you can receive a notification and the parameters the user enters using this feature.  For example if you add the command 'broadcast' and the user types in '-broadcast run away' your window will receive a message with 'run away' as the parameters (in a global ATOM).

* Parameters: WPARAM - HWND of the window to receive the message. LPARAM - ATOM of the command (NOTE: spaces won't work correct)
* Returns On failure  0 - All command space has been used,  1 - the command ATOM was not found (must be a global atom)
    - On success - returns the value of the message your window will receive (should be >= WM_USER + 400). The message you will receive will be like this:
        - MessageID = return value from add call (remember to use SendMessage)
        - WPARAM =  NULL - either a global atom couldn't be create or no parameters were entered
        - or a global ATOM that contains the parameters (You must delete the ATOM yourself)(See Windows SDK on ATOMS)

!!! tip
    Commands are not case sensitive.  If you add a command '/command' the user will have to type -/command in order for you to get the message.  To remove a command send the command again with a zero as the WPARAM.

### Get User ID (WM_USER+210)

You must use send message on this one.  It returns a ID that Razor can use to identify the user (NOTE: it is not the actual user ID used by the servers).  This is used to add a player to your party.

* Returns 0 if no user is logged in or a value if a user is logged into the game.

### Get Shard Name (WM_USER+211)

* Returns 0 if no user is logged into the game or if a global atom could not be created.
* Returns a global atom with the shard name in it. 

You must use SendMessage for this message.

### Add user to party (WM_USER+212)

* Parameters: WPARAM - User ID obtained from Get User ID message.
* Returns 0 on success, 1 on error (will not work if a macro is playing)  You must use send message if you want the return value.

### Get UO window handle (WM_USER + 213)

* Parameters: None
* Returns the HWND of the UO window.  NOTE - you must use SendMessage to get the correct value.

### Get poison status  (WM_USER + 214)

* Parameters: None
* Returns  1 if poisoned or 0 if not poisoned.  NOTE - you must use SendMessage to get the correct value.

### Set skill lock (WM_USER + 215)

* Parameters:  WPARAM = zero based skill number, LPARAM = lock direction  [0 = up, 1 = down, 2 = locked]
* Returns 0 on success, 1 on failure (will fail with invalid param(s) or not logged into game).  NOTE: when the lock value is set Razor requests the current skill values from the server.  So don't set locks in rapid succession for a long period of time or you may lag yourself out.

### Get account identifier (WM_USER + 216)

* Parameters:  None currently
* Returns 0 if no character is logged into game otherwise a identifier for the account

## Messages Sent

Messages Razor will send to apps that have registered a valid window.

### Resource Count Finished (WM_USER+301)

This message is sent when resource counting has finished.

* Parameters : WPARAM is defined below and LPARAM is the count
    - BloodMoss = 1
    - BlackPearl = 2
    - Garlic = 3
    - Ginseng = 4
    - Mandrake Root = 5
    - NightShade = 6
    - Sulfurous Ash = 7
    - Spider Silk = 8
    - User1 = 9
    - User2 = 10    
    - User3 = 11
    - User4 = 12
    - User5 = 13
    - User6 = 14
    - Batwing = 100
    - DaemonBlood = 101
    - GraveDust = 102
    - NoxCrystal = 103
    - PigIron = 104
    - Tithing = 200

### Spell was attempted  (WM_USER+302)

This message is sent when a spell is attempted.  It is not known if the spell will fail or timeout or be cancelled.

* Parameters:  WPARAM is the spell being cast (1 based)

### Logon (WM_USER+303)

Sent when a character logs into the game

* Parameters:  WPARAM is a character identifier (number value).

### Magery skill level (WM_USER+304)

Sent a few seconds after entering game and then whenever the skills window is opened or the skill level changes.

* Parameters: WPARAM is set to the whole number of the skill.  LPARAM is set to the tenth's.

### Current Int and Mana Level (WM_USER+305)

Sent whenever new information is sent from the server.

* Parameters: WPARAM has Intel and LPARAM has current mana level.

### Skill Level (WM_USER+306)

Sent a few seconds after entering the game and then whenever the skills window is opened or a skill level changes.

* Parameters: WPARAM is skillnum (0 based).  LPARAM is skill level * 10. Meaning if the skill is 19.4 you will get 194

### Macro Finished (WM_USER+307)

Send when a macro finishes playing through one pass.

### Logout (WM_USER+308)

Sent when a character logs out of the game.

### Current Max HP and HP Level (WM_USER+309)

Sent whenever new information is sent from the server.

* Parameters: WPARAM has Max HP and LPARAM has current hit point level. (NOTE this changed 8/2001 because of changes OSI made to how max hp are calculated [no longer str = max hp]).

### Current Dex and Stamina Level (WM_USER+310)

Sent whenever new information is sent from the server.

* Parameters: WPARAM has Dex and LPARAM has current stamina level.

### Add House/boat (WM_USER+311)  [You must set flag 1 to get this message]

Sent whenever a house comes into update range.

* Parameters:    WPARAM - X in the loword and Y in the hiword.  (x=LOWORD(wParam); y=HIWORD(wParam);)
                      LPARAM - house type

* Types:
    - 0x4000=small boat (n)
    - 0x4001=small boat (e)
    - 0x4002= small boat (s)
    - 0x4003=small boat (w)
    - 0x4004=small dragon boat (n)
    - 0x4005=small boat (e)
    - 0x4006=small boat (s)
    - 0x4007=small boat (w)
    - 0x4008=medium boat (n)
    - 0x4009=medium boat (e)
    - 0x400a=medium boat (s)
    - 0x400b=medium boat (w)
    - 0x400c=medium dragon boat (n)
    - 0x400d=medium boat (e)
    - 0x400e=medium boat (s)
    - 0x400f=medium boat (w)
    - 0x4010=large boat (n) 
    - 0x4011=large boat (e)
    - 0x4012=large boat (s)
    - 0x4013=large boat (w)
    - 0x4014=large dragon boat (n)
    - 0x4015=large boat (e)
    - 0x4016=large boat (s)
    - 0x4017=large boat (w)
    - 0x4064=small plaster
    - 0x4066=small field stone
    - 0x4068=small brick
    - 0x406a=small wood
    - 0x406c=small wood and plaster
    - 0x406e=small hut
    - 0x4070=blue tent
    - 0x4072=green tent
    - 0x4074=large house
    - 0x4076=two story (wood & plaster)
    - 0x4078=two story (plaster)
    - 0x407a=tower
    - 0x407c=keep
    - 0x407e=castle
    - 0x408c=generic shop
    - 0x4096=marble patio
    - 0x4098=small tower
    - 0x409a=log cabin
    - 0x409c=sandstone patio
    - 0x409e=villa
    - 0x40a0=stone shop
    - 0x40a2=marble shop
    - 0x5388=minax's fortress

### Delete House/Boat (WM_USER+312) [You must set flag 1 to get this message]

Sent when a boat moves or is dry docked or disappears (same with house)

* Parameters:    WPARAM - X in the loword and Y in the hiword.  (x=LOWORD(wParam); y=HIWORD(wParam);) LPARAM - house type

### Facet/land info (WM_USER+313) [You must set flag 1 to get this message]

Sent when the client receives information on which facet/world you are on.  This happens when you switch facets/worlds and can at other times as well

* Parameters:    WPARAM -  0 = Felucca  1 = Trammel  2 = Ilshenar

### Power hour entered (WM_USER+314)

!!! tip
    Not used by Razor

Sent when the client receives the 'You are in your power hour!' message.