# Variables

One way to communicate between Unity and Ink projects is through *variables*. The ink story can write relevant data into variables that will be read by the game to display it in some form, or the game can write data into the ink variables to inform the story about interactions that happen on the Unity side.

The main interface is the *Variable Listeners* panel. A variable listener is a component that listens to changes in a variable to read or synchronize variable values between ink and Unity. There are multiple ways to handle variables, each of which can satisfy different needs.

## Match by name

This is the most used and basic way to use variables. It allows a variable to be fully synchronized between Unity and Ink: each time the specified variable is changed in Ink, it's also written in the Unity Atom, and each time it's changed in the atom, it's also written back in Ink.

Since Ink is weakly typed, the Ink Atom uses the @LemuRivolta.InkAtoms.VariableValue type that wraps a generic value of type `object`.

This method could be used for example to synchronize the number of life points of the main character.

## Match by regex and list

This method listens to multiple variables at once, either by specifying a regular expression that matches with the variable name, or by providing an explicit list of variables to listen to.

Since there are multiple variables listened at once, an Atom Event is raised at every change, but no value is synchronized with atoms. Two different events are provided:

- *Variable Change Event* only provides the new value of the variable changed
- *Variable Pair Change Event* provides both the new and the old value

## Match by name (ink list)

This method is equivalent to the Match by name, but it's used for ink lists, and the value is written in a [value list](https://unity-atoms.github.io/unity-atoms/introduction/overview#value-lists) atom. This provides access to the single entries, and events corresponding to the add and removal.