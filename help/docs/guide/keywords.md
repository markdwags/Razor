# Keywords Overview

## and

Syntax: `(statement) and (statement)`

Description: The `and` keyword links statements together and if all statements are `true` the condition is met.

!!! example

    === "General"

        ```razor
        if insysmsg 'hello' and insysmsg 'bye'
            say 'found'
        endif
        ```

## as

Syntax: `(statement) as (variable)`

Description: The `as` keyword works in conjunction with the `findtype` expression.

!!! example

    === "General"

        ```razor
        if findtype 'dagger' as 'mydagger'
            overhead 'found dagger'
            dclick 'mydagger'
        endif
        ```

## break

Syntax: `break`

Description: The `break` keyword terminate the closest enclosing loop such as a `for` loop.

!!! example

    === "General"

        ```razor
        for 100
            if stam = 100
                break
            else
                say 'loop again'
            endif
        endfor
        ```

## continue

Syntax: `continue`

Description: The `continue` keyword passes control to the next iteration of the enclosing `for` or `while` loop which the keyword appears.

!!! example

    === "General"

        ```razor
        while stam < 100

            if stam = 50
               say 'At 50!'
               // Skip saying 'Not at 50'
               continue
            endif

            say 'Not at 50'

        endwhile
        ```

## in

Syntax: `(word) in (variable string)`

Description: The `in` keyword works in conjunction with the `getlabel` command.

!!! example

    === "General"

        ```razor hl_lines="4"
        
        if findtype '217' as 'a_dog'
            getlabel 'a_dog' 'dog_label'

            if 'Fido' in 'dog_label'
                overhead 'found mydog!' 5
                overhead 'dog_label' 67
            endif    
        endif
        ```


## loop/replay

Syntax: `loop` OR `replay`

Description: The `loop` or `replay` keyword will restart the currently run script back to the beginning indefinitely.

!!! example

    === "General"

        ```razor
        if mana = 100
            say 'all done!'
            stop
        else
            say 'still meditating'
        endif

        wait 1000

        loop
        ```

## not

Syntax: `not (statement)`

Description: They `not` keyword returns the opposite of the statement result.

!!! example

    === "General"

        ```razor
        if not 'dead'
            say 'I live!'
        endif

        if 'dead'
            say 'I died!'
        endif
        ```

        ```razor
        while not hp = 100
            overhead 'My HP isnt full'
        endwhile
        ```

## or

Syntax: `(statement) or (statement)`

Description: The `or` keyword links statements together and if one statements is `true` the condition is met.

!!! example

    === "General"

        ```razor
        if insysmsg 'hello' or insysmsg 'bye'
            say 'found'
        endif
        ```

## stop

Syntax: `stop`

Description: The `stop` keyword will stop the execution of the current script.

!!! example

    === "General"

        ```razor
        if stam = 100
            say 'all done!'
            stop
        else
            say 'still waiting'
        endif

        wait 1000

        loop
        ```
