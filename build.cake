var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

private const string cakePaket = "./Source/Cake.Paket.sln";
private readonly string cakePaketAddin = "./Source/Cake.Paket.Addin/bin/" + configuration;
private readonly string cakePaketModule = "./Source/Cake.Paket.Module/bin/" + configuration;
private readonly string cakePaketUnitTests = "./Source/Cake.Paket.UnitTests/bin/" + configuration + "/*.UnitTests.dll";

private readonly string reports = "./Reports";
private readonly string coverage = reports + "/coverage.xml";
private readonly string resharperSettings = "./Source/Cake.Paket.sln.DotSettings";
private readonly string inspectCode = reports + "/inspectCode.xml";
private readonly string dupFinder = reports + "/dupFinder.xml";

Setup(tool =>
{
    Information(Figlet("Cake.Paket"));
    Information("\tCopyright (c) 2016 Larz White - MIT License");
});

Task("Clean").Does(() =>
{
    CleanDirectories(new[] {cakePaketAddin, cakePaketModule, reports});
});

Task("Build").IsDependentOn("Clean").Does(() =>
{
    if(IsRunningOnWindows())
    {
        MSBuild(cakePaket, settings => settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(reports);

    OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath(coverage), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));

    if(AppVeyor.IsRunningOnAppVeyor && HasEnvironmentVariable("COVERALLS_REPO_TOKEN"))
    {      
        CoverallsNet(coverage, CoverallsNetReportType.OpenCover, new CoverallsNetSettings{RepoToken = EnvironmentVariable("COVERALLS_REPO_TOKEN")});
    }
    else
    {
        Warning("\nNot pushing OpenCover results to Coveralls because the build is not on AppVeyor and/or the environment variable (repo token) does not exits.\n");
    }
});

Task("Run-InspectCode").IsDependentOn("Build").Does(() =>
{
    InspectCode(cakePaket, new InspectCodeSettings{ SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCode });
});

Task("Run-DupFinder").IsDependentOn("Build").Does(() =>
{
    DupFinder(cakePaket, new DupFinderSettings { ShowStats = true, ShowText = true, OutputFile = dupFinder });
});

Task("Default").IsDependentOn("Run-Unit-Tests").IsDependentOn("Run-InspectCode").IsDependentOn("Run-DupFinder");

RunTarget("Default");