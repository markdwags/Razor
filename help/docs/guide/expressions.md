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

Example(s):

```php
if count 'garlic' < 5
    say 'getting low on garlic'
endif
```

## findbuff

- `findbuff 'name of buff/debuff`

Description: Used to check if a specific buff/debuff is applied to you.

Example(s):

```php
if findtype 'magic reflection'
    overhead 'Im set!'
else
    cast 'magic reflection'
endif
```

## findtype

- `findtype ('name of item') [inrangecheck (true/false)]` OR `findtype (graphicID) [inrangecheck (true/false)]`

Description: Used to check if a specific item name of graphic ID exists.  Range check, if true, will check within 2 tiles.

Example(s):

```php
// Look for a saw
if findtype 'saw'
    say 'found'
endif
```

## hidden

- `hidden`

Description: Used to check if you are hidden.

Example(s):

```php
// Look for a saw
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

Example(s):

```php
while hp < 100
    say 'not at 100 yet'
    wait 5000
endwhile
```

```php
if maxhp = 120
    say 'Full hp!'
endif
```

## insysmsg

- `insysmsg ('message to look for')`
- `insysmessage ('message to look for')`

Description: Used to check if certain text appears within the system message log.

Example(s):

```php
// Check if system message has a specific message
if insysmsg 'too far away'
    overhead 'You are too far away'
endif
```

## lhandempty

- `lhandempty`

Description: Used to check if your left hand is empty

Example(s):

```php
if lhandempty
    hotkey 'empty right hand!'
endif
```

## mana & maxmana

- `mana`
- `maxmana`

Description: Used to get your current or max mana levels.

Example(s):

```php
while mana < maxmana
    skill 'meditation'
    wait 11000
endwhile
```

## mounted

- `mounted`

Description: Used to check if you are currently on a mount

Example(s):

```php
if mounted
    say 'mounted'
else
    say 'not mounted'
endif
```

## poisoned

- `poisoned`

Description: Used to check if you are currently poisoned.

Example(s):

```php
if poisoned
    hotkey 'drink cure'
endif
```

## rhandempty

- `rhandempty`

Description: Used to check if your right hand is empty

Example(s):

```php
if rhandempty
    hotkey 'empty right hand!'
endif
```

## skill

- `skill ('name')`

Description: Used to get the current skill level for a given skill.

Example(s):

```php
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

Example(s):

```php
if stam < 30
say 'I need to rest'
endif
```

```php
if maxstam = 120
say 'I feel so powerful!'
endif
```

## str, dex & int

- `str`
- `dex`
- `int`

Description: Used to get your current strength, dexterity and intelligence.

Example(s):

```php
if str = 100
    say 'I am a strong person'
endif
```

## weight

- `weight`

Description: Used to get your current weight.

Example(s):

```php
if weight = 300
    say 'I feel heavy'
endif
```
