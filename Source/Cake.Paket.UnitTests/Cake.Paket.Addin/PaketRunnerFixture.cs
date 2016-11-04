using Cake.Paket.Addin;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin
{
    internal sealed class PaketRunnerFixture : ToolFixture<PaketSettings>
    {
        internal PaketRunnerFixture() : base("paket.exe")
        {
        }

        protected override void RunTool()
        {
            var tool = new PaketRunner(FileSystem, Environment, ProcessRunner, Tools, new FakeLog());
            tool.Run(Settings);
        }
    }
}
