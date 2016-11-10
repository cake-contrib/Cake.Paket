<p align="center">
  <img src="./Images/CakePaketLogo.png" />
</p>

# Cake.Paket

Adds [Paket](https://fsprojects.github.io/Paket/) support to [Cake](http://cakebuild.net/) via a Cake addin and module.

# Master Branch

The master branch status can be found on [README.md](https://github.com/larzw/Cake.Paket/blob/master/README.md).

# Features

Allows the use of paket preprocessor directives and commands

```cake
// tools
#tool paket:?package=NUnit.ConsoleRunner&group=main
#tool paket:?package=OpenCover

// addins
#addin paket:?package=Cake.Figlet&group=build/setup
#addin paket:?package=Cake.Paket

...

// Creates a nuget package
Task("Paket-Pack").Does(() =>
{
    PaketPack("foo");
});

...
```

instead of using NuGet

```cake
// tools
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=OpenCover

// addins
#addin nuget:?package=Cake.Figlet&version=0.3.1

...

// Creates a nuget pacakge
Task("NuGet-Pack").Does(() =>
{
    NuGetPack("bar.nuspec", new NuGetPackSettings{OutputDirectory = foo});
});

...
```

# Quick Start

1. Get the modified [cake bootstrapper script](CakeBootstrapper.md).
2. Create a *modules* dependency group and add *Cake.Paket.Module* to your *paket.dependency* file
```
    group modules
        nuget Cake.Paket.Module
```
3. If you need to use paket commands such as *pack* and *push* then add `#addin paket:?package=Cake.Paket` to your cake script.

# Example Project

[Cake.Paket.Example](https://github.com/larzw/Cake.Paket.Example) is an example project which uses Paket with Cake. Additionally, the project for the paket addin and module is another good resource, see [build.cake](https://github.com/larzw/Cake.Paket/blob/master/build.cake).

# Contributing

All types of contributions are welcome! Please see the [Contributing](https://github.com/larzw/Cake.Paket/blob/master/.github/CONTRIBUTING.md) guidlines.

# Questions

Feel free to open an [issue](https://github.com/larzw/Cake.Paket/issues) or **@larzw** me via [Gitter](https://gitter.im/cake-contrib/Lobby)
