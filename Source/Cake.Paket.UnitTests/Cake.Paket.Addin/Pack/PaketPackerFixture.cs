using Cake.Paket.Addin.Pack;
using Cake.Paket.Addin.Tooling;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Pack
{
    /// <summary>
    /// Mock of PaketPacker class.
    /// </summary>
    internal sealed class PaketPackerFixture : ToolFixture<PaketPackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackerFixture" /> class.
        /// </summary>
        /// <param name="fakeDirectory">The output directory.</param>
        internal PaketPackerFixture(string fakeDirectory)
            : base("paket.exe")
        {
            FakeDirectory = FileSystem.CreateDirectory(fakeDirectory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackerFixture" /> class with the directory output = NuGet.
        /// </summary>
        internal PaketPackerFixture()
            : this("NuGet")
        {
        }

        private FakeDirectory FakeDirectory { get; }

        /// <summary>
        /// Runs PaketPacker tool
        /// </summary>
        protected override void RunTool()
        {
            var resolver = new PaketToolResolver(FileSystem, Environment, Tools, ProcessRunner, new FakeArguments(), new FakeLog());
            var packer = new PaketPacker(FileSystem, Environment, Tools, ProcessRunner, resolver);
            packer.Pack(FakeDirectory.Path, Settings);
        }
    }
}
