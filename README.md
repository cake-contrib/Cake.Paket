<p align="center">
  <a href="https://github.com/larzw/Cake.Paket"><img src="https://raw.githubusercontent.com/larzw/Cake.Paket/master/docs/images/CakePaketLogo.png" height="100em" /></a>
</p>

# Cake.Paket

Adds [Paket](https://fsprojects.github.io/Paket/) support to [Cake](http://cakebuild.net/) via a Cake addin and module.

# Master Branch

|Tool|Information|Badge|
|:--|:--|:--|
|AppVeyor|Windows Build|[![AppVeyor branch](https://img.shields.io/appveyor/ci/larzw/Cake-Paket/master.svg)](https://ci.appveyor.com/project/larzw/cake-paket/branch/master)|
|Travis CI|Linux, OS X|[![Travis branch](https://img.shields.io/travis/larzw/Cake.Paket/master.svg)](https://travis-ci.org/larzw/Cake.Paket)|
|DocFX|Documentation|[![Documentation Status](https://img.shields.io/badge/DocFX-latest-green.svg)](https://larzw.github.io/Cake.Paket/site)|
|Codecov|Code Coverage|[![Codecov branch](https://img.shields.io/codecov/c/github/larzw/Cake.Paket/master.svg)](http://codecov.io/github/larzw/Cake.Paket?branch=master)|
|NuGet|Stable *Cake.Paket* addin package|[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.svg)](https://www.nuget.org/packages/Cake.Paket/)
|NuGet|Stable *Cake.Paket.Module* module package|[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.Module.svg)](https://www.nuget.org/packages/Cake.Paket.Module/)
|MyGet|Pre-Release *Cake.Paket* addin package|[![MyGet Pre Release](https://img.shields.io/myget/mathphysics/vpre/Cake.Paket.svg)](https://www.myget.org/feed/mathphysics/package/nuget/Cake.Paket)
|MyGet|Pre-Release *Cake.Paket.Module* module package|[![MyGet Pre Release](https://img.shields.io/myget/mathphysics/vpre/Cake.Paket.Module.svg)](https://www.myget.org/feed/mathphysics/package/nuget/Cake.Paket.Module)
|Gitter|**@larzw** me for questions|[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/cake-contrib/Lobby)|

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

* Get the modified [cake bootstrapper script](https://larzw.github.io/Cake.Paket/site/manual/CakeBootstrapper.html), then create a *tools* dependency group and add *Cake* to your *paket.dependencies* file
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

[Cake.Paket.Example](https://github.com/larzw/Cake.Paket.Example) is an example project which uses Paket with Cake. Additionally, the project for the paket addin and module is another good resource, see [build.cake](https://github.com/larzw/Cake.Paket/blob/master/build.cake).

# Documentation

- See the [Documentation](https://larzw.github.io/Cake.Paket/site) for additional help.
- Cake's sites contains documentation of the addin at [Cake.Paket](http://cakebuild.net/dsl/paket/).

# Contributing

All types of contributions are welcome! Please see the [Contributing](https://github.com/larzw/Cake.Paket/blob/master/.github/CONTRIBUTING.md) guidlines.

# Questions

Feel free to open an [issue](https://github.com/larzw/Cake.Paket/issues) or **@larzw** me via [Gitter](https://gitter.im/cake-contrib/Lobby)

# Icon

- Copyright (c) .NET Foundation and Contributors - Cake icon
- Copyright (c) 2015 Alexander Groß, Steffen Forkmann - Paket icon
- Paket icon used with the consent of Paket Team
    - [Asked for permission](https://gitter.im/fsprojects/Paket?at=5824803cdf5ae966453ce2a6)
    - [Permission given](https://gitter.im/fsprojects/Paket?at=58248c0b31c5cbef43dca66e)
