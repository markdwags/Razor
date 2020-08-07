# Conditions & Loops Overview

As found in the Razor macro system, you can use `if`, `for` and `while` when writing a script.

## if

```php
if (statement)

elseif (statement)

else

endif
```

Example(s):

```php
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

## for

```php
for (value)

endfor
```

Example(s):

```php
// Say hello 10 times
for 10
    say 'hello'
    wait 1000
endfor
```

## while

```php
while statement

endwhile
```

Example(s):

```php
while hits < 100
    say 'I need a heal!'
    wait 1000
endwhile
```