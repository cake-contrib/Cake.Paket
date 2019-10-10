using Cake.Core;
using Cake.Core.IO;
using Cake.Paket.Addin.Push;
using Cake.Paket.Addin.Tooling;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Push
{
    /// <summary>
    /// Mock of PaketPusher class.
    /// </summary>
    internal class PaketPusherFixture : ToolFixture<PaketPushSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPusherFixture" /> class.
        /// </summary>
        internal PaketPusherFixture()
            : base("paket.exe")
        {
            FakeFile fakeFile = FileSystem.CreateFile("NuGet/foo.nupkg");
            FilePath = fakeFile.Path;
            FakeArguments = Substitute.For<ICakeArguments>();
        }

        internal FilePath FilePath { get; set; }

        private ICakeArguments FakeArguments { get; }

        /// <summary>
        /// Runs PaketPusher tool
        /// </summary>
        protected override void RunTool()
        {
            var resolver = new PaketToolResolver(FileSystem, Environment, Tools, ProcessRunner, FakeArguments, new FakeLog());
            var pusher = new PaketPusher(FileSystem, Environment, Tools, ProcessRunner, resolver);
            pusher.Push(FilePath, Settings);
        }
    }
}
