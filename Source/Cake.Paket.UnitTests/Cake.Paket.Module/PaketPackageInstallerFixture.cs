using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.NuGet;
using Cake.Paket.Module;
using Cake.Testing;
using NSubstitute;

namespace Cake.Paket.UnitTests.Cake.Paket.Module
{
    /// <summary>
    /// Mock of PaketPusher class.
    /// </summary>
    internal sealed class PaketPackageInstallerFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaketPackageInstallerFixture"/> class.
        /// </summary>
        internal PaketPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            ContentResolver = Substitute.For<INuGetContentResolver>();
            Log = new FakeLog();
            Package = new PackageReference("paket:?package=Cake.Foo&group=build/setup");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./packages");
        }

        /// <summary>
        /// Gets or sets the enviornment.
        /// </summary>
        internal ICakeEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the content resolver.
        /// </summary>
        internal INuGetContentResolver ContentResolver { get; set; }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        internal ICakeLog Log { get; set; }

        /// <summary>
        /// Gets or sets the package.
        /// </summary>
        internal PackageReference Package { get; set; }

        /// <summary>
        /// Gets or sets the package type.
        /// </summary>
        internal PackageType PackageType { get; set; }

        /// <summary>
        /// Gets or sets the install path.
        /// </summary>
        internal DirectoryPath InstallPath { get; set; }

        /// <summary>
        /// Create the installer.
        /// </summary>
        /// <returns>The paket package installer.</returns>
        internal PaketPackageInstaller CreateInstaller()
        {
            return new PaketPackageInstaller(Environment, ContentResolver, Log);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <returns>The installed files.</returns>
        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <returns><c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.</returns>
        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
