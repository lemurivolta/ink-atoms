VAR var1 = 0
VAR var2 = 0
VAR var3 = 0
VAR duration = 0

-> beginning

= beginning

Make a choice:

+ first
+ second
+ third
-

good, you made a choice.

Playing a sound (tag).

This was the tag line. #play-sound:test sound

Playing a sound (func).

~ duration = playSound("test sound")

The sound lasted {duration} seconds.

Playing a sound (command).

~ duration = 2
@playSound soundName:"test sound"

The sound lasted {duration} seconds.

~ var1 = var1 + 1
~ var2 = var2 + 1
~ var3 = var3 + 1

Anyway.

~ var1 = var1 + 1
~ var2 = var2 + 1
~ var3 = var3 + 1

-> END

EXTERNAL playSound(soundName)