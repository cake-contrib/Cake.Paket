/*
For local builds
 $ .\build.ps1 -Configuration Debug

To Create a release

 Create the BODY of the release notes (say releasenotes.md). Note, don't put this in the repository.
    $ git checkout -b release-1.0.0
    $ .\build.ps1 -Target Pre-Release -Configuration Release -ScriptArgs "-releaseNotes='C:\Users\larz\Desktop\releasenotes.md'"
    $ git add .
    $ git commit -m '[skip ci] Release setup.'
    $ git push origin head
    * Merge into master
    $ git checkout master
    $ git tag -s -a 'v1.0.0' -m 'versoin 1.0.0' (enter passphrase)
    $ git push origin v1.0.0
    $ .\build.ps1 -Target Release-On-Github -ScriptArgs "-releaseNotes='C:\Users\larz\Desktop\releasenotes.md' -gitHubUserName='username' -gitHubPassword='password'"
    $ .\build.ps1 -Target Paket-Push -ScriptArgs "-nuGetUrl='https://www.nuget.org/api/v2/package' -nuGetApiKey='00000000-0000-0000-0000-000000000000'"
*/

// Tools
#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=coveralls.net
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#tool paket:?package=ReSharperReports
#tool paket:?package=docfx.console
#tool paket:?package=GitVersion.CommandLine
#tool paket:?package=gitreleasemanager

// Addins
#addin paket:?package=Cake.ReSharperReports
#addin paket:?package=Cake.Figlet
#addin paket:?package=Cake.Coveralls
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.DocFx
#addin paket:?package=Cake.FileHelpers

// Arguments
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nuGetUrl = Argument("nuGetUrl","https://www.myget.org/F/mathphysics/api/v2/package");
var nuGetApiKey = Argument("nuGetApiKey", string.Empty);
var releaseNotes = Argument("releaseNotes", string.Empty);
var gitHubUserName = Argument("gitHubUserName", string.Empty);
var gitHubRepoOwner = Argument("gitHubRepoOwner", gitHubUserName);
var gitHubPassword = Argument("gitHubPassword", string.Empty);

// Solution and projects
var source = "./Source";
var cakePaket = string.Format("{0}/Cake.Paket.sln", source);
var cakePaketAddin = string.Format("{0}/Cake.Paket.Addin/bin/{1}", source, configuration);
var cakePaketModule = string.Format("{0}/Cake.Paket.Module/bin/{1}", source, configuration);
var cakePaketUnitTests = string.Format("{0}/Cake.Paket.UnitTests/bin/{1}/*.UnitTests.dll", source, configuration);
var solutionInfo = string.Format("{0}/SolutionInfo.cs", source);

// Reports and Coding standards.
var reports = "./Reports";
var coverage = string.Format("{0}/coverage.xml", reports);
var resharperSettings = string.Format("{0}/Cake.Paket.sln.DotSettings", source);
var inspectCodeXml = string.Format("{0}/inspectCode.xml", reports);
var inspectCodeHtml = string.Format("{0}/inspectCode.html", reports);
var dupFinderXml = string.Format("{0}/dupFinder.xml", reports);
var dupFinderHtml = string.Format("{0}/dupFinder.html", reports);

// Docs
var docs = "./docs";
var docfx = string.Format("{0}/docfx.json", docs);
var api = string.Format("{0}/api", docs);
var site = string.Format("{0}/site", docs);
var docCache = "/obj/xdoc";
var cakeNuGetDocCache = string.Format("{0}/Cake.NuGet/{1}", source, docCache);
var cakePaketAddinDocCache = string.Format("{0}/Cake.Paket.Addin/{1}", source, docCache);
var cakePaketModuleDocCache = string.Format("{0}/Cake.Paket.Module/{1}", source, docCache);

// NuGet
var nuGet = "./NuGet";
var paketTemplate = string.Format("{0}/**/paket.template", source);

// Copyright
var copyright = string.Format("Copyright (c) {0} Larz White", DateTime.Now.Year);

// Version
var version = GitVersion();

Setup(context =>
{
    Information(Figlet("Cake.Paket"));
    Information("\t\tMIT License");
    Information(string.Format("\t{0}\n", copyright));

    CleanDirectories(reports);
    EnsureDirectoryExists(reports);
});

Teardown(context =>
{
    DeleteFiles(string.Format("{0}/*.xml", reports));
});

