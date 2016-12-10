// Tools
#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=coveralls.net
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#tool paket:?package=ReSharperReports
#tool "paket:?package=docfx.console"

// Addins
#addin paket:?package=Cake.ReSharperReports
#addin paket:?package=Cake.Figlet
#addin paket:?package=Cake.Coveralls
#addin paket:?package=Cake.Paket
#addin "paket:?package=Cake.DocFx"

// Cake script arguments
// .\build.ps1 -Target Default -Configuration Debug -ScriptArgs "-buildVersion='1.0.0' -nuGetUrl='https://www.myget.org/F/mathphysics/api/v2/package' -nuGetApiKey='00000000-0000-0000-0000-000000000000'"
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildVersion = Argument("buildVersion", "1.0.0");
var nuGetUrl = Argument("nuGetUrl","https://www.myget.org/F/mathphysics/api/v2/package");
var nuGetApiKey = Argument("nuGetApiKey", string.Empty);

// Solution and projects
var source = "./Source";
var cakePaket = source + "/Cake.Paket.sln";
var cakePaketAddin = source + "/Cake.Paket.Addin/bin/" + configuration;
var cakePaketModule = source + "/Cake.Paket.Module/bin/" + configuration;
var cakePaketUnitTests = source + "/Cake.Paket.UnitTests/bin/" + configuration + "/*.UnitTests.dll";
var solutionInfo = source + "/SolutionInfo.cs";

// Reports and Coding standards.
var reports = "./Reports";
var coverage = reports + "/coverage.xml";
var resharperSettings = source + "/Cake.Paket.sln.DotSettings";
var inspectCodeXml = reports + "/inspectCode.xml";
var inspectCodeHtml = reports + "/inspectCode.html";
var dupFinderXml = reports + "/dupFinder.xml";
var dupFinderHtml = reports + "/dupFinder.html";

// NuGet
var nuGet = "./NuGet";

// Docs
var docs = "./docs";
var docfx = docs + "/docfx.json";
var api = docs + "/api";
var site = docs + "/site";
var docCache = "/obj/xdoc";
var cakeNuGetDocCache = source + "/Cake.NuGet/" + docCache;
var cakePaketAddinDocCache = source + "/Cake.Paket.Addin/" + docCache;
var cakePaketModuleDocCache = source + "/Cake.Paket.Module/" + docCache;

Setup(context =>
{
    Information(Figlet("Cake.Paket"));
    Information("\t\tMIT License");
    Information(string.Format("\tCopyright (c) {0} Larz White", DateTime.Now.Year));
});

Teardown(context =>
{
    DeleteFiles(reports + "/*.xml");
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
    }
});

Task("Run-InspectCode").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        EnsureDirectoryExists(reports);
        InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCodeXml });
        ReSharperReports(inspectCodeXml, inspectCodeHtml);
    }
});

Task("Run-DupFinder").IsDependentOn("Build").Does(() =>
{
    if(IsRunningOnWindows())
    {
        EnsureDirectoryExists(reports);
        DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinderXml });
        ReSharperReports(dupFinderXml, dupFinderHtml);
    }
});

Task("Paket-Pack").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(nuGet);

    if(IsRunningOnWindows())
    {
        if(HasEnvironmentVariable("APPVEYOR_BUILD_VERSION"))
        {
            buildVersion = buildVersion + "-build" + EnvironmentVariable("APPVEYOR_BUILD_VERSION");
        }

        Information("package version " + buildVersion);
        PaketPack(nuGet, new PaketPackSettings { Version = buildVersion });
    }
});

Task("Paket-Push").IsDependentOn("Paket-Pack").Does(() =>
{
    if(IsRunningOnWindows())
    {
        if(HasEnvironmentVariable("MYGET_API_TOKEN"))
        {
            nuGetApiKey = EnvironmentVariable("MYGET_API_TOKEN");
        }

        if(!string.IsNullOrWhiteSpace(nuGetApiKey))
        {
            PaketPush(GetFiles(nuGet + "/*.nupkg"), new PaketPushSettings { Url = nuGetUrl, ApiKey = nuGetApiKey });
        }
    }
});

Task("Generate-Docs").Does(() =>
{
    CleanDirectories(new[] {api, cakeNuGetDocCache, cakePaketAddinDocCache, cakePaketModuleDocCache});

    // You have to specify the tool path.
    // This is a known GitHub issue https://github.com/cake-contrib/Cake.DocFx/issues/5 for this addin.
    // It's not related to using Cake.Paket.
    DocFx(docfx, new DocFxSettings{ ToolPath = "./packages/tools/docfx.console/tools/docfx.exe" });
});

Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Paket-Push");
RunTarget(target);
