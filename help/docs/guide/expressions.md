# Expressions Overview

When using the `if` or `while` conditions, you can access the following expressions to check against.

The following operators are supported:

- `=`
- `==`
- `!=`
- `<`
- `<=`
- `>`
- `>=`

## count/counter

- `count ('name of counted item')`
- `counter ('name of counted item')`

Description: Used to get the current number of a specific counted item in Razor

!!! note
    You must have a counter setup in `Display->Counters` before using this expression.

!!! example

    === "Counting garlic"

        ```vim
        if count 'garlic' < 5
            say 'getting low on garlic'
        endif
        ```

## findbuff

- `findbuff 'name of buff/debuff`

Description: Used to check if a specific buff/debuff is applied to you.

!!! example

    === "Check for magic reflection"

        ```vim
        if findbuff 'magic reflection'
            overhead 'Im set!'
        else
            cast 'magic reflection'
        endif
        ```

## findtype

- `findtype ('name of item') [inrangecheck (true/false)]` OR `findtype (graphicID) [inrangecheck (true/false)]`

Description: Used to check if a specific item name of graphic ID exists.  Range check, if true, will check within 2 tiles.

!!! example

    === "Find a saw"

        ```vim        
        if findtype 'saw'
            say 'found'
        endif
        ```

## hidden

- `hidden`

Description: Used to check if you are hidden.

!!! example

    === "Check if hidden"

        ```vim
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

        ```vim
        while hp < 100
            say 'not at 100 yet'
            wait 5000
        endwhile
        ```

    === "Example 2"

        ```vim
        if maxhp = 120
            say 'Full hp!'
        endif
        ```

## insysmsg

- `insysmsg ('message to look for')`
- `insysmessage ('message to look for')`

Description: Used to check if certain text appears within the system message log.

!!! example

    === "Check for message"

        ```vim
        if insysmsg 'too far away'
            overhead 'You are too far away'
        endif
        ```

## lhandempty

- `lhandempty`

Description: Used to check if your left hand is empty

!!! example

    === "General"

        ```vim
        if lhandempty
            hotkey 'empty right hand!'
        endif
        ```

## mana & maxmana

- `mana`
- `maxmana`

Description: Used to get your current or max mana levels.

!!! example

    === "General"

        ```vim
        while mana < maxmana
            skill 'meditation'
            wait 11000
        endwhile
        ```

## mounted

- `mounted`

Description: Used to check if you are currently on a mount

!!! example

    === "General"

        ```vim
        if mounted
            say 'mounted'
        else
            say 'not mounted'
        endif
        ```

## poisoned

- `poisoned`

Description: Used to check if you are currently poisoned.

!!! example

    === "General"

        ```vim
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

        ```vim
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

        ```vim
        if rhandempty
            hotkey 'empty right hand!'
        endif
        ```

## skill

- `skill ('name')`

Description: Used to get the current skill level for a given skill.

!!! example

    === "General"

        ```vim
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

    === "General"

        ```vim
        if stam < 30
        say 'I need to rest'
        endif
        ```

        ```vim
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

    === "General"

        ```vim
        if str = 100
            say 'I am a strong person'
        endif
        ```

## weight

- `weight`

Description: Used to get your current weight.

!!! example

    === "General"

        ```vim
        if weight = 300
            say 'I feel heavy'
        endif
        ```