Task("Build").Does(() =>
{
    CleanDirectories(new[] {cakePaketAddin, cakePaketModule});

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
    if(IsRunningOnWindows())
    {
        OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath(coverage), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));
    }
    else
    {
        XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false});
    }
});

Task("Publish-Coverage-Report").IsDependentOn("Run-Unit-Tests").Does(() =>
{
    if(HasEnvironmentVariable("COVERALLS_REPO_TOKEN"))
    {
        CoverallsNet(coverage, CoverallsNetReportType.OpenCover, new CoverallsNetSettings{RepoToken = EnvironmentVariable("COVERALLS_REPO_TOKEN")});
    }
});

Task("Run-InspectCode").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCodeXml });
        ReSharperReports(inspectCodeXml, inspectCodeHtml);
    }
});

Task("Run-DupFinder").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinderXml });
        ReSharperReports(dupFinderXml, dupFinderHtml);
    }
});

Task("Update-Paket-Template-Copyright").Does(() =>
{
    var copyright2Find = @"Copyright \(c\) \d{4}.*";
    ReplaceRegexInFiles(paketTemplate,copyright2Find, copyright);
});

Task("Paket-Pack").IsDependentOn("Build").IsDependentOn("Update-Paket-Template-Copyright").Does(() =>
{
    CleanDirectories(nuGet);
    EnsureDirectoryExists(nuGet);

    if(configuration.Equals("Release"))
    {
        PaketPack(nuGet, new PaketPackSettings { Version = version.MajorMinorPatch, ReleaseNotes = FileReadText(releaseNotes), BuildConfig = configuration });
    }
    else
    {
        var buildVersion = string.Format("{0}-build{1}", version.MajorMinorPatch, version.BuildMetaData);
        PaketPack(nuGet, new PaketPackSettings { Version = buildVersion, BuildConfig = configuration });
    }
});

Task("Paket-Push").Does(() =>
{
    if(HasEnvironmentVariable("MYGET_API_TOKEN"))
    {
        nuGetApiKey = EnvironmentVariable("MYGET_API_TOKEN");
    }

    if(!string.IsNullOrWhiteSpace(nuGetApiKey))
    {
        PaketPush(GetFiles(string.Format("{0}/*.nupkg",nuGet)), new PaketPushSettings { Url = nuGetUrl, ApiKey = nuGetApiKey });
    }
});

Task("Upload-AppVeyor-Artifacts").Does(() =>
{
    if(AppVeyor.IsRunningOnAppVeyor)
    {
       AppVeyor.UploadArtifact(inspectCodeHtml);
       AppVeyor.UploadArtifact(dupFinderHtml);
    }
});

Task("Update-SolutionInfo").Does(() =>
{
    CreateAssemblyInfo(solutionInfo, new AssemblyInfoSettings { Product = "Cake.Paket", Version = version.AssemblySemVer, FileVersion = version.AssemblySemVer, Copyright = copyright, ComVisible = false });
});

Task("Generate-Docs").Does(() =>
{
    CleanDirectories(new[] {api, cakeNuGetDocCache, cakePaketAddinDocCache, cakePaketModuleDocCache});

    // You have to specify the tool path.
    // This is a known GitHub issue https://github.com/cake-contrib/Cake.DocFx/issues/5 for this addin.
    // It's not related to using Cake.Paket.
    DocFx(docfx, new DocFxSettings{ ToolPath = "./packages/tools/docfx.console/tools/docfx.exe" });
});

Task("Release-On-Github").Does(() =>
{
    GitReleaseManagerCreate(gitHubUserName, gitHubPassword, gitHubRepoOwner,"Cake.Paket", new GitReleaseManagerCreateSettings{Name = string.Format("v{0}", version.MajorMinorPatch), InputFilePath = releaseNotes});
    GitReleaseManagerPublish(gitHubUserName, gitHubPassword, gitHubRepoOwner,"Cake.Paket", string.Format("v{0}", version.MajorMinorPatch));
});

Task("TravisCI").IsDependentOn("Run-Unit-Tests");
Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Paket-Pack");
Task("AppVeyor").IsDependentOn("Update-SolutionInfo").IsDependentOn("Default").IsDependentOn("Publish-Coverage-Report").IsDependentOn("Paket-Push").IsDependentOn("Upload-AppVeyor-Artifacts");
Task("Pre-Release").IsDependentOn("Update-SolutionInfo").IsDependentOn("Generate-Docs").IsDependentOn("Paket-Pack");
RunTarget(target);
