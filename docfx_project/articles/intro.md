# Overview

[Ink](https://www.inklestudios.com/ink/) projects are a set of files containing **the text of a game, enriched with tags and instructions** to provide the necessary interactivity. Ink is a language aimed at writers and narrative designers, providing a classic writing interface while also offering flexible and powerful options to handle various forms of branching and status tracking. Ink is also able to control the whole game flow at a high level, deferring to Unity where needed.

[Unity Atoms](https://unity-atoms.github.io/unity-atoms/) is a set of tools that is based on **scriptable objects to represent atomic elements (data and events)**, accessible from the whole project. They provide the base of an architecture aimed at programmers which is a modular, editable and testable replacement for singleton instances, managers, enumerations and much more.

# Why Ink Atoms?

Ink Atoms builds on the power of Ink and Unity Atoms, providing a structure for interacting inside Unity with Ink stories through Atoms. Its main advantages are:

- The ones that [Atoms already provide on its own](https://github.com/unity-atoms/unity-atoms?tab=readme-ov-file#motivation): **modularity, editability, better debugging**;
- **Asynchronous execution of commands** (Ink's external functions are synchronous only);
- **Less boilerplate**: it implements the code necessary to get Ink integrated inside of Unity, providing an opinionated yet flexible way of interacting with its systems;
- Provides a default structure for some features that are typically needed, like specially formatted command lines or tags to execute **custom code**.

# Where to go from here?

You can read the @installation and @initial chapters to set up your environment.

After that, the @base chapter provides the starting information to use Ink Atoms in your projects.