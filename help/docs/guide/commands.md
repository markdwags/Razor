# Overview

The commands issued in the scripting engine are similar to a commands you might enter into a command prompt or shell.  Each line has a starting command, and a set of parameters.  Some of those parameters are required, some are optional.

`command (required) [optional]`

All parameters are shown inside of parenthesis or brackets.  Parameters within parenthesis are **required** while those in brackets are _optional_ and will default to specific value if not provided.

If you want to pass several words as a single parameter you must wrap them using `'` or `"`. For example `'hello goodbye'` is one parameter, while `hello` `goodbye` is two.

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

If you prefix a command with the `@` this will silence any warning/output from the command.  For example:

!!! example

    ```csharp
    // Generate warning if robe is out of reach
    lifttype 'robe'

    // Silence warning if robe is out of reach
    @lifttype 'robe'
    ```

Without the `@` symbol, if a robe isn't available, you would get an warning message telling you that it couldn't find a robe. But since the `@` symbol was provided, no warning is displayed.

# alliance

**Syntax**: `alliance ('message to send')`

**Description**: This command will force your character to say an alliance message passed as the parameter.

!!! example

    === "General"

        ```vim
        alliance 'Allies assemble!'
        ```

# attack

**Syntax**: `attack (serial)` or `attack ('variablename')`

**Description**: Attack a specific serial or variable tied to a serial.

!!! example

    === "Attack Target"

        ```vim
        attack '0x21B4'
        ```

    === "Attack Variable"

        ```vim
        attack 'attackdummy'
        ```
# cast

**Syntax**: `cast ('name of spell')`

**Description**: Cast a spell by name

!!! example

    === "Cast specific spell"

        ```vim
        cast 'blade spirits'
        ```

# clearall

**Syntax**: `clearall`

**Description**: Combines the following actions into one commmand: `Cancel Current Target, Clear Target Queue, Drop What You Are Currently Holding and Clear Drag/Drop Queue` into a single command.

!!! example

    === "Clear on sysmsg message"

        ```vim
        if insysmsg 'cannot find'
            clearall
        endif
        ```

# cleardragdrop

**Syntax**: `cleardragdrop`

**Description**: Clears Razor's the drag/drop queue

!!! example

    === "Clear on sysmsg message"

        ```vim
        if insysmsg 'cannot reach'
            cleardragdrop
        endif
        ```

# clearhands

**Syntax**: `clearhands ('left'/'right'/'both')`

**Description**: Undress your hands based on the param.

!!! example

    === "Undress both hands"

        ```vim
        clearhands 'both'
        ```

    === "Undress left hand"

        ```vim
        clearhands 'left'
        ```
# clearsysmsg

**Syntax**: `clearsysmsg`

**Description**: Clears the internal system message queue

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
# dclick

**Syntax**: `dclick (serial)` or `dclick ('left'/'right'/'hands')`

**Description**: This command will use (double-click) a specific item or mobile or use the item in one of your hands using `left`, `right` or `hands` to use an item in either hand.

!!! example

    === "Double-click a specific item id"

        ```vim
        dclick '0x34AB'
        ```

    === "Double-click a variable"

        ```vim
        dclick 'myvariable'
        ```

    === "Double-click any item in your hands"
        ```vim
        dclick 'hands'
        ```
    
    === "Double-click item in right hand"
        ```vim
        dclick 'right'
        ```

# dclicktype

**Syntax**: `dclicktype ('name of item') OR (graphicID) [true/backpack]`

**Description**: This command will use (double-click) an item type either provided by the name or the graphic ID.

!!! tip "Range Check"
    If you include the optional `true` parameter, items within range (2 tiles) will only be considered. If you include the optional `backpack` parameter, items in your backpack only be considered.

!!! tip "Getting the graphic name or ID"
    To get the name or the ID of item, use the `>info` command in Razor and click on the item. You can use either the `Item Name` or `Id`.

    ![item-command](../images/infocommand.png)

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

# dress

**Syntax**: `dress ('name of dress list')`

**Description**: This command will execute a spec dress list you have defined in Razor.

!!! example

    === "Use existing dress list"

        ```vim
        dress 'My Sunday Best'
        ```
# drop

