---
uid: LemuRivolta.InkAtoms.VariableObserver
remarks: *content
---
This namespace contains all classes related to @variable-observers.
The most important classes of this namespace are:

- @LemuRivolta.InkAtoms.VariableObserver.VariableObserver - the abstract root class of all observers
    - @LemuRivolta.InkAtoms.VariableObserver.VariableObserverByName`1 - the abstract root class for all observers that keep in sync with a variable by name; there's a limited set of them, which are:
        - @LemuRivolta.InkAtoms.VariableObserver.BoolVariableObserver
        - @LemuRivolta.InkAtoms.VariableObserver.FloatVariableObserver
        - @LemuRivolta.InkAtoms.VariableObserver.IntVariableObserver
        - @LemuRivolta.InkAtoms.VariableObserver.StringVariableObserver and
        - @LemuRivolta.InkAtoms.VariableObserver.ListVariableObserver
    - @LemuRivolta.InkAtoms.VariableObserver.EventsVariableObserver - the abstract root class for all observers that emit events; there are only two of these, which are:
        - @LemuRivolta.InkAtoms.VariableObserver.ListingVariableObserver that tracks an explicit list of variables, and
        - @LemuRivolta.InkAtoms.VariableObserver.RegexVariableObserver that tracks a list of variables expressed through a regular expression

Instances of these classes are typically created via the editor UI.