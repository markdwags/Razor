# Razor Scripting Engine Guide

## Introduction

The Razor Scripting Engine (referred to as RSE) is a "command based scripting language" designed to make it easier to create, edit and share scripts without expanding Razor into the realm of automation that you can find in assistants such as Steam, Razor Enhanced and EasyUO.

To be clear, this **is not** an implementation of the Steam scripting language into Razor. While the command style is similar and some commands may be the same across both assistants, the scripting engine was built out using the existing Razor macro functionality that have been availab

For example, here is a Razor macro that casts blade spirit, waits for the target and casts a relative location based on your position.

```
Assistant.Macros.MacroCastSpellAction|33
Assistant.Macros.WaitForTargetAction|30
Assistant.Macros.TargetRelLocAction|3|
```

With a Razor script, it becomes:

```
cast 'blade spirits'
waitfortarget
targetrelloc 3 1
```

## Commands and Parameters

The commands issued using the RSE are similar to a commands entered into a command prompt or shell.  Each line has a starting command, and a set of parameters.  Some of those parameters are required, some are optional. 

`command (required) [optional]`

All parameters are shown inside of parenthesis or brackets.  Parameters within parenthesis are **required** while those in brackets are optional or will default to specific value if not provided.

### String Parameters

If you want to pass several words as a single parameter you must wrap them using `'` or `"`. For example `"hello goodbye"` is one parameter, while `hello goodbye` is two.

## Loops and Conditions

RSE supports the following loops and conditions:

### if/elseif/else/endif

```
if (statement)

elseif (statement)

else

endif
```

### while/endwhile

```
while (statement)

endwhile
```

### for/endfor
```
for (value)

endfor
```

## Keywords

- break
- continue
- stop
- replay
- not (statement)
- (statement) and (statement)
- (statement) or (statement)

## Expressions

When using the `if` or '`while` conditions, you can access the following expressions to check against.

### stam/maxstam

Used to get your current stamina or max stamina.

```
if (stam < 30)
say 'I need to rest'
endif
```

```
if (maxstam = 120)
say 'I feel so powerful!'
endif
```


### hp/maxhp/hits/maxhits
### mana/maxmana
### str/dex/int
### skill ('name')

## Comments

You can use `//` at the beginning of the line to have the RSE ignore that specific line.

```
// Hello world example
say 'hello!'
//say 'world'
```

## Layers

Any RSE parameter (such as the one for `dropitem`) that required the `layername` parameter can use any of the following keywords (case insensitive):

- RightHand
- LeftHand
- Shoes
- Pants
- Shirt
- Head
- Gloves
- Ring
- Talisman
- Neck
- Hair
- Waist
- InnerTorso
- Bracelet
- FacialHair
- MiddleTorso
- Earrings
- Arms
- Cloak
- Backpack
- OuterTorso
- OuterLegs
- InnerLegs

## Commands

### cast
### dress
### undress
### dressconfig
### target
### targettype
### targetrelloc
### waitfortarget
### dclicktype/usetype
### dclick/useobject
### dclickvar
### drop
### droprelloc
### lift
### lifttype
### waitforgump
### gumpreply
### gumpclose
### menu
### menuresponse
### waitformenu
### promptresponse
### waitforprompt
### hotkey
### say
### msg
### overhead
### sysmsg
### wait
### pause
### waitforstat
### setability
### setlasttarget
### lasttarget
### setvar
### skill
### walk

## Credits

- Jaedan for which the [parser/lexer](https://github.com/jaedan/steam-engine) that drives this scripting engine is based on.