# Cake.Paket.Module

In order to use paket instead of nuget in the preprocessor directive e.g. `#tool paket:?package=Cake.Foo` and/or  `#addin paket:?package=Cake.Bar`, you need to include *Cake.Paket.Module* and ensure cake can find the module.

# Basic Usage

Although the following directory structure is not required, we'll use it for our example

```
|-build.cake
|-build.ps1
|-build.sh
|-.paket
|      |-paket.bootsrapper.exe
|-packages
         |-...
```

The packages directroy is defined by [dependency groups](https://fsprojects.github.io/Paket/groups.html) in the [paket.dependencies](https://fsprojects.github.io/Paket/dependencies-file.html) file. Let's assume it looks like

```bash
# tools
group tools
    source https://nuget.org/api/v2
    nuget NUnit.ConsoleRunner=3.4.0
    nuget JetBrains.ReSharper.CommandLineTools
    nuget Cake # Get Cake.exe

# addins
group addins
    source https://nuget.org/api/v2
    nuget Cake.Figlet

# modules
group modules
    source https://nuget.org/api/v2
    nuget Cake.Paket.Module
```

It's also good practice to create a cake bootstrapper script. The script will restore the dependencies and run *Cake.exe*. It can be as simple as

*build.ps1*
```powershell
# Download paket.exe
.\.paket\paket.bootstraper.exe

# Restore the dependencies
.\.paket\paket.exe restore

# Run Cake script and specify the location of the modules directory
.\packages\tools\Cake\Cake.exe --paths_modules=.\packages\modules
```

*build.sh*
```bash
# Download paket.exe
./.paket/paket.bootstraper.exe

# Restore the dependencies
./.paket/paket.exe restore

# Run Cake script and specify the location of the modules directory
exec mono ./packages/tools/Cake/Cake.exe --paths_modules=./packages/modules
```

Our cake script can now use paket

```csharp
// Use paket instead of nuget in the preprocessor directive.
// You need to specify the paket group in the URI.
#tool paket:?package=NUnit.ConsoleRunner&group=tools
#tool paket:?package=JetBrains.ReSharper.CommandLineTools&group=tools
#addin paket:?package=Cake.Figlet&group=addins

...
```

As you can see, we replaced *nuget* with *paket* and specified the group in the URI. Think of this as a replacement for the [paket.references](https://fsprojects.github.io/Paket/references-files.html) file. Furthermore, you can't specify a version in the URI; this is handeled by the *paket.dependencies* file. Also, we specified the module directory location uisng the flag *paths_modules*. See [default configuration values](http://cakebuild.net/docs/fundamentals/default-configuration-values) for more information.

## Removing Groups from the URI

If you create dependency groups that correspond to cakes tool/addin directory path, you can avoid using groups in the URI

*build.ps1*
```powershell
# Download paket.exe
.\.paket\paket.bootstraper.exe

# Restore the dependencies
.\.paket\paket.exe restore

# Run Cake script and specify the location of the modules, tools, and addins directory
.\packages\tools\Cake\Cake.exe --paths_modules=.\packages\modules --paths_tools=.\packages\tools --paths_addins=.\packages\addins
```

*build.sh*
```bash
# Download paket.exe
./.paket/paket.bootstraper.exe

# Restore the dependencies
./.paket/paket.exe restore

# Run Cake script and specify the location of the modules, tools, and addins directory
exec mono ./packages/tools/Cake/Cake.exe --paths_modules=./packages/modules --paths_tools=./packages/tools --paths_addins=./packages/addins
```

and our cake script can be simplified to

```csharp
// Use paket instead of nuget in the preprocessor directive.
// You dont't need to specify the paket group in the URI.
#tool paket:?package=NUnit.ConsoleRunner
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#addin paket:?package=Cake.Figlet

...
```

As you can see, we removed the group from the URI. This is because we grouped the tools and addins into the tools and addins group in the *paket.dependencies* file. Then, we passed this information to *Cake.exe* using the *paths_tools* and *paths_addins* flag. See [default configuration values](http://cakebuild.net/docs/fundamentals/default-configuration-values) for more information.

A quick tip is to configure the tools path as `--paths_tools=.\packages`, then you don't need groups in the tool URI ever. This is because cakes tool locator handles it for you. However, for projects with many dependencies, you may see a performance increase by specifing the group in the tool URI and/or restricting the tool path.

## Complex Dependency Groups

You can group the *paket.dependencies* file however you want! For example,

1. You don't need to use tools, addins, and modules for the group names.
2. You can put the tools/addins in any group you want, they don't even need to be in the same group.

Let's consider the complex dependency file

```bash
# This is the same as using: group main
source https://nuget.org/api/v2
nuget NUnit.ConsoleRunner=3.4.0

group build/setup
    source https://nuget.org/api/v2
    nuget Cake.Figlet

# tools
group tools
    source https://nuget.org/api/v2
    nuget JetBrains.ReSharper.CommandLineTools
    nuget Cake

# addins
group addins
    source https://nuget.org/api/v2
    nuget Cake.Paket

# modules
group modules
    source https://nuget.org/api/v2
    nuget Cake.Paket.Module
```

and the cake script becomes

```csharp
#tool paket:?package=NUnit.ConsoleRunner&group=main
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#addin paket:?package=Cake.Figlet&group=build/setup
#addin paket:?package=Cake.Paket
...
```

where the bootstrapper script from the the last section was used.

A quick reminder is that in paket, if you don't place a dependency in a group, it's place in the main group.

## Don't use the Legacy Preprocessor Notation

You'll see the legacy notation for tools and addins e.g. `#addin Cake.Figlet`. Since this is equivelent to `#addin nuget:?package=Cake.Figlet` you should not use it.
