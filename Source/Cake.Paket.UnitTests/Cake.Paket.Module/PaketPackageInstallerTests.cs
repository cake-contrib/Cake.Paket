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
        [Fact]
        public void Should_Be_Able_To_Install_If_Scheme_Is_Correct()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = new PackageReference("paket:?package=Cake.Core") };

            // When
            var result = fixture.CanInstall();

            // Then
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Not_Be_Able_To_Install_If_Scheme_Is_Incorrect()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = new PackageReference("homebrew:?package=Cake.Core") };

            // When
            var result = fixture.CanInstall();

            // Then
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Throw_If_ContentResolver_Is_Null()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { ContentResolver = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("contentResolver");
        }

        [Fact]
        public void Should_Throw_If_Environment_Is_Null()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Environment = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("environment");
        }

        [Fact]
        public void Should_Throw_If_Install_Path_Is_Null()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { InstallPath = null };

            // When
            Action result = () => fixture.Install();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("path");
        }

        [Fact]
        public void Should_Throw_If_Log_Is_Null()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Log = null };

            // When
            Action result = () => fixture.CreateInstaller();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("log");
        }

        [Fact]
        public void Should_Throw_If_Uri_Is_Null_For_CanInstall()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = null };

            // When
            Action result = () => fixture.CanInstall();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("package");
        }

        [Fact]
        public void Should_Throw_If_Uri_Is_Null_For_Install()
        {
            // Given
            var fixture = new PaketPackageInstallerFixture { Package = null };

            // When
            Action result = () => fixture.Install();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("package");
        }
    }
}
