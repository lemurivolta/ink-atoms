VAR var1 = 0
VAR var2 = 0
VAR var3 = 0
VAR duration = 0.0
VAR boolVariable = true
VAR floatVariable = 2.3
VAR stringVariable = "string"

LIST testList = first, second, third

-> beginning

= beginning
-> changeVars ->
-> changeVars23 ->
-> testLists ->
-> testChangeList ->
-> playSoundsCommand ->
-> playSoundsTag ->
-> playSoundsFunc ->
-> commandWithChoices ->
-> waitFunc ->
-> makeAChoice ->

-> DONE


= changeVars

Changing var1

~ var1 = 88

Changing boolVariable

~ boolVariable = false

Changing floatVariable

~ floatVariable = 4.6

Changing floatVariable to an int

~ floatVariable = 2

Changing stringVariable

~ stringVariable = "other string"

->->


= testChangeList

Adding a list entry via code.

~ addListEntry()

->->




= changeVars23

Changing var2

~ var2 = 99

Changing var3

~ var3 = 99

->->



= testLists

Adding an element to the list.

~ testList += first

Adding another element to the list.

~ testList += second

Removing an element from the list.

~ testList -= first

->->






= playSoundsCommand

Playing a sound (command).

~ duration = 2
>>> playSound soundName:"test sound"

The sound lasted {duration} seconds.

->->



= playSoundsTag

Playing a sound (tag).

This was the tag line. #play-sound:test sound

->->



= playSoundsFunc

Playing a sound (func).

~ duration = playSound("test sound")
>>> 
~ duration = playSound("test sound")

The sound lasted {duration} seconds.

->->


= commandWithChoices

Now I'll wait a bit and pick a random choice

>>> randomChoice
+ First choice
+ Second choice
+ Third choice
-

->->



= makeAChoice

Make a choice:

+ first
+ second
+ third
-

->->



= waitFunc

Now I'll wait an number of seconds expressed via string.

~ wait("1")

Now I'll wait 1 second.

~ wait(1)

Now I'll wait 0.5 seconds.

~ wait(0.5)

->->


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

EXTERNAL addListEntry()

=== function addListEntry() ===
~ testList += third