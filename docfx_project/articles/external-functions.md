---
uid: external-functions
---
# External Functions

There are three classes that can be implemented to create an external function, according to how it works.

- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.ActionExternalFunctionProcessor for *synchronous* external functions that *do not need to return a value*,
- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.FuncExternalFunctionProcessor`1 for *synchronous* external functions that *need to return a value*, and
- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.CoroutineExternalFunctionProcessor for *asynchronous* external functions that can optionally return a value (see limitations below).

In all cases, the steps are:

- Extend the base class:

```csharp
public class Test1PlaySoundExternalFunction : CoroutineExternalFunction {
  // ...
```

- Call the constructor with the name of the external function you want to create:

```csharp
    public Test1PlaySoundExternalFunction() : base("playSound") { }
```

- Override the `Process` method. According to the kind of base class, the method has a different signature:
  - `void ActionExternalFunctionProcessor.`@'LemuRivolta.InkAtoms.ExternalFunctionProcessors.ActionExternalFunctionProcessor.Process(LemuRivolta.InkAtoms.ExternalFunctionProcessors.ExternalFunctionProcessorContext)' 
  - `T FuncExternalFunctionProcessor.`@'LemuRivolta.InkAtoms.ExternalFunctionProcessors.FuncExternalFunctionProcessor`1.Process(LemuRivolta.InkAtoms.ExternalFunctionProcessors.ExternalFunctionProcessorContext)'
  - `IEnumerator CoroutineExternalFunctionProcessor`@'LemuRivolta.InkAtoms.ExternalFunctionProcessors.CoroutineExternalFunctionProcessor.Process(LemuRivolta.InkAtoms.ExternalFunctionProcessors.ExternalFunctionProcessorContextWithResult)'
  
The @LemuRivolta.InkAtoms.ExternalFunctionProcessors.ExternalFunctionProcessorContext provides access to the arguments. The asynchronous version (@LemuRivolta.InkAtoms.ExternalFunctionProcessors.ExternalFunctionProcessorContextWithResult) also allows to set the return value (*before* yielding the first time), and works exactly like a coroutine, allowing to `yield` instructions and wait for the execution. In any case, this method is called when the external function is called from ink.
