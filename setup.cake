#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=Codecov
#tool paket:?package=GitVersion.CommandLine
#tool paket:?package=gitreleasemanager
#addin paket:?package=Cake.Figlet
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.Codecov
#addin paket:?package=Cake.Powershell

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
    if(IsRunningOnWindows())
    {
        MSBuild(cakePaket, settings => settings.SetConfiguration(configuration));
    }
    else
    {
      XBuild(cakePaket, settings => settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests").IsDependentOn("Build").WithCriteria(IsRunningOnWindows()).Does(() =>
{
	var cakePaketUnitTests = string.Format("./Source/Cake.Paket.UnitTests/bin/{0}/*.UnitTests.dll", configuration);
    OpenCover(tool => tool.XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false}), new FilePath("./coverage.xml"), new OpenCoverSettings().WithFilter("+[Cake.Paket.Addin]*").WithFilter("+[Cake.Paket.Module]*").WithFilter("-[Cake.Paket.UnitTests]*"));
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

Task("Commit").Does(() =>
{
	var version = GitVersion();
	var script = String.Format(@"
		git commit -am '[skip ci] Preparing for release {0}.';
		$releaseBranch = git rev-parse --abbrev-ref HEAD;
		git checkout master;
		git merge $releaseBranch;
		git push;
	", version.MajorMinorPatch);

	StartPowershellScript(script);
});

Task("Run-GitReleaseManager").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var version = GitVersion();
	var githubUsername = "larzw";
	var githubPassword = EnvironmentVariable("githubPassword");
	GitReleaseManagerCreate(githubUsername, githubPassword, "larzw", "Cake.Paket", new GitReleaseManagerCreateSettings {Milestone = version.MajorMinorPatch});
	GitReleaseManagerClose(githubUsername, githubPassword, "larzw", "Cake.Paket", version.MajorMinorPatch);
	GitReleaseManagerPublish(githubUsername, githubPassword, "larzw", "Cake.Paket", version.MajorMinorPatch);
});

Task("Paket-Pack").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var version = GitVersion();
	EnsureDirectoryExists("./nuspec");
	PaketPack("./nuspec", new PaketPackSettings { Version = version.MajorMinorPatch });	
});

Task("Paket-Push").WithCriteria(ShouldRunRelease()).Does(() =>
{
	var apiKey = EnvironmentVariable("apiKey");
	PaketPush(GetFiles("./nuspec/*.nupkg"), new PaketPushSettings { Url = "https://www.nuget.org/api/v2/package", ApiKey = apiKey });
});

Task("Default").IsDependentOn("Publish-Coverage-Report").IsDependentOn("Run-GitReleaseManager").IsDependentOn("Paket-Pack").IsDependentOn("Paket-Push");
Task("Pre-Release").IsDependentOn("Update-SolutionInfo").IsDependentOn("Commit");
RunTarget(target);

private bool ShouldRunRelease()
{
	return AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag && configuration.Equals("Release");
}