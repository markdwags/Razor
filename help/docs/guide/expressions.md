# Statements & Loops

As found in the Razor macro system, you can use `if`, `for`, `foreach`, and `while` when writing a script to add some basic logic and flows.

## if

**Syntax**:

```razor
if (expression)
    // commands to execute if this expression is true
elseif (expression)
    // commands to execute if the first expression was false, and this expression is true
else
    // commands to execute if both expressions above were false, default to running these
endif
```

**Description**: This selects a path to execute based on the value of a boolean expression. An `if` statement can be combined with `else` or `elseif` to choose two or more distinct paths based on the result of the boolean expression. All `if` statements must end with `endif`.

!!! example

    === "if/endif"

        ```razor
        if stam = 100
            overhead 'Stam at 100!'
        endif
        ```

    === "if/elseif/else"

        ```razor
        if stam = 100
            say 'Stamina full'
        elseif stam < 20
            say 'Still a ways to go'
        elseif stam < 60
            say 'Getting closer'
        else
            say 'waiting'
        endif
        ```

    === "if/elseif"

        ```razor
        if stam = 100
            say 'Stamina full'
        elseif stam < 20
            say 'Still a ways to go'
        elseif stam < 60
            say 'Getting closer'
        endif
        ```

    === "Using 'not'"

        ```razor
        if not stam = 100
            say 'Stamina is not full'    
        endif
        ```

## for

**Syntax**:

```razor
for ('number')
    // commmands to execute the number of times defined in the for statement
endfor
```

**Description**: This allows you to execute a block of commands a specific number of times. All `for` loops must end with `endfor`.

