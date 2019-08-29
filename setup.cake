#tool paket:?package=xunit.runner.console&version=2.4.1
#tool paket:?package=OpenCover&version=4.7.922
#tool paket:?package=Codecov&version=1.7.1
#tool paket:?package=GitVersion.CommandLine&version=5.0
#tool paket:?package=gitreleasemanager&version=0.8
#addin paket:?package=Cake.Figlet&version=1.3
#addin paket:?package=Cake.Paket&version=4.0
#addin paket:?package=Cake.Codecov&version=0.6

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Clean").Does(() =>
{
    Information(Figlet("Cake.Paket"));

	if(FileExists("./coverage.xml"))
	{
		DeleteFile("./coverage.xml");
	}

	var cakePaketAddin = string.Format("./Source/Cake.Paket.Addin/bin/{0}", configuration);
	var cakePaketModule = string.Format("./Source/Cake.Paket.Module/bin/{0}", configuration);
    CleanDirectories(new[] {"./nuspec", cakePaketAddin, cakePaketModule});
});

Task("Build").IsDependentOn("Clean").Does(() =>
{
	PaketRestore();
	var cakePaket = "./Source/Cake.Paket.sln";

    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    DotNetCoreBuild(cakePaket, settings);
});

Task("Run-Unit-Tests").IsDependentOn("Build").WithCriteria(IsRunningOnWindows()).Does(() =>
{
    OpenCover(
        tool => tool.DotNetCoreTest(
            "./Source/Cake.Paket.UnitTests/Cake.Paket.UnitTests.csproj",
            new DotNetCoreTestSettings() { Configuration = "Debug" }),
        new FilePath("./coverage.xml"),
        new OpenCoverSettings()
        {
            OldStyle = true
        }
            .WithFilter("+[Cake.Paket.Addin]*")
            .WithFilter("+[Cake.Paket.Module]*")
            .WithFilter("-[Cake.Paket.UnitTests]*"));
});

Task("Publish-Coverage-Report").IsDependentOn("Run-Unit-Tests").WithCriteria(AppVeyor.IsRunningOnAppVeyor).Does(() =>
{
    Codecov("./coverage.xml");
});

Task("Update-SolutionInfo").Does(() =>
{
	var solutionInfo = "./Source/SolutionInfo.cs";
	GitVersion(new GitVersionSettings { UpdateAssemblyInfo = true, UpdateAssemblyInfoFilePath = solutionInfo});
});

Task("Run-GitReleaseManager").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var version = GitVersion();
	var githubUsername = EnvironmentVariable("GITHUB_USERNAME");
	var githubPassword = EnvironmentVariable("GITHUB_PASSWORD");
	GitReleaseManagerCreate(githubUsername, githubPassword, githubUsername, "Cake.Paket", new GitReleaseManagerCreateSettings {Milestone = version.MajorMinorPatch});
	GitReleaseManagerClose(githubUsername, githubPassword, githubUsername, "Cake.Paket", version.MajorMinorPatch);
	GitReleaseManagerPublish(githubUsername, githubPassword, githubUsername, "Cake.Paket", version.MajorMinorPatch);
});

Task("Paket-Pack").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var version = GitVersion();
	EnsureDirectoryExists("./nuspec");
	PaketPack("./nuspec", new PaketPackSettings { Version = version.MajorMinorPatch });
});

Task("Paket-Push").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var apiKey = EnvironmentVariable("NUGET_API_KEY");
	PaketPush(GetFiles("./nuspec/*.nupkg"), new PaketPushSettings { Url = "https://www.nuget.org/api/v2/package", ApiKey = apiKey });
});

Task("Default").IsDependentOn("Publish-Coverage-Report").IsDependentOn("Run-GitReleaseManager").IsDependentOn("Paket-Pack").IsDependentOn("Paket-Push");
Task("Pre-Release").IsDependentOn("Update-SolutionInfo");
RunTarget(target);

private bool ShouldRunRelease()
{
	return AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag;
}
