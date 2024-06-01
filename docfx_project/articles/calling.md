# Calling code from Ink

Ink provides a built-in mechanism to call functions in the hosted environment, called *external functions*. This method has some limitations, namely that functions are strictly *synchronous*, but offer the advantage that they can return values to ink once executed, and can provide a fallback pure-ink implementation (which allows for an easy testing of a game in the Ink editor).

To overcome this limitation, the usually used approach is to have a convention between the ink writers and the unity implementation to mark lines that have to be interpreted as commands. Unity will stop execution for as long as needed to complete the action without advancing.

Another tool used to send data and commands are tags. They can both be used to describe a line (e.g.: a change in expression of the main character) but also to start an action (e.g.: the camera panning to a certain position).

All these three methods are supported in Ink Atom Story in similar ways. In all these situations, one must implement a class by extending a base class and extend all its parts.