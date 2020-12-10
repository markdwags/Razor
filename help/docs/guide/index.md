# Introduction

The **Razor Scripting Engine** is a _"command based scripting language"_ designed to make it easier to read, create, edit and share Razor scripts.

To learn more about this version of Razor, please visit the [home page](http://www.uor-razor.com) or [GitHub repository](https://github.com/markdwags/Razor).

# Design

This is not an implementation of the _Steam_ or _Razor Enhanced_ scripting languages or functionality into Razor. The scripting engine attempts to maintain existing Razor functionality while offering quality of life improvements over the existing macro system.

For example, instead of having to do the following:

- Record macro
- Double click dagger
- Stop recording
- Right-click on dagger
- Select Double Click use by type

You can simply type:

```vim
dclicktype 'dagger'
```

# Scripts vs. Macros

While updates to Razor have made it easier to share macros with friends, one of the core issues with Razor macros is the serialized format they are stored in, which essentially makes it the syntax. While one could edit macros outside of Razor, it was never designed for user input and simply uses `|` as a delimiter to store commands and parameters.

For example, even a basic macro that casts blade spirit, waits for the target and casts a relative location based on your position can be difficult to read to the untrained eye. More so, is not user friendly if one were to type want to type it out.

```vim
Assistant.Macros.MacroCastSpellAction|33
Assistant.Macros.WaitForTargetAction|30
Assistant.Macros.TargetRelLocAction|3|1
```

With a Razor script, it becomes:

```vim
cast 'blade spirits'
waitfortarget
targetrelloc 3 1
```

Another example using a script that uses detect hidden, waits for target, targets self, waits .40 seconds, checks sysmessage, says something and targets the closest mobile.

```vim
Assistant.Macros.UseSkillAction|14
Assistant.Macros.WaitForTargetAction|30
Assistant.Macros.HotKeyAction|1059|
Assistant.Macros.PauseAction|00:00:00.4000000
Assistant.Macros.IfAction|4|0|you can see nothing
Assistant.Macros.ElseAction
Assistant.Macros.SpeechAction|0|38|3|ENU|2|16|52|I Ban Thee
Assistant.Macros.WaitForTargetAction|30
Assistant.Macros.HotKeyAction|2003|
Assistant.Macros.EndIfAction
```

With a Razor script, it becomes:

```vim
skill 'detecthidden'
waitfortarget
target 'self'

wait 400

if insysmsg 'you see nothing'
    overhead 'All clear'
else
    say 'I ban thee'
    waitfortarget
    hotkey 'target closest grey humanoid'
endif
```

As you can see, while maintaining the same functionality, a Razor script is much easier to read and edit.

# Script Editor

Razor Script can be written using any text editor.  The script editor built into Razor offers syntax highlighting and auto-completion.

Here are a list of some of keyboard shortcuts available in the editor:

## General/Misc

Common text editor shortcuts (such as `ctrl-C` and `ctrl-V` for copy/paste are not listed):


- ++ctrl+g++: Goto to a specific line dialog

- ++ctrl+f++: Find/Search dialog
- ++f3++: Find next
- ++ctrl+r++: Search & replace dialog
- ++ctrl+shift+c++: Comment / Uncomment selected code
- ++alt+"Mouse Drag"++: Select multiple lines of text
- ++ctrl+home++: Go to the first line
- ++ctrl+end++: Go to the last line
- ++alt+up++: Move selected lines up
- ++alt+down++: Move selected lines down
- ++shift+up++: Move selected lines up
- ++shift+down++: Move selected lines down

## Bookmarks

- ++ctrl+b++: Bookmark line
- ++ctrl+shift+b++: Remove bookmark
- ++ctrl+b++: Move cursor to next bookmark
- ++ctrl+shift+n++: Move cursor to previous bookmark

## Auto-Complete

- ++ctrl+space++: Open auto-complete menu

## Zoom

- ++ctrl+"Mouse Wheel Up"++: Zoom in
- ++ctrl+"Mouse Wheel Down"++: Zoom out
- ++ctrl+0++: Reset zoom

# About

The **Razor Scripting Engine** was designed and implemented by [Quick](https://github.com/markdwags) and is released under GPLv3.