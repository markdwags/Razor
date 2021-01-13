# Commands Overview

The commands issued in the scripting engine are similar to a commands you might enter into a command prompt or shell.  Each line has a starting command, and a set of parameters.  Some of those parameters are required, some are optional.

`command (required) [optional]`

All parameters are shown inside of parenthesis or brackets.  Parameters within parenthesis are **required** while those in brackets are _optional_ and will default to specific value if not provided.

If you want to pass several words as a single parameter you must wrap them using `'` or `"`. For example `"hello goodbye"` is one parameter, while `hello` `goodbye` is two.

For example, if your script is something like:

```python
# Say 'Hello'
say Hello friends!
```

You will end up just saying `Hello`.  Instead, you if you wrap the words in single (') or double quotes (") the engine will see it as a single parameter.

```python
# Say 'Hello friends!'
say 'Hello friends!'
```

Here the list of the support commands:

## alliance

Syntax: `alliance ('message to send')`

Description: This command will force your character to say an alliance message passed as the parameter.

!!! example

    === "General"

        ```vim
        alliance 'Allies assemble!'
        ```

## attack

Syntax: `attack (serial)` or `attack ('variablename')`

Description: Attack a specific serial or variable tied to a serial.

!!! example

    === "Attack Target"

        ```vim
        attack '0x21B4'
        ```

    === "Attack Variable"

        ```vim
        attack 'attackdummy'
        ```
## cast

Syntax: `cast ('name of spell')`

Description: Cast a spell by name

!!! example

    === "Cast specific spell"

        ```vim
        cast 'blade spirits'
        ```
## clearsysmsg

Syntax: `clearsysmsg`

Description: Clears the internal system message queue

!!! example

    === "General"

        ```vim
        sysmsg 'hello'
        sysmsg 'bye'

        if insysmsg 'hello'
            say 'hello!'
        endif

        clearsysmsg

        if insysmsg 'bye'
            say 'this condition wont be met since it was cleared on line 8'
        endif
        ```
## dclick

Syntax: `dclick (serial)` or `useobject (serial)`

Description: This command will use (double-click) a specific item or mobile.

!!! example

    === "Double-click a specific item id"

        ```vim
        dclick '0x34AB'
        ```

    === "Double-click a variable"

        ```vim
        dclick 'myvariable'
        ```

## dclicktype

Syntax: `dclicktype ('name of item') OR (graphicID) [true/backpack]`

Description: This command will use (double-click) an item type either provided by the name or the graphic ID.

!!! tip
    If you include the optional `true` parameter, items within range (2 tiles) will only be considered. If you include the optional `backpack` parameter, items in your backpack only be considered.

!!! example

    === "Use any item"

        ```vim
        dclicktype 'dagger'
        waitfortarget
        targettype 'robe'
        ```

    === "..with range check"

        ```vim hl_lines="1 3"
        dclicktype 'dagger' true
        waitfortarget
        targettype 'robe' true
        ```

    === "..with backpack only"

        ```vim hl_lines="1 3"
        dclicktype 'dagger' backpack
        waitfortarget
        targettype 'robe' backpack
        ```

## dress

Syntax: `dress ('name of dress list')`

Description: This command will execute a spec dress list you have defined in Razor.

!!! example

    === "Use existing dress list"

        ```vim
        dress 'My Sunday Best'
        ```
## drop

Syntax: `drop (serial) (x) (y) [z]` or `drop (serial) (layer)` or `drop 'ground' (x) (y) [z]`

Description: This command will drop the item you are holding either at your feet, on a specific layer , at a specific X/Y/Z location on the ground or within the defined serial.

!!! tip
    The functionality of `drop 'ground' (x) (y) [z]` is also available in with [droprelloc](./#droprelloc).

!!! tip
    A list of available [layers](./layers.md) for reference that can be used with this command.

!!! example

    === "Lift item, drop on your chest/torso"

        ```vim
        lift '0x400D54A7' 1
        drop 'self' InnerTorso
        ```

    === "Lift item, drop on ground at location"

        ```vim
        lift '0x400D54A7' 1
        drop 'ground' 5926 1148 0
        ```

## droprelloc

Syntax: `droprelloc (x) (y)`

Description: This command will drop the item you're holding to a location relative to your position.

Example:

!!! example

    === "Drop Relative Location"

        ```vim
        lift '0x400EED2A' 1
        wait 1000
        droprelloc 1 1
        ```

## emote

Syntax: `emote ('message to send') [hue]`

Description: This command will force your character to emote the message passed as the parameter.

!!! tip
    This command will append `*` around the emote so `emote 'smiles'` will be displayed in game as `*smiles*`.

!!! example

    === "Emote"

        ```vim
        emote 'smiles'
        ```

    === "Emote with hue"

        ```vim
        emote 'smiles in another color' 454
        ```

## guild

Syntax: `guild ('message to send')`

Description: This command will force your character to say a guild message passed as the parameter.

!!! example

    === "Guild message"

        ```vim
        guild 'Hello fellow guildmates!'
        ```

## gumpresponse

Syntax: `gumpresponse (buttonID)`

Description: Responds to a specific gump button

!!! example

    === "Gump Response"

        ```vim
        gumpresponse 4
        ```

## gumpclose

Syntax: `gumpclose`

Description: This command will close the last gump that opened.

!!! example

    === "Close gump"

        ```vim
        gumpclose
        ```

## hotkey

Syntax: `hotkey ('name of hotkey')`

Description: This command will execute any Razor hotkey by name.

!!! example

    === "Hotkey"

        ```vim
        skill 'detect hidden'
        waitfortarget
        hotkey 'target self'
        ```

## lasttarget

Syntax: `lasttarget`

Description: This command will target your last target set in Razor.

!!! example

    === "Cast on last target"

        ```vim
        cast 'magic arrow'
        waitfortarget
        lasttarget
        ```

## lift

Syntax: `lift (serial) [amount]`

Description: This command will lift a specific item and amount. If no amount is provided, `1` is defaulted.

!!! example

    === "Lift item"

        ```vim
        lift '0x400EED2A' 1
        wait 1000
        droprelloc 1 1 0
        ```

## lifttype

Syntax: `lifttype (gfx) [amount]` or `lifttype ('name of item') [amount]`

Description: This command will lift a specific item by type either by the graphic id or by the name. If no amount is provided, `1` is defaulted.

!!! example

    === "Lift by name"

        ```vim
        lifttype 'robe'
        wait 1000
        droprelloc 1 1
        ```

    === "Lift by item id"

        ```vim
        lifttype '0x1FCD'
        wait 1000
        droprelloc 1 1
        ```

## menu

Syntax: `menu (serial) (index)`

Description: Selects a specific index within a context menu

!!! example

    === "Menu"

        ```vim
        menu '0x123ABC' 4
        ```

## menuresponse

Syntax: `menuresponse (index) (menuId) [hue]`

Description: Responds to a specific menu and menu ID

!!! example

    === "Description"

        ```vim
        menuresponse 3 4
        ```

## organizer

Syntax: `organizer (number) ['set']`

Description: This command will execute a specific organizer agent. If the `set` parameter is included, you will instead be prompted to set the organizer agent's hotbag.

!!! example

    === "Execute organizer agent 1"

        ```vim
        organizer 1
        ```

    === "Set a hotbag on organizer agent 4"

        ```vim
        organizer 4 'set'
        ```

## overhead

Syntax: `overhead ('text') [color] [serial]`

Description: This command will display a message over your head. Only you can see this.

!!! example

    === "Overhead message"

        ```vim
        if stam = 100
            overhead 'ready to go!'
        endif
        ```

## potion

Syntax: `potion ('potion type')`

Types: `heal, cure, refresh, nightsight, ns, explosion, strength, str, agility`

Description: This command will use a specific potion based on the type.

!!! example

    === "Use agility potion"

        ```vim
        potion 'agility'
        ```

    === "Use heal potion"

        ```vim
        potion 'heal'
        ```

## promptresponse

Syntax: `promptresponse ('prompt response')`

Description: This command will respond to a prompt triggered from actions such as renaming runes or giving a guild title.

!!! example

    === "Rename a recall rune"

        ```vim
        dclicktype 'rune'
        waitforprompt
        promptresponse 'to home'
        ```

## restock

Syntax: `restock (number) ['set']`

Description: This command will execute a specific restock agent. If the `set` parameter is included, you will instead be prompted to set the restock agent's hotbag.

!!! example

    === "Use Restock Agent 1"

        ```vim
        if count garlic < 4
            restock 1
        endif
        ```

    === "Set a hotbag on Restock Agent 4"

        ```vim
        restock 4 'set'
        ```

## say

Syntax: `say ('message to send') [hue]` or `msg ('message to send') [hue]`

Description: This command will force your character to say the message passed as the parameter.

!!! example

    === "Say message"

        ```vim
        say 'Hello world!'
        ```

    === "Say message with hue"

        ```vim
        say 'Hello world!' 454
        ```

## script

Syntax: `script 'name'` or `script 'category\name'`

Description: This command will call another script.

!!! tip
    You can call scripts in categories using `cat1\cat2\scriptname` format.

!!! example

    === "Execute script"

        ```vim hl_lines="2"
        if hp = 40
            script 'healcure'
        endif
        ```

    === "Execute script in category"

        ```vim hl_lines="2"
        if mana = 40
            script 'magery\meditation'
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

!!! example

    === "Turn off scavenger"

        ```vim
        scavenger 'off'
        ```

## sell

Syntax: `sell`

Description: This command will set the Sell agent's hotbag.

!!! example

    === "Set Agent Hotbag"

        ```vim
        sell
        ```

## setability

Syntax: `setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']`

Description: This will set a specific ability on or off. If `on` or `off` is missing, `on` is defaulted.

!!! example

    === "Set stun"

        ```vim
        setability 'stun'
        ```

    === "Turn off stun"

        ```vim
        setability 'stun' off
        ```

## setlasttarget

Syntax: `setlasttarget`

Description: This command will pause the script until you select a target to be set as Last Target.

!!! example

    === "Set last target and cast"

        ```vim
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

!!! example

    === "Set variable and use it"

        ```vim
        setvar 'dummy'

        cast 'magic arrow'
        waitfortarget
        target 'dummy'
        ```

## skill

Syntax: `skill 'name of skill'` or `skill last`

Description: This command will use a specific skill (assuming it's a usable skill).

!!! tip "Supported skill names"
    `anatomy, animallore, itemidentification, itemid, armslore, begging, peacemaking, peace, cartography, detectinghidden, discord, discordance, evaluatingintelligence, evalint, forensicevaluation, forensiceval, hiding, provocation, provo, inscription, poisoning, spiritspeak, stealing, taming, tasteidentification, tasteid, tracking, meditation, stealth, removetrap, imbuing`

!!! example

    === "Use meditation"

        ```vim
        while mana < maxmana
            say 'mediation!'
            skill 'meditation'
            wait 11000
        endwhile
        ```

## sysmsg

Syntax: `sysmsg ('message to display in system message')`

Description: This command will display a message in the lower-left of the client.

!!! example

    === "Description"

        ```vim hl_lines="2"
        if stam = 100
            sysmsg 'ready to go!'
        endif
        ```

## target

Syntax: `target (serial)` or `target (x) (y) (z)`

Description: This command will target a specific mobile or item or target a specific location based on X/Y/Z coordinates.

!!! example

    === "Specific target"

        ```vim
        cast 'lightning'
        waitfortarget
        target 0xBB3
        ```

    === "Specific location"

        ```vim
        cast 'fire field'
        waitfortarget
        target 5923 1145 0
        ```

## targettype

Syntax: `targettype (graphic)` or `targettype ('name of item or mobile type') true` or `targettype ('name of item or mobile type') backpack`

Description: This command will target a specific type of mobile or item based on the graphic id or based on the name of the item or mobile.

!!! tip
    If the optional parameter is passed in as `true` only items within the range of 2 tiles will be considered. If the optional parameter is passed in as `backpack` only items in your backpack will be considered.

!!! example

    === "Target by name"

        ```vim hl_lines="3"
        dclicktype 'dagger'
        waitfortarget
        targettype 'robe'
        ```

    === "Target by type"

        ```vim hl_lines="3"
        dclick '0x4005ECAF'
        waitfortarget
        targettype '0x1F03'
        ```

    === "Target by type in range" 

        ```vim hl_lines="3"
        dclick '0x4005ECAF'
        waitfortarget
        targettype '0x1F03' true
        ```

    === "Target by name in backpack" 
           
        ```vim hl_lines="3"
        dclicktype 'dagger'
        waitfortarget
        targettype 'robe'
        ```

## targetrelloc

Syntax: `targetrelloc (x-offset) (y-offset)`

Description: This command will target a specific location on the map relative to your position.

!!! example

    === "Target 1 X, 1 Y from player location"

        ```vim
        cast 'fire field'
        waitfortarget
        targetrelloc 1 1
        ```

## undress

Syntax: `undress ['name of dress list']'` or `undress 'LayerName'`

Description: This command will either undress you completely if no dress list is provided. If you provide a dress list, only those specific items will be undressed. Lastly, you can define a layer name to undress.

!!! tip
    Available [layers](./layers.md) for reference

!!! example

    === "Full naked"

        ```vim
        undress
        ```

    === "Specfic items in dress list"

        ```vim
        undress 'My Sunday Best'
        ```

    === "Remove your shirt and pants"

        ```vim
        undress 'Shirt'
        undrsss 'Pants'
        ```

## useonce

Syntax: `useonce ['add'/'addcontainer']`

Description: This command will execute the UseOnce agent. If the `add` parameter is included, you can add items to your UseOnce list. If the `addcontainer` parameter is included, you can add all items in a container to your UseOnce list.

!!! example

    === "Use top item"

        ```vim
        useonce
        ```

    === "Add to list"

        ```vim
        useonce 'add'
        ```

    === "Add to container"

        ```vim
        useonce 'addcontainer'
        ```

## walk

Syntax: `walk ('direction')`

Description: This command will turn and/or walk your player in a certain direction.

!!! example

    === "Walk around"

        ```vim
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

!!! tip
    `1000` milliseconds is equal to `1` second.
    
    `1000 x number of seconds = total milliseconds`

!!! example

    === "Wait 5 seconds"

        ```vim
        while stam < 100    
            wait 5000
        endwhile
        ```
## waitforgump

Syntax: `waitforgump [gump id]`

Description: This command will wait for a gump. If no `gump id` is provided, it will wait for **any** gump.

!!! example

    === "Wait for any gump"

        ```vim
        waitforgump
        ```

    === "Wait for specific gump"

        ```vim
        waitforgump 4
        ```
## waitformenu

Syntax: `waitformenu [menu id]`

Description: This command will wait for a context menu. If no `menu id` is provided, it will wait for **any** menu.

!!! example

    === "Wait for any menu"

        ```vim
        waitformenu
        ```

    === "Wait for specific menu"

        ```vim
        waitformenu 4
        ```

## waitforprompt

Syntax: `waitforprompt`

Description: This command will wait for a prompt before continuing.

!!! example

    === "Renaming recalling rune"

        ```vim hl_lines="2"
        dclicktype 'rune'
        waitforprompt
        promptresponse 'to home'
        ```

## waitforsysmsg

Syntax: `waitforsysmsg` or `wfsysmsg`

Description: This command will wait a specific message to be added to the system message queue before continuing.

!!! example

    === "Wait for system message"

        ```vim hl_lines="1"
        waitforsysmsg 'ready to go'
        overhead 'Ready!'
        ```

## waitfortarget

Syntax: `waitfortarget [pause in milliseconds]` or `wft [pause in milliseconds]`

Description: This command will cause the script to pause until you have a target cursor.  By default it will wait 30 seconds but you can define a specific wait time if you prefer.

!!! example

    === "Cast and wait"

        ```vim hl_lines="2"
        cast 'energy bolt'
        waitfortarget
        hotkey 'Target Closest Enemy'
        ```

    === "Using 'wft' shorthand on last target"

        ```vim hl_lines="2"
        cast 'energy bolt'
        wft
        target 'last'
        ```

## whisper

Syntax: `whisper ('message to send') [hue]`

Description: This command will force your character to whisper the message passed as the parameter.

!!! example

    === "Whisper message"

        ```vim
        whisper 'Hello world!'
        ```

    === "Whisper message with hue"

        ```vim
        whisper 'Hello world!' 454
        ```

## yell

Syntax: `yell ('message to send') [hue]`

Description: This command will force your character to yell the message passed as the parameter.

!!! example

    === "Yell message"

        ```vim
        yell 'Hello world!'
        ```

    === "Yell message with hue"

        ```vim
        yell 'Hello world!' 454
        ```
