#tool paket:?package=xunit.runner.console
#tool paket:?package=OpenCover
#tool paket:?package=Codecov
#addin paket:?package=Cake.Figlet
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.Codecov

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Setup(context =>
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

Task("Build").Does(() =>
{
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

Task("Default").IsDependentOn("Publish-Coverage-Report");
RunTarget(target);
