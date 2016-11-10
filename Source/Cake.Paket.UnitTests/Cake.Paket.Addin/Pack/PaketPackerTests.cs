using System;
using Cake.Core;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Pack
{
    /// <summary>
    /// PaketRunner unit tests
    /// </summary>
    public sealed class PaketPackerTests
    {
        /// <summary>
        /// Should throw if settings are null.
        /// </summary>
        [Fact]
        public void ShouldThrowIfSettingsAreNull()
        {
            // Given
            var fixture = new PaketPackerFixture() { Settings = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("settings");
        }

        /// <summary>
        /// Should throw if Paket executable was not found.
        /// </summary>
        [Fact]
        public void ShouldThrowIfPaketExecutableWasNotFound()
        {
            // Given
            var fixture = new PaketPackerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Could not locate paket.exe.");
        }

        /// <summary>
        /// Should use paket.exe from tool path if provided.
        /// </summary>
        /// <param name="toolPath">The tool path.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("/bin/tools/.paket/paket.exe", "/bin/tools/.paket/paket.exe")]
        [InlineData("./.paket/paket.exe", "/Working/.paket/paket.exe")]
        public void ShouldUsePaketExecutableFromToolPathIfProvided(string toolPath, string expected)
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }

        /// <summary>
        /// Should find paket.exe if tool path not provided.
        /// </summary>
        [Fact]
        public void ShouldFindPaketExecutableIfToolPathNotProvided()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be("/Working/tools/paket.exe");
        }

        /// <summary>
        /// Should set working directory.
        /// </summary>
        [Fact]
        public void ShouldSetWorkingDirectory()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Process.WorkingDirectory.FullPath.Should().Be("/Working");
        }

        /// <summary>
        /// Should throw if process was not started.
        /// </summary>
        [Fact]
        public void ShouldThrowIfProcessWasNotStarted()
        {
            // Given
            var fixture = new PaketPackerFixture();
            fixture.GivenProcessCannotStart();

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process was not started.");
        }

        /// <summary>
        /// Should throw if process has a non-zero exit code.
        /// </summary>
        [Fact]
        public void ShouldThrowIfProcessHasANonZeroExitCode()
        {
            // Given
            var fixture = new PaketPackerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process returned an error (exit code 1).");
        }

        /// <summary>
        /// Should set output.
        /// </summary>
        [Fact]
        public void ShouldSetOutput()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack output ""/Working/NuGet""");
        }

        /// <summary>
        /// Should set Version.
        /// </summary>
        [Fact]
        public void ShouldSetVersion()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { Version = "1.0.0" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack output ""/Working/NuGet"" version ""1.0.0""");
        }
    }
}
