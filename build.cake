#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=coveralls.net
#tool paket:?package=JetBrains.ReSharper.CommandLineTools
#tool paket:?package=ReSharperReports
#addin paket:?package=Cake.ReSharperReports
#addin paket:?package=Cake.Figlet
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
var inspectCodeXml = reports + "/inspectCode.xml";
var inspectCodeHtml = reports + "/inspectCode.html";
var dupFinderXml = reports + "/dupFinder.xml";
var dupFinderHtml = reports + "/dupFinder.html";

var nuGet = "./NuGet";
var myGetUrl = "https://www.myget.org/F/mathphysics/api/v2/package";

Setup(context =>
{
    Information(Figlet("Cake.Paket"));
    Information("\t\tMIT License");
    Information("\tCopyright (c) 2016 Larz White");
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
        Warning("\nNot pushing OpenCover results to Coveralls because the build is not on windows and/or the environment variable (repo token) does not exits.\n");
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

    if(HasEnvironmentVariable("APPVEYOR_BUILD_VERSION") && IsRunningOnWindows())
    {
        buildVersion = EnvironmentVariable("APPVEYOR_BUILD_VERSION");
        Information("\nThe nupkg version is: " + buildVersion + "\n");
        PaketPack(nuGet, new PaketPackSettings { Version = buildVersion });
    }
    else
    {
        Warning("\nUsing default versioning for nupkg because the build is not on windows and/or the environment variable does not exits.\n");
    }
});

Task("Paket-Push").IsDependentOn("Paket-Pack").Does(() =>
{
    if(HasEnvironmentVariable("MYGET_API_TOKEN") && IsRunningOnWindows())
    {
        PaketPush(GetFiles(nuGet + "/*.nupkg"), new PaketPushSettings { Url = myGetUrl, ApiKey = EnvironmentVariable("MYGET_API_TOKEN") });
    }
    else
    {
        Warning("\nNot pushing nupkg because the build is not on windows and/or the environment variable does not exits.\n");
    }
});

Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Paket-Push");
RunTarget(target);
