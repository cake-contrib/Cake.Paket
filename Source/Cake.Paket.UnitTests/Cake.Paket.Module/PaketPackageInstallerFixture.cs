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
    internal sealed class PaketPackageInstallerFixture
    {
        internal ICakeEnvironment Environment { get; set; }

        internal INuGetContentResolver ContentResolver { get; set; }

        internal ICakeLog Log { get; set; }

        internal PackageReference Package { get; set; }

        internal PackageType PackageType { get; set; }

        internal DirectoryPath InstallPath { get; set; }

        internal PaketPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            ContentResolver = Substitute.For<INuGetContentResolver>();
            Log = new FakeLog();
            Package = new PackageReference("paket:?package=Cake.Foo&group=build/setup");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./packages");
        }

        internal PaketPackageInstaller CreateInstaller()
        {
            return new PaketPackageInstaller(Environment, ContentResolver, Log);

        }

        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
