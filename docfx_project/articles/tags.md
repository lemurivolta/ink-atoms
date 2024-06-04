---
uid: tags
---
# Tags

A tag can be parsed by a tag processor. An example could be:

```ink
I'm really shocked! #play-sound:drama.mp3:0.8
```

The tag is built by these parts:

- `#`: is the start of the tag (ink standard)
- `play-sound`: the text before the first `:` (if present) is the tag processor name
- `drama.mp3`: is the first argument
- `0.8`: is the last argument

In practice, a tag is split using `:`, interpreting the first part as the tag name, and the other parts as the arguments.