**Syntax**: `drop (serial) (x) (y) [z]` or `drop (serial) (layer)` or `drop 'ground' (x) (y) [z]`

**Description**: This command will drop the item you are holding either at your feet, on a specific layer , at a specific X/Y/Z location on the ground or within the defined serial.

!!! tip
    The functionality of `drop 'ground' (x) (y) [z]` is also available in with [droprelloc](./#droprelloc).

!!! tip
    A list of available [layers](./layers.md) for reference that can be used with this command.

!!! example

    === "Lift item, drop on your chest/torso"

        ```vim
        lift '0x400D54A7'
        drop 'self' InnerTorso
        ```

    === "Lift item, drop on ground at location"

        ```vim
        lift '0x400D54A7'
        drop 'ground' 5926 1148 0
        ```

# droprelloc

**Syntax**: `droprelloc (x) (y)`

**Description**: This command will drop the item you're holding to a location relative to your position.

Example:

!!! example

    === "Drop Relative Location"

        ```vim
        lift '0x400EED2A'
        wait 1000
        droprelloc 1 1
        ```

# emote

**Syntax**: `emote ('message to send') [hue]`

**Description**: This command will force your character to emote the message passed as the parameter.

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

# guild

**Syntax**: `guild ('message to send')`

**Description**: This command will force your character to say a guild message passed as the parameter.

!!! example

    === "Guild message"

        ```vim
        guild 'Hello fellow guildmates!'
        ```

# gumpresponse

**Syntax**: `gumpresponse (buttonID)`

**Description**: Responds to a specific gump button

!!! example

    === "Gump Response"

        ```vim
        gumpresponse 4
        ```

# gumpclose

**Syntax**: `gumpclose`

**Description**: This command will close the last gump that opened.

!!! example

    === "Close gump"

        ```vim
        gumpclose
        ```

# hotkey

**Syntax**: `hotkey ('name of hotkey')`

**Description**: This command will execute any Razor hotkey by name.

!!! example

    === "Hotkey"

        ```vim
        skill 'detect hidden'
        waitfortarget
        hotkey 'target self'
        ```

# interrupt
**Syntax**: `interrupt`

**Description**: This command will interrupt a casting action. 

!!! example

    === "Example"

        ```vim
        cast 'energy bolt'
        if hp < 10
            interrupt
            cast 'greater healing'
            wft
            target 'self'
        end if
        
        wft
        target 'last'    
        ```


# lasttarget

**Syntax**: `lasttarget`

**Description**: This command will target your last target set in Razor.

!!! example

    === "Cast on last target"

        ```vim
        cast 'magic arrow'
        waitfortarget
        lasttarget
        ```

# lift

**Syntax**: `lift (serial) [amount]`

**Description**: This command will lift a specific item and amount. If no amount is provided, `1` is defaulted.

!!! example

    === "Lift item"

        ```vim
        lift '0x400EED2A'
        wait 1000
        droprelloc 1 1 0
        ```

# lifttype

**Syntax**: `lifttype (gfx) [amount]` or `lifttype ('name of item') [amount]`

**Description**: This command will lift a specific item by type either by the graphic id or by the name. If no amount is provided, `1` is defaulted.

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

# menu

**Syntax**: `menu (serial) (index)`

**Description**: Selects a specific index within a context menu

!!! example

    === "Menu"

        ```vim
        menu '0x123ABC' 4
        ```

# menuresponse

**Syntax**: `menuresponse (index) (menuId) [hue]`

**Description**: Responds to a specific menu and menu ID

!!! example

    === "Description"

        ```vim
        menuresponse 3 4
        ```

# organizer

**Syntax**: `organizer (number) ['set']`

**Description**: This command will execute a specific organizer agent. If the `set` parameter is included, you will instead be prompted to set the organizer agent's hotbag.

!!! example

    === "Execute organizer agent 1"

        ```vim
        organizer 1
        ```

    === "Set a hotbag on organizer agent 4"

        ```vim
        organizer 4 'set'
        ```

# overhead

**Syntax**: `overhead ('text') [color] [serial]`

**Description**: This command will display a message over your head. Only you can see this.

!!! example

    === "Overhead message"

        ```vim
        if stam = 100
            overhead 'ready to go!'
        endif
        ```

# potion

**Syntax**: `potion ('potion type')`

Types: `heal, cure, refresh, nightsight, ns, explosion, strength, str, agility`

**Description**: This command will use a specific potion based on the type.

!!! example

    === "Use agility potion"

        ```vim
        potion 'agility'
        ```

    === "Use heal potion"

        ```vim
        potion 'heal'
        ```

# promptresponse

**Syntax**: `promptresponse ('prompt response')`

**Description**: This command will respond to a prompt triggered from actions such as renaming runes or giving a guild title.

!!! example

    === "Rename a recall rune"

        ```vim
        dclicktype 'rune'
        waitforprompt
        promptresponse 'to home'
        ```

# random

**Syntax**: `random (max number)`

**Description**: This command will generate a random number between 1 and the max number.

!!! example

    === "Random Message Check"

        ```vim
        clearsysmsg

        random 10

        if insysmsg 'Random: 5'
            say 'Hello!'
        else
            say 'Hail!'
        endif
        ```
# restock

**Syntax**: `restock (number) ['set']`

**Description**: This command will execute a specific restock agent. If the `set` parameter is included, you will instead be prompted to set the restock agent's hotbag.

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

# say

**Syntax**: `say ('message to send') [hue]` or `msg ('message to send') [hue]`

**Description**: This command will force your character to say the message passed as the parameter.

!!! example

    === "Say message"

        ```vim
        say 'Hello world!'
        ```

    === "Say message with hue"

        ```vim
        say 'Hello world!' 454
        ```

# script

**Syntax**: `script 'name'` or `script 'category\name'`

**Description**: This command will call another script.

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
# scavenger

**Syntax**: `scavenger ['clear'/'add'/'on'/'off'/'set']`

**Description**: This command will control the scavenger agent.

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

# sell

**Syntax**: `sell`

**Description**: This command will set the Sell agent's hotbag.

!!! example

    === "Set Agent Hotbag"

        ```vim
        sell
        ```

# setability

**Syntax**: `setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']`

**Description**: This will set a specific ability on or off. If `on` or `off` is missing, `on` is defaulted.

!!! example

    === "Set stun"

        ```vim
        setability 'stun'
        ```

    === "Turn off stun"

        ```vim
        setability 'stun' off
        ```

# setlasttarget

**Syntax**: `setlasttarget`

**Description**: This command will pause the script until you select a target to be set as Last Target.

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
# setvar

**Syntax**: `setvar ('variable') [timeout` or `setvariable ('variable') [timeout]`

**Description**: This command will pause the script until you select a target to be assigned a variable. Please note, the variable must exist before you can assign values to it. Default timeout is 30 seconds that can be changed by passing in a new timeout value in milliseconds.

!!! example

    === "Set variable and use it"

        ```vim
        setvar 'dummy'

        cast 'magic arrow'
        waitfortarget
        target 'dummy'
        ```

# skill

**Syntax**: `skill 'name of skill'` or `skill last`

**Description**: This command will use a specific skill (assuming it's a usable skill).

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

# sysmsg

**Syntax**: `sysmsg ('message to display in system message')`

**Description**: This command will display a message in the lower-left of the client.

!!! example

    === "Description"

        ```vim hl_lines="2"
        if stam = 100
            sysmsg 'ready to go!'
        endif
        ```

# target

**Syntax**: `target (closest/random/next/prev) [type1,type2] [humanoid/monster]` or `target (closest/random/next/prev) [type1!type2] [humanoid/monster]` or `target (serial)` or `target (clear/cancel)`

**Description**: This command will target a specific mobile based either the type searched for or the serial. If you provide a list of target types, you can use `,` for a general list and `!` for a priority list.

| Type              | Notoriety Name                                   | Notoriety Color                            |
| ----------------- | ------------------------------------------------ | ------------------------------------------ |
| `nonfriendly`     | Attackable, Criminal, Enemy, Murderer            | Gray (but not criminal), Gray, Orange, Red |
| `friendly`        | Innocent, Guild/Ally                             | Blue, Green                                |
| `enemy`           | Enemy                                            | Orange                                     |
| `red`/`murderer`  | Murderer                                         | Red                                        |
| `gray`/`grey`     | Attackable, Criminal                             | Gray (but not criminal), Gray              |
| `criminal`        | Criminal                                         | Gray                                       |
| `blue`/`innocent` | Innocent                                         | Blue                                       |
| `friend`          | Based on your [friends list](../../help/friends) | Any                                        |

| List Type | Delimiter | Description                                                                                                        |
| --------- | --------- | ------------------------------------------------------------------------------------------------------------------ |
| General   | `,`       | When the script tries to acquire a target, it will look for all the target types passed in the list. See Examples. |
| Priority  | `!`       | When the script tries to acquire a target, it will prioritize each type. See Examples.                             |

!!! example

    === "Specific target"

        ```vim
        cast 'lightning'
        waitfortarget
        target '0xBB3'
        ```

    === "General list"

        ```csharp
        cast 'lightning'
        waitfortarget

        // General list using a ,
        // If a red mobile is closer than a gray mobile, this will target the red mobile
        target closest gray,red
        ```

    === "Priority list"

        ```csharp
        cast 'lightning'
        waitfortarget

        // Priority list using a !
        // If a red mobile is closer than a gray mobile, this will target the gray mobile
        target closest gray!red
        ```

    === "Target closest red"

        ```vim
        cast 'lightning'
        waitfortarget
        target closest 'red'
        ```

    === "Target closest gray or red monster"

        ```vim
        cast 'lightning'
        waitfortarget
        target closest 'gray,red' monster
        ```

    === "Target random mobile"

        ```vim
        cast 'lightning'
        waitfortarget
        target random
        ```

    === "Target random red monster"

        ```vim
        cast 'lightning'
        waitfortarget
        target random 'red' monster
        ```

    === "Next humanoid target"

        ```vim        
        target next humanoid
        ```    

    === "Cancel current target"

        ```vim        
        target cancel
        ```
    
    === "Clear target queue"

        ```vim        
        target clear
        ```

# targetrelloc

**Syntax**: `targetrelloc (x-offset) (y-offset)`

**Description**: This command will target a specific location on the map relative to your position.

!!! example

    === "Target 1 X, 1 Y from player location"

        ```vim
        cast 'fire field'
        waitfortarget
        targetrelloc 1 1
        ```

# targetloc

**Syntax**: `targetloc (x) (y) (z)`

**Description**: This command will target a specific location on the map.

!!! example

     === "Specific location"

        ```vim
        cast 'fire field'
        waitfortarget
        targetloc 5923 1145 0
        ```

# targettype

**Syntax**: `targettype (graphic)` or `targettype ('name of item or mobile type') true` or `targettype ('name of item or mobile type') backpack`

**Description**: This command will target a specific type of mobile or item based on the graphic id or based on the name of the item or mobile.

!!! tip "Range Check"
    If the optional parameter is passed in as `true` only items within the range of 2 tiles will be considered. If the optional parameter is passed in as `backpack` only items in your backpack will be considered.

!!! tip "Getting the graphic name or ID"
    To get the name or the ID of item, use the `>info` command in Razor and click on the item. You can use either the `Item Name` or `Id`.

    ![item-command](../images/infocommand.png)

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

# undress

**Syntax**: `undress ['name of dress list']'` or `undress 'LayerName'`

**Description**: This command will either undress you completely if no dress list is provided. If you provide a dress list, only those specific items will be undressed. Lastly, you can define a layer name to undress.

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

# useonce

**Syntax**: `useonce ['add'/'addcontainer']`

**Description**: This command will execute the UseOnce agent. If the `add` parameter is included, you can add items to your UseOnce list. If the `addcontainer` parameter is included, you can add all items in a container to your UseOnce list.

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

# virtue

**Syntax**: `virtue ('honor'/'sacrifice'/'valor')`

**Description**: This command will invoke Honor, Sacrifice or Valor.

!!! example

    === "Invoke Honor"

        ```vim
        virtue 'honor'
        ```

    === "Invoke Sacrifice"

        ```vim
        virtue 'sacrifice'
        ```

    === "Invoke Valor"

        ```vim
        virtue 'valor'
        ```

# walk

**Syntax**: `walk ('direction')`

**Description**: This command will turn and/or walk your player in a certain direction.

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

# wait & pause

**Syntax**: `wait [time in milliseconds` or `pause [time in milliseconds]`

**Description**: This command will pause the execution of a script for a given time.

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
# waitforgump

**Syntax**: `waitforgump [gump id/'any'] [timeout]`

**Description**: This command will wait for a gump. If no `gump id` is provided, it will wait for **any** gump. Default timeout is 30 seconds that can be changed by passing in a new timeout value in milliseconds.

!!! tip "Timeout parameter"
    To modify the default 30 second timeout for any gump, you must include include the `any` keyword before the timeout.

    `waitforgump 'any' 5000` will wait for 5 seconds

    `waitforgump 5000` will wait 30 seconds for a gump with the id of 5000

!!! example

    === "Wait for any gump"

        ```vim
        waitforgump 'any' 
        ```

    === "Wait for any gump for 10 seconds"

        ```vim
        waitforgump 'any' 10000
        ```

    === "Wait for specific gump"

        ```vim
        waitforgump 4
        ```

    === "Wait for specific gump for 5 seconds"

        ```vim
        waitforgump 34252 5000
        ```

# waitformenu

**Syntax**: `waitformenu [menu id/'any'] [timeout]`

**Description**: This command will wait for a context menu. If no `menu id` is provided, it will wait for **any** menu. Default timeout is 30 seconds that can be changed by passing in a new timeout value in milliseconds.

!!! tip "Timeout parameter"
    To modify the default 30 second timeout for any menu, you must include include the `any` keyword before the timeout.

    `waitformenu 'any' 5000` will wait for 5 seconds

    `waitformenu 5000` will wait 30 seconds for a menu with the id of 5000

!!! example

    === "Wait for any menu"

        ```vim
        waitformenu
        ```

    === "Wait for any menu for 5 seconds"

        ```vim
        waitformenu 'any' 5000
        ```

    === "Wait for specific menu"

        ```vim
        waitformenu 4
        ```

# waitforprompt

**Syntax**: `waitforprompt [promptid/'any'] [timeout]`

**Description**: This command will wait for a prompt before continuing. If no `prompt id` is provided, it will wait for **any** prompt. Default timeout is 30 seconds that can be changed by passing in a new timeout value in milliseconds.

!!! tip "Timeout parameter"
    To modify the default 30 second timeout for any gump, you must include include the `any` keyword before the timeout.

    `waitforprompt 'any' 5000` will wait for 5 seconds

    `waitforprompt 5000` will wait 30 seconds for a prompt with the id of 5000

!!! example

    === "Renaming recalling rune"

        ```vim hl_lines="2"
        dclicktype 'rune'
        waitforprompt
        promptresponse 'to home'
        ```

    === "Rename recalling rune with wait"

        ```vim hl_lines="2"
        dclicktype 'rune'
        waitforprompt 'any' 5000
        promptresponse 'to home'
        ```

# waitforsysmsg

**Syntax**: `waitforsysmsg 'message to wait for' [timeout]` or `wfsysmsg 'message to wait for' [timeout]`

**Description**: This command will wait a specific message to be added to the system message queue before continuing.  Default timeout is 30 seconds that can be changed by passing in a new timeout value in milliseconds.

!!! example

    === "Wait for system message"

        ```vim hl_lines="1"
        waitforsysmsg 'ready to go'
        overhead 'Ready!'
        ```

    === "Wait for system message for 5 seconds"

        ```vim hl_lines="1"
        waitforsysmsg 'ready to go' 5000
        overhead 'Done waiting'
        ```

# waitfortarget

**Syntax**: `waitfortarget [pause in milliseconds]` or `wft [pause in milliseconds]`

**Description**: This command will cause the script to pause until you have a target cursor.  By default it will wait 30 seconds but you can define a specific wait time if you prefer.

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

# whisper

**Syntax**: `whisper ('message to send') [hue]`

**Description**: This command will force your character to whisper the message passed as the parameter.

!!! example

    === "Whisper message"

        ```vim
        whisper 'Hello world!'
        ```

    === "Whisper message with hue"

        ```vim
        whisper 'Hello world!' 454
        ```

# yell

**Syntax**: `yell ('message to send') [hue]`

**Description**: This command will force your character to yell the message passed as the parameter.

!!! example

    === "Yell message"

        ```vim
        yell 'Hello world!'
        ```

    === "Yell message with hue"

        ```vim
        yell 'Hello world!' 454
        ```
