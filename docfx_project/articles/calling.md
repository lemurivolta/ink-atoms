# Calling code from Ink

Ink provides a built-in mechanism to call functions in the hosted environment, called **@'external-functions'**. Ink's documentation provides information about [how external functions work](https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#external-functions), but in short the way they are declared inside Ink is something like this:

```ink
/**
 * Declare that Unity will provide a function named
 * "playSound" that takes a single argument and that can
 * be called from Ink.
 */
EXTERNAL playSound(soundName)

/**
 * Default implementation of the playSound function for
 * running the game inside Inky: it just prints that a
 * sound will be played for debugging purposes.
 */
=== function playSound(soundName)
    PLAY SOUND "{soundName}"
```

This method has some limitations, namely that functions are strictly *synchronous*, but offer the advantage that they can return values to Ink once executed, and can provide a fallback pure Ink implementation.

To overcome this limitation, the typical approach is to have a convention between the Ink writers and the Unity programmers to mark lines that have to be interpreted as **@'command-lines'**. Unity will run custom code when these lines are met, which can be asynchronous (e.g.: asking for the camera to pan to a specific position before advancing with the conversation) and even take a choice following the command line (e.g.: a command that chooses a dialogue branch according to the character that is clicked on the Unity scene). Examples of commands could be:

```ink
Main Character: Look there!

@panCamera // in Inky, this line will be printed as-is,
           // but Unity will recognize it as a command
           // and pan the camera

Other Character: I don't see anything... what is it that you've seen?

// In Inky, this line will be printed followed by choices
// to take. In Unity, the command line implementation
// could allow the player to click or tap on one of the
// various elements in the scene and take the
// corresponding choice.
@clickOnElement
+ [Mountain]
  Main Character: That snowy mountain is majestic!
+ [Seaside]
  Main Character: The seaside is stunning in the evening.
+ [Other Character]
  Main Character: Well... you.
  Other Character: Oh... oh!
-
```

Another tool used to send data and commands is **@'tags'**. They can both be used to describe a line (e.g.: a change in expression of the main character) but also to start an action (e.g.: fade to a different soundtrack). Building on the previous example, we could write something like:

```ink
Main Character: Well... you. #changeExpression:blush
Other Character: Oh... oh!   #changeExpression:panic
```

## How to use them

These three methods are supported in Ink Atoms in similar ways:
- Implement a base abstract class, which depends on the type of calling method and whether a return value is needed and/or if it's synchronous or not
- Add an instance of the class and set it in the editor.

You can read the details about the available classes and how to extend Ink Atoms this way in the respective sections.