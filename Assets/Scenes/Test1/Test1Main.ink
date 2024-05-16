VAR var1 = 0
VAR var2 = 0
VAR var3 = 0
VAR duration = 0

LIST testList = first, second, third

-> beginning

= beginning

Playing a sound (tag).

This was the tag line. #play-sound:test sound



Playing a sound (command).

~ duration = 2
>>> playSound soundName:"test sound"

The sound lasted {duration} seconds.



Playing a sound (func).

~ duration = playSound("test sound")
>>> 
~ duration = playSound("test sound")

The sound lasted {duration} seconds.


Now I'll wait an invalid number of seconds.

~ wait("1")

Now I'll wait 1 second.

~ wait(1)

Now I'll wait 0.5 seconds.

~ wait(0.5)

Now I'll pick a random choice

>>> randomChoice
+ First choice
+ Second choice
+ Third choice
-

Now I'll set var1 to 2

~ var1 = 2

Adding an element to the list.

~ testList += first

Adding another element to the list.

~ testList += second

Removing an element from the list.

~ testList -= first



Make a choice:

+ first
+ second
+ third
-

good, you made a choice.

~ var1 = var1 + 1
~ var2 = var2 + 1
~ var3 = var3 + 1

Anyway.

~ var1 = var1 + 1
~ var2 = var2 + 1
~ var3 = var3 + 1


-> END

EXTERNAL playSound(soundName)

=== function playSound(soundName) ===
PLAY SOUND {soundName}.

=== function getSoundAssetName(fileName) ===
{ fileName:
- "test sound": 347544__masgame__mouse-click-sounds
- else: xxx
}

EXTERNAL wait(seconds)

=== function wait(seconds) ===
WAIT FOR {seconds} seconds.