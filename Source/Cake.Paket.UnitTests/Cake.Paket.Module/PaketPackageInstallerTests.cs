using System;
using Cake.Core.Packaging;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Module
{
    /// <summary>
    /// PaketPackageInstaller unit tests
    /// </summary>
    public sealed class PaketPackageInstallerTests
    {
        /// <summary>
        /// Should throw if environment is null.
        /// </summary>
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

        /// <summary>
        /// Should throw if content resolver is null.
        /// </summary>
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

        /// <summary>
        /// Should throw if log is null.
        /// </summary>
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

        /// <summary>
        /// Should throw if Uri is null for CanInstall.
        /// </summary>
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

        /// <summary>
        /// Should be able to install if schema is correct.
        /// </summary>
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

        /// <summary>
        /// Should not be able to install if schema is incorrect.
        /// </summary>
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

        /// <summary>
        /// Should throw is url is null for Install.
        /// </summary>
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

        /// <summary>
        /// Should throw if install path is null.
        /// </summary>
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
