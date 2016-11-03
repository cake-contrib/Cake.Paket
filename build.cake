var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

private const string cakePaket = "./Source/Cake.Paket.sln";
private readonly string cakePaketAddin = "./Source/Cake.Paket.Addin/bin/" + configuration;
private readonly string cakePaketModule = "./Source/Cake.Paket.Module/bin/" + configuration;
private readonly string cakePaketUnitTests = "./Source/Cake.Paket.UnitTests/bin/" + configuration + "/*.UnitTests.dll";

Setup(tool =>
{
    Information(Figlet("Cake.Paket"));
    Information("\tCopyright (c) 2016 Larz White - MIT License");
});

Task("Clean").Does(() =>
{
    CleanDirectories(new[] {cakePaketAddin, cakePaketModule});
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
    XUnit2(cakePaketUnitTests, new XUnit2Settings {ShadowCopy = false});
});

Task("Default").IsDependentOn("Run-Unit-Tests");

RunTarget("Default");