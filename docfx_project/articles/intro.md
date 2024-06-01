# Overview

Ink projects are a set of files containing the text of a game, enriched with tags and instructions to provide the necessary interactivity. Ink gives its best when it is in complete control of both the textual contents and the high-level flow of the game.

Unity Atoms is a set of tools that revolves around the use of *atoms*, which are scriptable objects that represent atomic elements (data and events), accessible from the whole project. They provide a functional replacement for singleton instances and managers.

Ink Atoms builds on these elements, providing a structure for interacting inside Unity with Ink stories through Atoms. Its main advantages are:

- The ones that [Atoms already provide on its own](https://github.com/unity-atoms/unity-atoms?tab=readme-ov-file#motivation): **modularity, editability, better debugging**;
- **Asynchronous execution of commands** (Ink's external functions are synchronous only);
- **Less boilerplate**: it implements the code necessary to get Ink integrated inside of Unity, providing an opinionated yet flexible way of interacting with its systems;
- Provides a default structure for some features that are typically needed, like specially formatted command lines or tags to execute **custom code**.

## Where to go from here?

You can read the @installation and @initial chapters to set up your environment.

After that, the @base chapter provides the starting information to use Ink Atoms in your projects.