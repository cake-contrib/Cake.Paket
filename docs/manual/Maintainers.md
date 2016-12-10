# Maintainers

This section is for maintainers of the project.

## Creating a Release

Assuming the code has been merged into master and all CI builds are fine. To create a release:

* Update the *paket.template* file for the projects *Cake.Paket.Module* and *Cake.Paket.Addin*
    * copyright
    * releaseNotes
* Update the Source/SolutionInfo.cs
    * AssemblyVersion
    * AssemblyFileVersion
    * AssemblyCopyright
* Create a signed git tag
    * git tag -s v1.0.0 -m 'version 1.0.0' (then enter your passphrase)
* Edit the github release notes and include any contributors.
    * Title = Cake.Paket v1.0.0
    * Body = Release notes from *paket.template*.
* Create the NuGet packages
    * `.\build.ps1 -Target Paket-Pack -ScriptArgs "-buildVersion='1.0.0'"`
* Upload the NuGet packages to nuget.org.
* Generate the website
    * `.\build.ps1 -Target Generate-Docs`
* Update *build.cake*
    * buildVersion
* Create a commit that include [skip ci] and push the changes to github.

## Documentation

You may need to empty your cache and hard reload to see your content changes!

* The content for the site can be found in
    * docs/images
    * docs/manual
* To build the site run
    * `.\build.ps1 -Target Generate-Docs`
* Only changes pushed to the master branch will appear on the online documentation. Thus, for local developoment use
    * `.\packages\tools\docfx.console\tools\docfx.exe .\docs\docfx.json --serve`
    * Then navigate to `http://localhost:8080/`
