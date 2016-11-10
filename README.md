<p align="center">
  <a href="https://github.com/larzw/Cake.Paket"><img src="https://raw.githubusercontent.com/larzw/Cake.Paket/master/Documentation/Images/CakePaketLogo.png" /></a>
</p>

# Under Construction!

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

# Quick Start

Here is an example Cake script showing Cake.Paket and Cake.Paket.Module in action.

```cake
//=================================================================================================
// Use paket to manage dependencies.
// If you don't specify a group in the uri e.g.
//   #addin paket:?package=Cake.Figlet&group=build
//     or
//   #tool paket:?package=xunit.runner.console&group=build/helper/tool
// Then paket will look in the default tools and/or addins directory.
//=================================================================================================
#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=coveralls.net
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#addin paket:?package=Cake.Figlet&group=build
#addin paket:?package=Cake.Coveralls
#addin paket:?package=Cake.Paket

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var buildVersion = Argument("buildVersion", "0.0.0-alpha0");

var cakePaket = "./Source/Cake.Paket.sln";
var cakePaketAddin = "./Source/Cake.Paket.Addin/bin/" + configuration;
var cakePaketModule = "./Source/Cake.Paket.Module/bin/" + configuration;
var cakePaketUnitTests = "./Source/Cake.Paket.UnitTests/bin/" + configuration + "/*.UnitTests.dll";

var reports = "./Reports";
var coverage = reports + "/coverage.xml";
var resharperSettings = "./Source/Cake.Paket.sln.DotSettings";
var inspectCode = reports + "/inspectCode.xml";
var dupFinder = reports + "/dupFinder.xml";

var nuGet = "./NuGet";

Setup(tool =>
{
    Information(Figlet("Cake.Paket"));
    Information("\t\tMIT License");
    Information("\tCopyright (c) 2016 Larz White");
});

Task("Clean").Does(() =>
{
    CleanDirectories(new[] {cakePaketAddin, cakePaketModule, reports, nuGet});
});

Task("Build").IsDependentOn("Clean").Does(() =>
{
    if(IsRunningOnWindows())
    {
        MSBuild(cakePaket, settings => settings.SetConfiguration(configuration));
    }
    else
    {
      XBuild(cakePaket, settings => settings.SetConfiguration(configuration));
    }

});

Task("Run-Unit-Tests").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(reports);

    if(HasEnvironmentVariable("COVERALLS_REPO_TOKEN") && IsRunningOnWindows())
    {
        OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath(coverage), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));
        CoverallsNet(coverage, CoverallsNetReportType.OpenCover, new CoverallsNetSettings{RepoToken = EnvironmentVariable("COVERALLS_REPO_TOKEN")});
    }
    else
    {
        XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false});
        Warning("\nNot pushing OpenCover results to Coveralls because the build is not on windows and/or the environment variable (repo token) does not exits.\n");
    }
});

Task("Run-InspectCode").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        EnsureDirectoryExists(reports);

        InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCode });
    }
});

Task("Run-DupFinder").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        EnsureDirectoryExists(reports);

        DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinder });
    }
});

Task("Paket-Pack").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(nuGet);

    if(HasEnvironmentVariable("APPVEYOR_BUILD_VERSION") && IsRunningOnWindows())
    {
        buildVersion = EnvironmentVariable("APPVEYOR_BUILD_VERSION");
    }
    else
    {
        Warning("\nUsing default versioning for nupkg because the build is not on windows and/or the environment variable does not exits.\n");
    }

    Information("\nThe nupkg version is: " + buildVersion + "\n");

    //=======================================================================
    // Use Cake.Paket addin to run PaketPack (which creates a NuGet package).
    //=======================================================================
    PaketPack(nuGet, new PaketPackSettings { Version = buildVersion });
});

Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Paket-Pack");

RunTarget(target);

```

In your cake bootstrapper script you'll need to resotre the packages via paket. You also need to set an enviornment variable or pass an argument into Cake.exe (which tells cake where the *.paket* directory is) if your cake script is not in the same directory as the .paket directory.

```powershell
# restore packages
.\.paket\paket.exe restore

# Set enviornment variable
$ENV:PAKET = ".\.paket"

# Run Cake.exe normally
Cake.exe ....
```

OR

```powershell
# restore packages
.\.paket\paket.exe restore

# Run Cake.exe normally (but add the --paket argument)
Cake.exe --paket ".\.paket" ...
```

OR

```powershell
# restore packages
.\.paket\paket.exe restore

# Run Cake.exe normally (if the .paket directory is in the same place as build.cake)
Cake.exe ...
```

See the [Quick Start](http://cakepaket.readthedocs.io/en/latest/QuickStart/) guide.

# Example Project

[Cake.Paket.Example](https://github.com/larzw/Cake.Paket.Example) is an example project which uses Paket with Cake.

# Documentation

See the [Documentation](http://cakepaket.readthedocs.io/en/latest/) for additional help.

# Contributing

All types of contributions are welcome! Please see the [Contributing](https://github.com/larzw/Cake.Paket/blob/master/.github/CONTRIBUTING.md) guidlines.

# Questions

Feel free to open an [issue](https://github.com/larzw/Cake.Paket/issues) or **@larzw** me via [Gitter](https://gitter.im/cake-contrib/Lobby)
