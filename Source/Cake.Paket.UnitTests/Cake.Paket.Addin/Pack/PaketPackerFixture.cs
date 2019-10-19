using Cake.Core;
using Cake.Core.IO;
using Cake.Paket.Addin.Pack;
using Cake.Paket.Addin.Tooling;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Mock of PaketPacker class.
    /// </summary>
    internal class PaketPackerFixture : ToolFixture<PaketPackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackerFixture" /> class with the directory output = NuGet.
        /// </summary>
        internal PaketPackerFixture()
            : base("paket.exe")
        {
            FakeDirectory fakeDirectory = FileSystem.CreateDirectory("NuGet");
            Output = fakeDirectory.Path;
            FakeArguments = Substitute.For<ICakeArguments>();
        }

        internal DirectoryPath Output { get; set; }

        private ICakeArguments FakeArguments { get; }

        /// <summary>
        /// Runs PaketPacker tool.
        /// </summary>
        protected override void RunTool()
        {
            var resolver = new PaketToolResolver(FileSystem, Environment, Tools, ProcessRunner, FakeArguments, new FakeLog());
            var packer = new PaketPacker(FileSystem, Environment, Tools, ProcessRunner, resolver);
            packer.Pack(Output, Settings);
        }
    }
}
