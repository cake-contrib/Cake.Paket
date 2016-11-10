using System;
using Cake.Core.Packaging;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Module
{
    public sealed class PaketPackageInstallerTests
    {
        [Fact]
        public void ShouldThrowIfEnvironmentIsNull()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Environment = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("environment");
        }

        [Fact]
        public void ShouldThrowIfContentResolverIsNull()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { ContentResolver = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("contentResolver");
        }

        [Fact]
        public void ShouldThrowIfLogIsNull()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Log = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("log");
        }

        [Fact]
        public void ShouldThrowIfUriIsNullForCanInstall()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = null };

            // When
            Action result = () => fixture.CanInstall();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("package");
        }

        [Fact]
        public void ShouldBeAbleToInstallIfSchemeIsCorrect()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = new PackageReference("paket:?package=Cake.Core") };

            // When
            var result = fixture.CanInstall();

            // Then
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotBeAbleToInstallIfSchemeIsIncorrect()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = new PackageReference("homebrew:?package=Cake.Core") };

            // When
            var result = fixture.CanInstall();

            // Then
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldThrowIfUriIsNullForInstall()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = null };

            // When
            Action result = () => fixture.Install();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("package");
        }

        [Fact]
        public void ShouldThrowIfInstallPathIsNull()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { InstallPath = null };

            // When
            Action result = () => fixture.Install();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("path");
        }
    }
}
