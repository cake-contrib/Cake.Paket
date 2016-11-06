var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var version = Argument("version", "0.0.0-alpha");

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
    //Information(Figlet("Cake.Paket"));
    Information("\tCopyright (c) 2016 Larz White - MIT License");
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

    XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false});
    //OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath(coverage), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));

    if(AppVeyor.IsRunningOnAppVeyor && HasEnvironmentVariable("COVERALLS_REPO_TOKEN"))
    {      
        //CoverallsNet(coverage, CoverallsNetReportType.OpenCover, new CoverallsNetSettings{RepoToken = EnvironmentVariable("COVERALLS_REPO_TOKEN")});
    }
    else
    {
        Warning("\nNot pushing OpenCover results to Coveralls because the build is not on AppVeyor and/or the environment variable (repo token) does not exits.\n");
    }
});

Task("Run-InspectCode").IsDependentOn("Build").Does(() =>
{
    //InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCode });
});

Task("Run-DupFinder").IsDependentOn("Build").Does(() =>
{
    //DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinder });
});

Task("Paket-Pack").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(nuGet);

    if(AppVeyor.IsRunningOnAppVeyor && HasEnvironmentVariable("APPVEYOR_BUILD_VERSION"))
    {
        version = EnvironmentVariable("APPVEYOR_BUILD_VERSION");
    }
    else
    {
        Warning("\nUsing default versioning for nupkg because the build is not on AppVeyor and/or the environment variable does not exits.\n");
    }

    Information("\nThe nupkg version is: " + version + "\n");

    var commands = "pack output " + nuGet + " version " + version;

    //Paket(new PaketSettings { Commands = commands, ToolPath = new FilePath("./.paket/paket.exe") });
});

Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder").IsDependentOn("Paket-Pack");

RunTarget(target);