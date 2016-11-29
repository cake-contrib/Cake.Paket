# Maintainers

This section is for maintainers of the project.

## Creating a Release

Assuming the code has been merged into master and all CI builds are fine. To create a release:

* Update the *paket.template* file for the projects *Cake.Paket.Module* and *Cake.Paket.Addin*.
    * Copyright
    * Release Notes
* Update copyright info in *build.cake* (Setup).
* Create a signed git tag.
* Edit the github release notes and include any contributors.
* Create a NuGet package e.g. `./.paket/paket.exe pack output NuGet version 0.0.0`.
* Upload NuGet packages to nuget.org.
* Update the AppVeyor build version and push the change to github.

## Misc

* The [develop](http://cakepaket.readthedocs.io/en/develop/) branch documentation.
