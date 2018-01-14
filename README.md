# Cake.Paket

[![AppVeyor branch](https://img.shields.io/appveyor/ci/cakecontrib/cake-paket/master.svg)](https://ci.appveyor.com/project/larzw/cake-paket/branch/master)
[![Travis branch](https://img.shields.io/travis/cake-contrib/Cake.Paket/master.svg)](https://travis-ci.org/larzw/Cake.Paket)
[![Codecov branch](https://img.shields.io/codecov/c/github/larzw/Cake.Paket/master.svg)](http://codecov.io/github/larzw/Cake.Paket?branch=master)
[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.svg)](https://www.nuget.org/packages/Cake.Paket/)
[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.Module.svg)](https://www.nuget.org/packages/Cake.Paket.Module/)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/cake-contrib/Lobby)

Adds [Paket](https://fsprojects.github.io/Paket/) support to [Cake](http://cakebuild.net/) via a Cake addin and module.

# Features

Allows the use of paket preprocessor directives and commands

```csharp
// tools
#tool paket:?package=NUnit.ConsoleRunner&group=main
#tool paket:?package=JetBrains.ReSharper.CommandLineTools

// addins
#addin paket:?package=Cake.Figlet&group=build/setup
#addin paket:?package=Cake.Paket

...

// Restores packages
Task("Paket-Restore").Does(() =>
{
    PaketRestore();
});

// Creates a nuget package
Task("Paket-Pack").Does(() =>
{
    PaketPack("./NuGet");
});

// Push a nuget package
Task("Paket-Push").IsDependentOn("Paket-Pack").Does(() =>
{
    PaketPush("./NuGet/foo.nupkg", new PaketPushSettings { ApiKey = "00000000-0000-0000-0000-000000000000" });
});

...
```

instead of using NuGet

```csharp
// tools
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=JetBrains.ReSharper.CommandLineTools

// addins
#addin nuget:?package=Cake.Figlet&version=0.3.1

...

// Restores packages
Task("NuGet-Restore").Does(() =>
{
    NuGetRestore("solution.sln");
});

// Creates a nuget pacakge
Task("NuGet-Pack").Does(() =>
{
    NuGetPack("bar.nuspec", new NuGetPackSettings{OutputDirectory = "./NuGet"});
});

// Push a nuget pacakge
Task("NuGet-Push").IsDependentOn("Paket-Pack").Does(() =>
{
    NuGetPush("./NuGet/foo.nupkg", new NuGetPushSettings{ApiKey = "00000000-0000-0000-0000-000000000000"});
});

...
```

# Quick Start

## Cake.Paket.Module

If you want to use paket instead of nuget in the preprocessor directive e.g. `#tool paket:?package=Cake.Foo` and/or  `#addin paket:?package=Cake.Bar` then you need to use *Cake.Paket.Module*.

* Get the modified [cake bootstrapper script](https://github.com/larzw/Cake.Paket/wiki/Cake-Bootstrapper-Script), then create a *tools* dependency group and add *Cake* to your *paket.dependencies* file
```
    group tools
        source https://nuget.org/api/v2
        nuget Cake
```

* Create a *modules* dependency group and add *Cake.Paket.Module* to your *paket.dependencies* file
```
    group modules
        source https://nuget.org/api/v2
        nuget Cake.Paket.Module
```

* Now you can use paket instead of nuget in the preprocessor directive.

## Cake.Paket (addin)

If you need to use paket commands such as *restore*, *pack*, and *push* then add `#addin paket:?package=Cake.Paket` if your using the *Cake.Paket.Module*, otherwise add ` #addin nuget:?package=Cake.Paket` and `#tool nuget:?package=Paket`.

Note that if you use ` #addin nuget:?package=Cake.Paket` you can use the cake teams default bootstrappers [build.ps1](https://github.com/cake-build/example/blob/master/build.ps1) and/or [build.sh](https://github.com/cake-build/example/blob/master/build.sh).

# Example Project

[Cake.Paket.Example](https://github.com/larzw/Cake.Paket.Example) is an example project which uses Paket with Cake. Additionally, the project for the paket addin and module is another good resource, see [setup.cake](https://github.com/larzw/Cake.Paket/blob/master/setup.cake).

# Documentation

- See the [Documentation](https://github.com/larzw/Cake.Paket/wiki) for additional help.
- Cake's sites contains documentation of the addin at [Cake.Paket](http://cakebuild.net/dsl/paket/).

# Contributing

All types of contributions are welcome!

# Questions

Feel free to open an [issue](https://github.com/larzw/Cake.Paket/issues) or **@larzw** me via [Gitter](https://gitter.im/cake-contrib/Lobby)
