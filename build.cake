#addin "Cake.Figlet"

Setup(ctx => 
{
    Information("");
    Information(Figlet("Cake.Paket"));
});

Task("Default").Does(() =>
{
    Information("Hello world");
});

RunTarget("Default");