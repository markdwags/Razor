# Commands Overview

The commands issued in the scripting engine are similar to a commands you might enter into a command prompt or shell.  Each line has a starting command, and a set of parameters.  Some of those parameters are required, some are optional.

`command (required) [optional]`

All parameters are shown inside of parenthesis or brackets.  Parameters within parenthesis are **required** while those in brackets are _optional_ and will default to specific value if not provided.

If you want to pass several words as a single parameter you must wrap them using `'` or `"`. For example `"hello goodbye"` is one parameter, while `hello` `goodbye` is two.

For example, if your script is something like:

```csharp
// Say 'Hello'
say Hello friends!
```

You will end up just saying `Hello`.  Instead, you if you wrap the words in single (') or double quotes (") the engine will see it as a single parameter.

```csharp
// Say 'Hello friends!'
say 'Hello friends!'
```

Here the list of the support commands:

## attack

Syntax: `attack (serial)` or `attack ('variablename')`

Description: Attack a specific serial or variable tied to a serial.

Example(s):

```csharp
attack 0x2AB4

attack 'attackdummy'
```

## cast

Syntax: `cast ('name of spell')`

Description: Cast a spell by name

Example(s):

```csharp
cast 'blade spirits'
```

## clearsysmsg

Syntax: `clearsysmsg`

Description: Clears the internal system message queue ()

Example(s):

```csharp

sysmsg 'hello'
sysmsg 'bye'

// You say hello!
if insysmsg 'hello'
   say 'hello!'
endif

clearsysmsg

// You say nothing
if insysmsg 'bye'
   say 'cya!'
endif

```

## dclick

Syntax: `dclick (serial)` or `useobject (serial)`

Description: This command will use (double-click) a specific item or mobile.

Example:

```csharp
dclick 0x34AB
```

## dclicktype

Syntax: `dclicktype ('name of item') OR (graphicID) [inrange]` or `usetype ('name of item') OR (graphicID) [inrange]` or `usetype ('name of item') OR (graphicID) backpack`

Description: This command will use (double-click) an item type either provided by the name or the graphic ID.

!!! tip
    If you include the optional `true` parameter, items within range (2 tiles) will only be considered. If you include the optional `backpack` parameter, items in your backpack only be considered.

Example:

```csharp
dclicktype 'dagger'
waitfortarget
targettype 'robe'
```

```csharp
// Same as above, but only items in range
dclicktype 'dagger' true
waitfortarget
targettype 'robe' true
```

```csharp
// Same as above, but only items in your pack
dclicktype 'dagger' backpack
waitfortarget
targettype 'robe' backpack
```

## dress

Syntax: `dress ('name of dress list')`

Description: This command will execute a spec dress list you have defined in Razor.

Example(s):

```csharp
dress 'My Sunday Best'
```

## drop

Syntax: `drop (serial) (x/y/z/layername)`

Description: This command will drop the item you are holding either at your feet, on a specific layer or at a specific X/Y/Z location.

Example(s):

```csharp
// Lift a specific item and drop on chest/torso
lift 0x400D54A7 1
drop 0x6311 InnerTorso
```

```csharp
// Lift a specific item, wait 5 seconds, drop it on a specific location
lift 0x400D54A7 1
wait 5000
drop 0xFFFFFFFF 5926 1148 0
```

## droprelloc

Syntax: `droprelloc (x) (y)`

Description: This command will drop the item you're holding to a location relative to your position.

Example:

```csharp
lift 0x400EED2A 1
wait 1000
droprelloc 1 1
```

## emote

Syntax: `emote ('message to send') [hue]`

Description: This command will force your character to emote the message passed as the parameter.

Example:

```bash
emote 'Hello world!'

emote 'Hello world!' 454
```

## gumpresponse

Syntax: `gumpresponse (buttonID)`

Description: Responds to a specific gump button

Example:

```csharp
gumpresponse 4
```

## gumpclose

Syntax: `gumpclose`

Description: This command will close the last gump that opened.

Example:

```csharp
gumpclose
```

## hotkey

Syntax: `hotkey ('name of hotkey')`

Description: This command will execute any Razor hotkey by name.

Example:

```csharp
skill 'detect hidden'
waitfortarget
hotkey 'target self'
```

## lasttarget

Syntax: `lasttarget`

Description: This command will target your last target set in Razor.

Example:

```csharp
cast 'magic arrow'
waitfortarget
lasttarget
```

## lift

Syntax: `lift (serial) [amount]`

Description: This command will lift a specific item and amount. If no amount is provided, `1` is defaulted.

Example(s):

```csharp
lift 0x400EED2A 1
wait 1000
droprelloc 1 1 0
```

## lifttype

Syntax: `lifttype (gfx) [amount]` or `lifttype ('name of item') [amount]`

Description: This command will lift a specific item by type either by the graphic id or by the name. If no amount is provided, `1` is defaulted.

Example(s):

```csharp
lifttype 'robe'
wait 1000
droprelloc 1 1 0

lifttype 0x1FCD
wait 1000
droprelloc 1 1
```

## menu

Syntax: `menu (serial) (index)`

Description: Selects a specific index within a context menu

Example:

```bash
menu 0x123ABC 4
```

## menuresponse

Syntax: `menuresponse (index) (menuId) [hue]`

Description: Responds to a specific menu and menu ID

Example:

```bash
menuresponse 3 4
```

## organizer

Syntax: `organizer (number) ['set']`

Description: This command will execute a specific organizer agent. If the `set` parameter is included, you will instead be prompted to set the organizer agent's hotbag.

Example:

```csharp
// Execute organizer agent 1
organizer 1

// Set a hotbag on organizer agent 4
organizer 4 'set'
```

## overhead

Syntax: `overhead ('text') [color] [serial]`

Description: This command will display a message over your head. Only you can see this.

Example:

```php
if stam = 100
    overhead 'ready to go!'
endif
```

## potion

Syntax: `potion ('potion type')`

Types: `heal, cure, refresh, nightsight, ns, explosion, strength, str, agility`

Description: This command will use a specific potion based on the type.

Example:

```csharp
potion 'agility'

potion 'heal'
```

## promptresponse

Syntax: `promptresponse ('prompt response')`

Description: This command will respond to a prompt triggered from actions such as renaming runes or giving a guild title.

Example:

```csharp
dclicktype 'rune'
waitforprompt
promptresponse 'to home'
```

## restock

Syntax: `restock (number) ['set']`

Description: This command will execute a specific restock agent. If the `set` parameter is included, you will instead be prompted to set the restock agent's hotbag.

Example:

```csharp
// Execute restock agent 1
restock 1

// Set a hotbag on restock agent 4
restock 4 'set'
```

## say

Syntax: `say ('message to send') [hue]` or `msg ('message to send') [hue]`

Description: This command will force your character to say the message passed as the parameter.

Example:

```bash
say 'Hello world!'

say 'Hello world!' 454
```

## script

Syntax: `script 'name'` or `script 'category\name'`

Description: This command will call another script. You can call scripts in categories using `cat1\cat2\scriptname` format.

```csharp
if hp = 40
   script 'healself'
endif

if mana = 40
   script 'magery\domeditation'
endif
```

## scavenger

Syntax: `scavenger ['clear'/'add'/'on'/'off'/'set']`

Description: This command will control the scavenger agent.

* `clear`: Clear scavenger agent cache
* `add`: Select an item to add to the list
* `on`: Turn on the scavenger agent
* `off`: Turn off the scavenger agent
* `set`: Set the scavenger agent's hotbag

Example:

```csharp
// Turn off the agent
scavenger 'off'
```

## sell

Syntax: `sell`

Description: This command will set the Sell agent's hotbag.

Example:

```csharp
// Get a target to set the sell agent hotbag
sell
```

## setability

Syntax: `setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']`

Description: This will set a specific ability on or off. If `on` or `off` is missing, `on` is defaulted.

Example:

```csharp
// enable stun
setability stun
```

## setlasttarget

Syntax: `setlasttarget`

Description: This command will pause the script until you select a target to be set as Last Target.

Example:

```csharp
overhead 'set last target'
setlasttarget
overhead 'set!'
cast 'magic arrow'
waitfortarget
target 'last'
```

## setvar

Syntax: `setvar ('variable')` or `setvariable ('variable')`

Description: This command will pause the script until you select a target to be assigned a variable. Please note, the variable must exist before you can assign values to it.

Example:

```csharp
// 'dummy' exist in the Razor UI
setvar 'dummy'

// Script pauses, cursor should be up

cast 'magic arrow'
waitfortarget
target 'dummy'
```

## skill

Syntax: `skill 'name of skill'` or `skill last`

Description: This command will use a specific skill (assuming it's a usable skill).

!!! tip "Supported skill names"
    `anatomy, animallore, itemidentification, itemid, armslore, begging, peacemaking, peace, cartography, detectinghidden, discord, discordance, evaluatingintelligence, evalint, forensicevaluation, forensiceval, hiding, provocation, provo, inscription, poisoning, spiritspeak, stealing, taming, tasteidentification, tasteid, tracking, meditation, stealth, removetrap, imbuing`

Example:

```csharp
while mana < maxmana
    say 'mediation!'
    skill 'meditation'
    wait 11000
endwhile
```

## sysmsg

Syntax: `sysmsg ('message to display in system message')`

Description: This command will display a message in the lower-left of the client.

Example:

```php
if stam = 100
    sysmsg 'ready to go!'
endif
```

## target

Syntax: `target (serial)` or `target (x) (y) (z)`

Description: This command will target a specific mobile or item or target a specific location based on X/Y/Z coordinates.

Example(s):

```csharp
// Cast lighting, wait for target, target a specific mobile
cast 'lightning'
waitfortarget
target 0xBB3

// Cast fire field, wait for target, target the X, Y, Z at 5923 1145 0
cast 'fire field'
waitfortarget
target 5923 1145 0
```

## targettype

Syntax: `targettype (graphic)` or `targettype ('name of item or mobile type') true` or `targettype ('name of item or mobile type') backpack`

Description: This command will target a specific type of mobile or item based on the graphic id or based on the name of the item or mobile.

!!! tip
    If the optional parameter is passed in as `true` only items within the range of 2 tiles will be considered. If the optional parameter is passed in as `backpack` only items in your backpack will be considered.

Example(s)

```csharp
// Use a dagger, wait and then target any item that is a robe
usetype 'dagger'
waitfortarget
targettype 'robe'

// Use a specific item, wait and then target a specific type
useobject 0x4005ECAF
waitfortarget
targettype 0x1f03

// Use a specific item, wait and then target a specific type within 2 tiles
useobject 0x4005ECAF
waitfortarget
targettype 0x1f03 true
```

## targetrelloc

Syntax: `targetrelloc (x-offset) (y-offset)`

Description: This command will target a specific location on the map relative to your position.

Example(s)

```csharp
// Cast fire field, wait for target and target the location 1 X, 1 Y from my location
cast 'fire field'
waitfortarget
targetrelloc 1 1
```

## undress

Syntax: `undress ['name of dress list']'` or `undress 'LayerName'`

Description: This command will either undress you completely if no dress list is provided. If you provide a dress list, only those specific items will be undressed. Lastly, you can define a layer name to undress.

Example(s):

```csharp
// Remove everything you are wearing
undress

// Remove just specific items in a dress list
undress 'My Sunday Best'

// Remove your shirt and pants
undress 'Shirt'
undrsss 'Pants'
```

## useonce

Syntax: `useonce ['add'/'addcontainer']`

Description: This command will execute the UseOnce agent. If the `add` parameter is included, you can add items to your UseOnce list. If the `addcontainer` parameter is included, you can add all items in a container to your UseOnce list.

Example:

```csharp
// Use top item in my UseOnce list
useonce

// Add item to my list
useonce 'add'

// Add all items in a container to my list
useonce 'addcontainer'
```

## walk

Syntax: `walk ('direction')`

Description: This command will turn and/or walk your player in a certain direction.

Example:

```csharp
walk 'North'
walk 'Up'
walk 'West'
walk 'Left'
walk 'South'
walk 'Down'
walk 'East'
walk 'Right'
```

## wait & pause

Syntax: `wait [time in milliseconds` or `pause [time in milliseconds]`

Description: This command will pause the execution of a script for a given time.

Example:

```php
while stam < 100
    // Wait 5 seconds
    wait 5000
endwhile
```

## waitforgump

Syntax: `waitforgump [gump id]`

Description: This command will wait for a gump. If no `gump id` is provided, it will wait for **any** gump.

Example:

```csharp
waitforgump

waitforgump 4
```

## waitformenu

Syntax: `waitformenu [menu id]`

Description: This command will wait for a context menu. If no `menu id` is provided, it will wait for **any** menu.

Example:

```csharp
waitformenu

waitformenu 4
```

## waitforprompt

Syntax: `waitforprompt`

Description: This command will wait for a prompt before continuing.

Example:

```csharp
dclicktype 'rune'
waitforprompt
promptresponse 'to home'
```

## waitforsysmsg

Syntax: `waitforsysmsg` or `wfsysmsg`

Description: This command will wait a specific message to be added to the system message queue before continuing.

Example:

```csharp
waitforsysmsg 'ready to go'
overhead 'Ready!'
```

## waitfortarget

Syntax: `waitfortarget [pause in milliseconds]` or `wft [pause in milliseconds]`

Description: This command will cause the script to pause until you have a target cursor.  By default it will wait 30 seconds but you can define a specific wait time if you prefer.

Example:

```csharp
cast 'energy bolt'
waitfortarget
hotkey 'Target Closest Enemy'
```

```csharp
cast 'energy bolt'
wft
target 'last'
```

## whisper

Syntax: `whisper ('message to send') [hue]`

Description: This command will force your character to whisper the message passed as the parameter.

Example:

```bash
whisper 'Hello world!'

whisper 'Hello world!' 454
```

## yell

Syntax: `yell ('message to send') [hue]`

Description: This command will force your character to yell the message passed as the parameter.

Example:

```bash
yell 'Hello world!'

yell 'Hello world!' 454
```
