# Command Line Parser

A command line parser allows the parsing of lines starting with a specific marker (`@` by default) and to execute code when the line is read. The format of these lines are something like:

```
@playSound soundName:"test sound" volume:3
```

Where:

- `@` is the prefix, marking the line as a command
- `playSound` is the name of the command
- `soundName` and `volume` are command's arguments
- `3` is the value of the argument volume; if the argument contains spaces, it
  can be wrapped between quotes, like
  in `"test sound"`

To implement a command line parser the flow is similar to the one of external functions. The differences are:

- the base class is `CommandLineParser`
- the method to implement is `Process`

The argument to Process (a `CommandLineParserContext`) contains more functionalities than the function parser. It allows access to the arguments by name. Moreover, since commands can be attached to choices immediately following it, it also allows to specify whether the ink file should just continue or pick a specific choice once the command is completed.
