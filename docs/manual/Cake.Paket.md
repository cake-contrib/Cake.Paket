# Cake.Paket

In order to use paket commands such as `paket.exe restore ...`, `paket.exe pack ...` and/or `paket.exe push ...` you need to include `#addin paket:?package=Cake.Paket`, where we assumed you are using the [Cake.Paket.Module](Cake.Paket.Module.md). If your not, then

* Include `#addin nuget:?package=Cake.Paket`.
* Use the cake teams default bootstrappers [build.ps1](https://github.com/cake-build/example/blob/master/build.ps1) and/or [build.sh](https://github.com/cake-build/example/blob/master/build.sh).
* **SKIP** the next secion **Basic Usage**.

# Basic Usage

Assuming you've read the section on [Cake.Paket.Module](Cake.Paket.Module.md), if you want to use paket commands, you'll need to include the addin *Cake.Paket* in the *paket.dependencies* file

```bash
# tools
group tools
    source https://nuget.org/api/v2
    nuget Cake # Get Cake.exe

# addins
group addin
    source https://nuget.org/api/v2
    nuget Cake.Paket

# modules
group modules
    source https://nuget.org/api/v2
    nuget Cake.Paket.Module
```

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

*build.cake*
```csharp
#addin paket:?package=Cake.Paket

...

// Runs paket.exe pack output "./NuGet"
Task("Paket-Pack").Does(() =>
{
    PaketPack("./NuGet");
});

// Runs paket.exe push file "./NuGet/foo.nupkg" apikey ""00000000-0000-0000-0000-000000000000"
Task("Paket-Push").IsDependentOn("Paket-Pack").Does(() =>
{
    PaketPush("./NuGet/foo.nupkg", new PaketPushSettings { ApiKey = "00000000-0000-0000-0000-000000000000" });
});

...
```

## Cake.Paket Configuration

By default cake looks in the *.\\.paket* directory for *paket.bootstrapper.exe*. If you need to re-define the path you have three options

1. Set the ToolPath in the cake script e.g. `new PaketPackSettings{ToolPath=".\.paket"}`
2. Add the flag *--paket* to *Cake.exe* e.g. `Cake.exe --paket=.\.paket`
3. Set the PAKET enviornment variable before calling *Cake.exe* e.g. `ENV:PAKET=.\.paket` or `export PAKET=./.paket`

## API

API documentation for *Cake.Paket* can be found on [Cake's](http://cakebuild.net/) website. Alternativly, see our API documentation by clicking on the API hyperlink in the navigation bar at the top of the page.
