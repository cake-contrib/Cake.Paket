[CmdletBinding()]
Param(
    [string]$githubUsername,
    [string]$githubPassword,
    [string]$apiKey
)

$Env:githubUsername = $githubUsername;
$Env:githubPassword = $githubPassword;
$Env:apiKey = $apiKey;

$version = .\packages\tools\GitVersion.CommandLine\tools\GitVersion.exe /showvariable MajorMinorPatch;
.\build.ps1 -Target Pre-Release;
git commit -am "Preparing for release $version.";
$releaseBranch = git rev-parse --abbrev-ref HEAD;
git checkout master;
git merge $releaseBranch;
git push;
git tag -a "$version" -m "$version";
git push origin "$version";
.\build.ps1 -Target Release;