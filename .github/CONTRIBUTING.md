# Contributing to Cake.Paket

All types of contributions are welcome!

# Getting Started

Assuming you have a GitHub account and have forked the repository, you'll need to run `.\.paket\paket.exe restore` to get all the dependencies. Checkout out [paket](https://fsprojects.github.io/Paket/) for more details. Optionally, if you want to run the build scripts

in PowerShell run
```
$ .\build.ps1 -Configuration Debug # Local builds
$ .\build.ps1 -Target AppVeyor -Configuration Debug # AppVeyor builds
```

and in bash run

```
$ ./build.sh --configuration Debug # Local builds
$ ./build.sh --target TravisCI --configuration Debug # TravisCI builds
```

Please see the *build.ps1* and/or *build.sh* scripts for more detail.

## Tooling

Optionally, you can install the Visual Studio plugins

1. Resharper
2. EditorConfig
3. CodeMaid
4. Productivity Power Tools
