# External Functions

There are three classes that can be implemented to create an external function, according to how it works.

- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.ActionExternalFunctionProcessor for *synchronous* external functions that *do not need to return a value*,
- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.FuncExternalFunctionProcessor`1 for *synchronous* external functions that *need to return a value*, and finally
- @LemuRivolta.InkAtoms.ExternalFunctionProcessors.CoroutineExternalFunctionProcessor for *asynchronous* external functions that can optionally return a value (see limitations below).

In all cases, the steps are:

- Extend the base class:

```csharp
public class Test1PlaySoundExternalFunction : CoroutineExternalFunction {
  // ...
```

- Call the constructor with the name of the external function:

```csharp
    public Test1PlaySoundExternalFunction() : base("playSound") { }
```

- Override the `Call` method. According to the kind of base class, the method has a different signature:
    - `public abstract void Call(ExternalFunctionContext context);`
    - `public abstract T Call(ExternalFunctionContext context);`
    - `public abstract IEnumerator Call(ExternalFunctionContextWithResult context);`

The ExternalFunctionContext provides access to the arguments. The asynchronous version (`ExternalFunctionContextWithResult`) also allows to set the return value (*before* yielding the first time), and works exactly like a coroutine, allowing to `yield` instructions and wait for the execution. In any case, this method is called when the external function is called from ink.
