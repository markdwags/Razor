# Variables Overview

Variables (also referred to as aliases) are commonly used values designed to be referenced in any of your scripts.

You can either define your own variables or you can use this list of pre-defined variables.

# Defining Variables

You can create custom variables that are available in your scripts.  This can be done one of two ways.  First, you can create custom variables in the Razor UI by going to the Options tab under the Scripts tab.

![variables](../images/scripts-options.png)

Simply click Add, give it a name and target the item or mobile and that variable is available in your scripts.  In the screenshot above, `regbag` is set to a bag.  To use this variable in a script, you simply reference it.

* Open your reg bag

```vim
dclick 'regbag'
```

You can also set variables in scripts by using the `setvar` command.  To update the `regbag` command, you might do something like this:

* Get target cursor to select item or mobile

```vim
setvar 'regbag'
```

The script will pause until you select a target. After you select a target, it will update and move on to the next line in the script.

# Pre-Defined Variables

Along with defining your own variables, you can use these pre-defined variables in your scripts.

| Variable          | Description                                                |
| ----------------- | ---------------------------------------------------------- |
| backpack          | `Returns:` The serial of your own backpack                 |
| hand              | `Returns:` The serial of the item in either hand           |
| lasttarget / last | `Returns:` The serial of your current last target in Razor |
| lastobject        | `Returns:` The serial of your last used object in Razor    |
| lefthand          | `Returns:` The serial of the item in your left hand        |
| righthand         | `Returns:` The serial of the item in your right hand       |
| self              | `Returns:` The serial of your player                       |
