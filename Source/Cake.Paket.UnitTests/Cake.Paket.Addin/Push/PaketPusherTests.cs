using System;
using Cake.Core;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Push
{
    /// <summary>
    /// PaketPusher unit tests
    /// </summary>
    public sealed class PaketPusherTests
    {
        /// <summary>
        /// Should throw if settings are null.
        /// </summary>
        [Fact]
        public void ShouldThrowIfSettingsAreNull()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = null };

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
            var fixture = new PaketPusherFixture();
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
            var fixture = new PaketPusherFixture { Settings = { ToolPath = toolPath } };
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
            var fixture = new PaketPusherFixture();

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
            var fixture = new PaketPusherFixture();

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
            var fixture = new PaketPusherFixture();
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
            var fixture = new PaketPusherFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process returned an error (exit code 1).");
        }

        /// <summary>
        /// Should set file.
        /// </summary>
        [Fact]
        public void ShouldSetFile()
        {
            // Given
            var fixture = new PaketPusherFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push file ""/Working/NuGet/foo.nupkg""");
        }

        /// <summary>
        /// Should set url.
        /// </summary>
        [Fact]
        public void ShouldSetUrl()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { Url = "www.google.com" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push file ""/Working/NuGet/foo.nupkg"" url ""www.google.com""");
        }

        /// <summary>
        /// Should set apikey.
        /// </summary>
        [Fact]
        public void ShouldSetApiKey()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { ApiKey = "00000000-0000-0000-0000-000000000000" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push file ""/Working/NuGet/foo.nupkg"" apikey ""00000000-0000-0000-0000-000000000000""");
        }

        /// <summary>
        /// Should set endpoint.
        /// </summary>
        [Fact]
        public void ShouldSetEndPoint()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { EndPoint = "/api/v3/package" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push file ""/Working/NuGet/foo.nupkg"" endpoint ""/api/v3/package""");
        }
    }
}
