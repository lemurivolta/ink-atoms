---
uid: installation
---

# Installation

There are two ways to install the package.

1. The package is available on the [openupm registry](https://openupm.com) in its latest stable version. You can install it via [openupm-cli](https://github.com/openupm/openupm-cli):

```sh
openupm add it.lemurivolta.ink-atoms
```

2. You can also install via git url by adding these entries in your **manifest.json**; this will always install a bleeding edge version:

```json
{
  "com.inkle.ink-unity-integration": "https://github.com/inkle/ink-unity-integration.git#upm",
  "it.lemurivolta.ink-atoms": "https://github.com/lemurivolta/ink-atoms#upm"
}
```

Once you have installed the package, you can move to @initial to create the necessary objects to work with this package.