---
uid: base
---

# Base usage

Once the asset has been created, a story can be started at any time. The Ink Atoms Story is not associated to a specific Ink story (which allows, for example, to support multiple languages). To start the story, you can call the method @LemuRivolta.InkAtoms.InkAtomsStory.StartStory(UnityEngine.TextAsset) of the asset:

```csharp
[SerializeField] private InkAtomsStory inkAtomsStory;
[SerializeField] private TextAsset enStory;
[SerializeField] private TextAsset itStory;

// ...

private void StartENStory() {
  inkAtomsStory.StartStory(enStory);
}
```

Ink Atoms uses the concept of *current step*, which represents the current state of the story: the line to display, the choices available, the tags, and so on. This step is saved on the Unity Atom marked as **Story Step Variable** under the *Atoms* foldout. This allows components to interrogate the current step, or get notified when the story advances and the current step changes.

To continue the story you can raise the event specified as **Continue Event** in the asset. The event takes a string as parameter, which is the name of the [flow](https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#multiple-parallel-flows-beta) to continue. If you don't know what flows are or how they work, you can safely pass <code>null</code> here, which means to use the default flow.

Once a choice is presented, it can be chosen by raising the event marked as **Choice Event**. This event contains data both about the choice index and the flow name.