<p align="center">
  <a href="https://github.com/larzw/Cake.Paket"><img src="https://raw.githubusercontent.com/larzw/Cake.Paket/master/docs/Images/CakePaketLogo.png" /></a>
</p>

# Cake.Paket

Adds [Paket](https://fsprojects.github.io/Paket/) support to [Cake](http://cakebuild.net/) via a Cake addin and module.

# Master Branch

|Tool|Information|Badge|
|:--|:--|:--|
|AppVeyor|Windows Build|[![AppVeyor branch](https://img.shields.io/appveyor/ci/larzw/Cake-Paket/master.svg)](https://ci.appveyor.com/project/larzw/cake-paket/branch/master)|
|Travis CI|Linux, OS X|[![Travis branch](https://img.shields.io/travis/larzw/Cake.Paket/master.svg)](https://travis-ci.org/larzw/Cake.Paket)|
|Read the Docs|Documentation|[![Documentation Status](https://readthedocs.org/projects/cakepaket/badge/?version=latest)](http://cakepaket.readthedocs.io/en/latest/?badge=latest)|
|Coverall|Code Coverage|[![Coveralls branch](https://img.shields.io/coveralls/larzw/Cake.Paket/master.svg)](https://coveralls.io/github/larzw/Cake.Paket?branch=master)|
|NuGet|Stable *Cake.Paket* addin package|[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.svg)](https://www.nuget.org/packages/Cake.Paket/)
|NuGet|Stable *Cake.Paket.Module* module package|[![NuGet](https://img.shields.io/nuget/v/Cake.Paket.Module.svg)](https://www.nuget.org/packages/Cake.Paket.Module/)
|MyGet|Pre-Release *Cake.Paket* addin package|[![MyGet Pre Release](https://img.shields.io/myget/mathphysics/vpre/Cake.Paket.svg)](https://www.myget.org/feed/mathphysics/package/nuget/Cake.Paket)
|MyGet|Pre-Release *Cake.Paket.Module* module package|[![MyGet Pre Release](https://img.shields.io/myget/mathphysics/vpre/Cake.Paket.Module.svg)](https://www.myget.org/feed/mathphysics/package/nuget/Cake.Paket.Module)
|Gitter|**@larzw** me for questions|[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/cake-contrib/Lobby)|

# Features

Allows the use of paket preprocessor directives and commands

```cake
// tools
#tool paket:?package=NUnit.ConsoleRunner&group=main
#tool paket:?package=JetBrains.ReSharper.CommandLineTools

// addins
#addin paket:?package=Cake.Figlet&group=build/setup
#addin paket:?package=Cake.Paket

...

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

```cake
// tools
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=JetBrains.ReSharper.CommandLineTools

// addins
#addin nuget:?package=Cake.Figlet&version=0.3.1

...

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

* Get the modified [cake bootstrapper script](https://github.com/larzw/Cake.Paket/blob/master/Documentation/CakeBootstrapper.md), then create a *tools* dependency group and add *Cake* to your *paket.dependencies* file
```bash
    group tools
        source https://nuget.org/api/v2
        nuget Cake
```

* Create a *modules* dependency group and add *Cake.Paket.Module* to your *paket.dependencies* file
```bash
    group modules
        source https://nuget.org/api/v2
        nuget Cake.Paket.Module
```

* Now you can use use paket instead of nuget in the preprocessor directive e.g. `#tool paket:?package=Cake.Foo` and/or  `#addin paket:?package=Cake.Bar`.

* If you need to use paket commands such as *pack* and *push* then add `#addin paket:?package=Cake.Paket` to your cake script.

# Example Project

[Cake.Paket.Example](https://github.com/larzw/Cake.Paket.Example) is an example project which uses Paket with Cake. Additionally, the project for the paket addin and module is another good resource, see [build.cake](https://github.com/larzw/Cake.Paket/blob/master/build.cake).

# Documentation

See the [Documentation](http://cakepaket.readthedocs.io/en/latest/) for additional help.

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
