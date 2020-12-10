# Conditions & Loops Overview

As found in the Razor macro system, you can use `if`, `for` and `while` when writing a script.

## if

```vim
if (statement)

elseif (statement)

else

endif
```

!!! example

    === "General"

        ```vim
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

    === "Using 'not'"

        ```vim
        if not stam = 100
            say 'Stamina is not full'    
        endif
        ```


## for

```vim
for (value)

endfor
```

!!! example

    === "For"

        ```vim        
        for 10
            say 'hello'
            wait 1000
        endfor
        ```

## while

```vim
while statement

endwhile
```

!!! example

    === "While"

        ```vim
        while hits < 100
            say 'I need a heal!'
            wait 1000
        endwhile
        ```

