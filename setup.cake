#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#tool paket:?package=ReSharperReports
#tool paket:?package=GitVersion.CommandLine
#tool paket:?package=Codecov
#tool paket:?package=Wyam

#addin paket:?package=Cake.ReSharperReports
#addin paket:?package=Cake.Figlet
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.Codecov
#addin paket:?package=Cake.Wyam

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var source = "./Source";
var cakePaket = string.Format("{0}/Cake.Paket.sln", source);
var cakePaketAddin = string.Format("{0}/Cake.Paket.Addin/bin/{1}", source, configuration);
var cakePaketModule = string.Format("{0}/Cake.Paket.Module/bin/{1}", source, configuration);
var cakePaketUnitTests = string.Format("{0}/Cake.Paket.UnitTests/bin/{1}/*.UnitTests.dll", source, configuration);
var solutionInfo = string.Format("{0}/SolutionInfo.cs", source);

var reports = "./Reports";
var nuGet = "./NuGet";
var coverage = string.Format("{0}/coverage.xml", reports);
var inspectCodeHtml = string.Format("{0}/inspectCode.html", reports);
var dupFinderHtml = string.Format("{0}/dupFinder.html", reports);
var version = GitVersion();

Setup(context =>
{
    Information(Figlet("Cake.Paket"));
    CleanDirectories(new[] {reports, nuGet, cakePaketAddin, cakePaketModule});
});

Task("Build").Does(() =>
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
    if(IsRunningOnWindows())
    {
        OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath(coverage), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));
    }
    else
    {
        XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false});
    }
});

Task("Publish-Coverage-Report").IsDependentOn("Run-Unit-Tests").WithCriteria(AppVeyor.IsRunningOnAppVeyor).Does(() =>
{
    Codecov(coverage);
});

Task("Run-InspectCode").IsDependentOn("Build").WithCriteria(IsRunningOnWindows()).Does(() =>
{
    EnsureDirectoryExists(reports);
    var resharperSettings = string.Format("{0}/Cake.Paket.sln.DotSettings", source);
    var inspectCodeXml = string.Format("{0}/inspectCode.xml", reports);
    InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCodeXml });
    ReSharperReports(inspectCodeXml, inspectCodeHtml);
});

Task("Run-DupFinder").IsDependentOn("Build").WithCriteria(IsRunningOnWindows()).Does(() =>
{
    EnsureDirectoryExists(reports);
    var dupFinderXml = string.Format("{0}/dupFinder.xml", reports);
    DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinderXml });
    ReSharperReports(dupFinderXml, dupFinderHtml);
});

Task("Upload-AppVeyor-Artifacts").WithCriteria(AppVeyor.IsRunningOnAppVeyor).Does(() =>
{
    AppVeyor.UploadArtifact(inspectCodeHtml);
    AppVeyor.UploadArtifact(dupFinderHtml);
});

Task("Update-SolutionInfo").Does(() =>
{
    var copyright = string.Format("Copyright (c) {0} Larz White", DateTime.Now.Year);
    CreateAssemblyInfo(solutionInfo, new AssemblyInfoSettings { Product = "Cake.Paket", Version = version.AssemblySemVer, FileVersion = version.AssemblySemVer, Copyright = copyright, ComVisible = false });
});

Task("Generate-Docs").Does(() =>
{
    Wyam(new WyamSettings(){RootPath = Directory("./docs")});
});


Task("TravisCI").IsDependentOn("Run-Unit-Tests");
Task("AppVeyor").IsDependentOn("Publish-Coverage-Report").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Upload-AppVeyor-Artifacts").IsDependentOn("Update-SolutionInfo").IsDependentOn("Generate-Docs");
Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Update-SolutionInfo").IsDependentOn("Generate-Docs");
RunTarget(target);