!!! tip
    You can use the [index](variables.md#index-variable) variable to track your position in the for loop.

When using `for` or `while` you have access to the `index` variable. This can be used with `overhead` (for example) to indicate the current loop number.

!!! example

    === "Say 'Hello' 10 times"

        ```razor        
        for 10
            say 'hello'
            wait 1000
        endfor
        ```

    === "Output current loop number"

        ```razor
        for 10
            wait 1000
            overhead 'index'
        endfor
        ```

## foreach

**Syntax**:

```razor
foreach ('variable') in ('list')
    // commands to execute for each item that is in a list
endfor
```

**Description**: This allows you to iterate over a list containing values. All `foreach` loops must end with `endfor`.

!!! example

    === "Loop through each item in a list"

        ```razor        
        createlist 'mylist'
        pushlist 'mylist' '0x1'
        pushlist 'mylist' '0x2'

        foreach 'item' in 'mylist'
           overhead 'item'
        endfor
        ```

## while

**Syntax**:

```razor
while ('expression')
    // commands to execute as long as the expression is remains true
endwhile
```

**Description**: This allows you execute a block of commands while a certain expression is true. All `while` loops must end with `endwhile`.

!!! example

    === "While"

        ```razor
        while hits < 100
            say 'I need a heal!'
            wait 1000
        endwhile
        ```

# Expression Operators

When using the `if` or `while` conditions, you can access the following expressions in the statement.

The following operators are supported:

| Operator | Description           |
| -------- | --------------------- |
| =        | Equal                 |
| ==       | Equal                 |
| !=       | Not equal             |
| <        | Less than             |
| <=       | Less than or equal    |
| >        | Greater than          |
| >=       | Greater than or equal |

# Expressions

Expressions are combined with statements like `if` and `while` to alter the execution path of your script.

Below are the several different types of expressions you can use broken into categories.

# List Expressions

## inlist

- `inlist ('list name') ('list item')`

Description: Used to check if a specific item is in a list

!!! example

    === "General"

        ```razor
        if inlist 'my_list' 'item1'
            overhead 'found item!'
        endif
        ```   

## list

- `list ('list name')`

Description: Used to check how many items are in a specific list

!!! example

    === "General"

        ```razor
        if list 'mylist' = 10
            overhead '10 items in your list'
        endif
        ```

## listexists

- `listexists ('list name')`

Description: Used to check if a list exists with a specific name.

!!! example

    === "General"

        ```razor
        if not listexists 'mylist'
            createlist 'mylist'
        endif
        ```

## poplist

- `poplist ('list name') ('list value'/'front'/'back')`

Description: This command will remove (pop) an item from the list. You can either pass in the specific item, or use `front` or `back` to remove the item from the front or back of the list.

!!! example

    === "General"

        ```razor
        if poplist 'mylist' back as 'item'
            overhead 'item'
        endif
        ```

# Misc Expressions

## count/counter

- `count ('name of counter')`
- `counter ('name of counter')`
- `count ('name of item') [hue]`
- `count (graphicID) [hue]`

Description: Used to get the current amount of a specific item in player's backpack.
Omitting the hue argument will result in count of all items of the specified type regardless of their hue.
The expression can be used either directly by item type and hue, or by referencing a named counter manually set up in the Counters tab.
[More info](../help/displaycounters.md#counters)

!!! example

    === "Counting garlic"

        ```razor
        if count 'garlic' < 5
            say 'getting low on garlic'
        endif
        ```
        
    === "Counting runebooks"
    
        ```razor
        if count 'spellbook' '1121' == 0
            say 'no runebooks found!'
        endif
        ```
        
    === "Detecting fancy coins"

        ```razor
        if count 'gold coin' > count 'gold coin' 0
            overhead 'woot woot fancy coins in the pack!'
        endif
        ```

## findtype

- `findtype ('name of item') [inrangecheck (true/false)/backpack] [hue]` OR `findtype (graphicID) [inrangecheck (true/false)/backpack] [hue]`

Description: Used to check if a specific item name of graphic ID exists. Range check, if true, will check within 2 tiles.

!!! tip "The `as` keyword"
    If you use `findtype` along with `as` you can assign a temporary variable to use throughout the script. See example below.

!!! tip "In-Game Info Gump"
    Not sure what name to enter or graphic ID to enter? Type `>info` and click on any item or mobile for more information.

    ![item-command](../images/infocommand.png)

    Click the blue dot next to the value you want to copy to the clipboard.

!!! example

    === "Find a saw"

        ```razor
        if findtype 'saw'
            say 'found saw'
        endif
        ```
    
    === "Find a saw using graphic id"

        ```razor
        if findtype '4148'
            say 'found saw'
        endif
        ```

    === "Find a saw within 2 tiles"

        ```razor
        if findtype 'saw' true
            say 'found saw within 2 tiles'
        endif
        ```

    === "Find a saw in your backpack"

        ```razor
        if findtype 'saw' backpack
            say 'found saw in my pack'
        endif
        ```

    === "Find a dagger and use it (using as)"

        ```razor hl_lines="1"
        if findtype 'dagger' as 'mydagger'
            overhead 'found dagger'
            dclick 'mydagger'
        endif
        ```

    === "Find a dagger in backpack with a hue"

        ```razor
        if findtype 'dagger' backpack 45 as 'mydagger'
            overhead 'found dagger'
            dclick 'mydagger'
        endif
        ```

## insysmsg

- `insysmsg ('message to look for')`
- `insysmessage ('message to look for')`

Description: Used to check if certain text appears within the system message log.

!!! tip "System Message Queue"
    Not sure if a specific message is in Razor's system message queue? Type `>sysmsgs` to see what Razor can find.

    Using `clearsysmsg` will clear out the queue completely.

!!! example

    === "Check for message"

        ```razor
        if insysmsg 'too far away'
            overhead 'You are too far away'
        endif
        ```   

## itemcount

- `itemcount`

Description: Used to return the current number of items you're carrying

!!! example

    === "General"

        ```razor
        if itemcount < 125
            overhead 'I still have room!'
        endif
        ```

## queued

- `queued`

Description: Used to check if your current queue is active (from restocking, organizing, etc)

!!! example

    === "General"

        ```razor
            if queued
                overhead 'Queue is active'
            else
                overhead 'No queue'
            endif
        ```

    === "Organizer"

        ```razor
        overhead 'Organizing'

        organizer 1

        while queued
            overhead 'Currently Organizing'
            wait 500 
        endwhile

        overhead 'Organized'
        ```

    === "Restock"

        ```razor
        overhead 'Restocking'

        restock 11
        waitfortarget 
        target 'self'

        while queued
            overhead 'Currently restocking'
            wait 500 
        endwhile

        overhead 'Restocked'
        ```

## targetexists

- `targetexists ['any'/'beneficial'/'harmful'/'neutral']`

Description: Used to check if the client current has a target cursor up

!!! example

    === "General"

        ```razor
        if targetexists 'beneficial'
            overhead 'Beneficial target found'
        elseif targetexists 'harmful'
            overhead 'Harmful target found'
        endif
        ```

## varexist

- `varexist`
- `varexists`

Description: Used to check if a variable exists.

!!! example

    === "General"

        ```razor
        if not varexist 'myrunebook'
            overhead 'Runebook variable not found -- select one'
            setvar 'myrunebook'
        endif

        dclick 'myrunebook'
        waitforgump 'any'
        gumpresponse 5
        ```

# Player Attribute Expressions

## diffhits

- `diffhits`
- `diffhp`

Description: Used to get the difference between you max hits and current hits.

!!! example

    === "General"

        ```razor
        if diffhits > 40
            overhead 'I need a heal!'
        endif
        ```

## diffmana

- `diffmana`

Description: Used to get the difference between you max mana and current mana.

!!! example

    === "General"

        ```razor
        if diffmana > 40
            skill 'Meditation'
        endif
        ```

## diffstam

- `diffstam`

Description: Used to get the difference between you max stamina and current stamina.

!!! example

    === "General"

        ```razor
         if diffstam > 30
            overhead 'Need stamina'
        endif
        ```

## diffweight

- `diffweight`

Description: Used to get the difference between you max weight and current weight.

!!! example

    === "General"

        ```razor
        if diffweight > 20
            overhead 'I can lift 20 more stone'
        endif
        ```

## followers

Description: Used to get the current number of followers.

!!! example

    === "General"

        ```razor
        if followers = 0
            overhead 'No one following me!'
        else
            overhead 'I have followers'
        endif
        ```

    === "..used with maxfollowers"

        ```razor
        if followers < maxfollowers
            overhead 'You can have more followers!'
        else
            overhead 'You hit your followers limit'
        endif
        ```

## findbuff

- `findbuff 'name of buff/debuff`

Description: Used to check if a specific buff/debuff is applied to you.

!!! example

    === "Check for magic reflection"

        ```razor
        if findbuff 'magic reflection'
            overhead 'Im set!'
        else
            cast 'magic reflection'
            wft
            target 'self'
        endif
        ```

## hidden

- `hidden`

Description: Used to check if you are hidden.

!!! example

    === "Check if hidden"

        ```razor
        if hidden
            overhead 'they cant see me'
        endif
        ```

## hp & maxhp

- `hp`
- `maxhp`
- `hits`
- `maxhits`

Description: Used to get your current or max hit points/health levels.

!!! example

    === "Example 1"

        ```razor
        while hp < 100
            say 'not at 100 yet'
            wait 5000
        endwhile
        ```

    === "Example 2"

        ```razor
        if maxhp = 120
            say 'Full hp!'
        endif
        ```

## lhandempty

- `lhandempty`

Description: Used to check if your left hand is empty

!!! example

    === "General"

        ```razor
        if lhandempty
            hotkey 'empty right hand!'
        endif
        ```

## invuln

- `invuln`
- `invul`
- `blessed`

Description: Used to get your invulnerable status

!!! example

    === "General"

        ```razor
        if invuln
            overhead 'I feel.. so powerful.'
        endif
        ```


## mana & maxmana

- `mana`
- `maxmana`

Description: Used to get your current or max mana levels.

!!! example

    === "General"

        ```razor
        while mana < maxmana
            skill 'meditation'
            wait 11000
        endwhile
        ```

## maxfollowers

Description: Used to get the maximum number of allowed followers.

!!! example

    === "General"

        ```razor
        if followers = maxfollowers
            overhead 'You hit your limit'       
        endif
        ```

    === "..used with followers"

        ```razor
        if followers < maxfollowers
            overhead 'You can have more followers!'
        else
            overhead 'You hit your followers limit'
        endif
        ```

## maxweight

- `maxweight`

Description: Used to get your max allowed weight.

!!! example

    === "General"

        ```razor
        if weight <= maxweight
            say 'I am overweight'
        endif
        ```

## mounted

- `mounted`

Description: Used to check if you are currently on a mount

!!! example

    === "General"

        ```razor
        if mounted
            say 'mounted'
        else
            say 'not mounted'
        endif
        ```

## name

- `name`

Description: Used to get your name of the currently logged in character

!!! example

    === "General"

        ```razor
        if name = 'Quick'
            overhead 'thats me!'
        endif
        ```

## paralyzed

- `paralyzed`

Description: Used to check if you are currently paralyzed.

!!! example

    === "General"

        ```razor
        if paralyzed
            overhead 'I cannot move!'
        endif
        ```

## poisoned

- `poisoned`

Description: Used to check if you are currently poisoned.

!!! example

    === "General"

        ```razor
        if poisoned
            hotkey 'drink cure'
        endif
        ```

## position

- `position (x, y)`
- `position (x, y, z)`

Description: Used to check if your current position matches the provided.

!!! example

    === "General"

        ```razor
        if position 2729 2133
            overhead 'You are currently in front of the Bucs Den teleporter'
        elseif position 2728 2133 5
            overhead 'You are standing on the Bucs Den teleporter'
        endif
        ```

## rhandempty

- `rhandempty`

Description: Used to check if your right hand is empty

!!! example

    === "General"

        ```razor
        if rhandempty
            hotkey 'empty right hand!'
        endif
        ```

## skill

- `skill ('name')`

Description: Used to get the current skill level for a given skill.

!!! tip "Supported skill names"
    Razor used to rely on a static list of names but now reads from your client's skills.mul file for the names of the skill.

!!! example

    === "General"

        ```razor
        if skill 'magery' < 62.5
            cast 'invisibility'
            waitfortarget
            target 'self'
        endif
        ```

## stam & maxstam

- `stam`
- `maxstam`

Description: Used to get your current stamina or max stamina.

!!! example

    === "General (stam)"

        ```razor
        if stam < 30
            say 'I need to rest'
        endif
        ```

    === "General (maxstam)"       

        ```razor
        if maxstam = 120
            say 'I feel so powerful!'
        endif
        ```

## str, dex & int

- `str`
- `dex`
- `int`

Description: Used to get your current strength, dexterity and intelligence.

!!! example

    === "Strength"

        ```razor
        if str = 100
            say 'Strength does not come from physical capacity. It comes from an indomitable will.'
        endif
        ```

    === "Dexterity"

        ```razor
        if dex = 100
            say 'Dexterity comes by experience and practice.'
        endif
        ```

    === "Intelligence"

        ```razor
        if str = 100
            say 'The true sign of intelligence is not knowledge but imagination.'
        endif
        ```

## warmode

- `warmode`

Description: Used to get your current combat/war status

!!! example

    === "General"

        ```razor
        if warmode
            overhead 'Lets fight'
        else
            overhead 'Peace to you'
        endif
        ```

## weight

- `weight`

Description: Used to get your current weight.

!!! example

    === "General"

        ```razor
        if weight = 300
            say 'I feel heavy'
        endif
        ```

# Timer Expressions

## timer

- `timer ('name')`

Description: Used to check how much time is left in an existing timer

!!! example

    === "General"

        ```razor
        // Create a new timer
        if not timerexists 'sample'
            createtimer 'sample'
        endif

        // Reset every 10 seconds
        if timer 'sample' > 10000
            settimer 'sample' 0
        endif
        ```

    === "Alert"

        ```razor
        # Create a new timer
        createtimer 'alert'

        while not dead            
    
            // Reset every 10 seconds
            if timer 'alert' > 10000
                overhead 'ALERT TRIGGERED!'
                settimer 'alert' 0
            endif

        endwhile

        
        ```

## timerexists

- `timerexist ('name')`

Description: Used to check if a timer exists

!!! example

    === "General"

        ```razor
        // Create a new timer
        if not timerexists 'sample'
            createtimer 'sample'
        endif

        // Reset every 10 seconds
        if timer 'sample' > 10000
            settimer 'sample' 0
        endif
        ```
