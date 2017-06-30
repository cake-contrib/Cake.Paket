Title: Maintainers
---

This section is for maintainers of the project.

## Creating a Release

* Create the **body** of the release notes (say *releasenotes.md*). Note, don't add this to the repository.
* `git checkout -b release-1.0.0`
* `.\build.ps1 -Target Pre-Release -Configuration Release -ScriptArgs "-releaseNotes='C:\Users\larz\Desktop\releasenotes.md'"`
* `git add .`
* `git commit -m 'Release: v1.0.0'`
* `git push origin head`
* Merge into master
* `git checkout master`
* `git tag -s -a 'v1.0.0' -m 'version 1.0.0'` (enter passphrase)
* `git push origin v1.0.0`
* `.\build.ps1 -Target Release-On-Github -ScriptArgs "-releaseNotes='C:\Users\larz\Desktop\releasenotes.md' -gitHubUserName='username' -gitHubPassword='password'"`
* `.\build.ps1 -Target Paket-Push -ScriptArgs "-nuGetUrl='https://www.nuget.org/api/v2/package' -nuGetApiKey='00000000-0000-0000-0000-000000000000'"`
* Update Cake.Paket.Example with the new version.

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
