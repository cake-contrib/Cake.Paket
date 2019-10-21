using Cake.Core;
using Cake.Core.IO;
using Cake.Paket.Addin.Restore;
using Cake.Paket.Addin.Tooling;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Restore
{
    /// <summary>
    /// Mock of PaketRestorer class.
    /// </summary>
    internal class PaketRestorerFixture : ToolFixture<PaketRestoreSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketRestorerFixture" /> class with the directory output = NuGet.
        /// </summary>
        internal PaketRestorerFixture()
            : base("paket.exe")
        {
            FakeDirectory fakeDirectory = FileSystem.CreateDirectory("NuGet");
            FakeArguments = Substitute.For<ICakeArguments>();
        }

        private ICakeArguments FakeArguments { get; }

        /// <summary>
        /// Runs PaketPacker tool.
        /// </summary>
        protected override void RunTool()
        {
            var resolver = new PaketToolResolver(FileSystem, Environment, Tools, ProcessRunner, FakeArguments, new FakeLog());
            var restorer = new PaketRestorer(FileSystem, Environment, Tools, ProcessRunner, resolver);
            restorer.Restore(Settings);
        }
    }
}
