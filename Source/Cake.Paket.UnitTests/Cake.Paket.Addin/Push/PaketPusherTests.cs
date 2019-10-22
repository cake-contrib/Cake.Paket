using System;
using Cake.Core;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Push
{
    /// <summary>
    /// PaketPusher unit tests.
    /// </summary>
    public sealed class PaketPusherTests
    {
        [Fact]
        public void Should_Find_Paket_Executable_If_Tool_Path_Not_Provided()
        {
            // Given
            var fixture = new PaketPusherFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be("/Working/tools/paket.exe");
        }

        [Fact]
        public void Should_Set_ApiKey()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { ApiKey = "00000000-0000-0000-0000-000000000000" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push ""/Working/NuGet/foo.nupkg"" --api-key ""00000000-0000-0000-0000-000000000000""");
        }

        [Fact]
        public void Should_Set_EndPoint()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { EndPoint = "/api/v3/package" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push ""/Working/NuGet/foo.nupkg"" --endpoint ""/api/v3/package""");
        }

        [Fact]
        public void Should_Set_File()
        {
            // Given
            var fixture = new PaketPusherFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push ""/Working/NuGet/foo.nupkg""");
        }

        [Fact]
        public void Should_Set_Url()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { Url = "www.google.com" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"push ""/Working/NuGet/foo.nupkg"" --url ""www.google.com""");
        }

        [Fact]
        public void Should_Set_WorkingDirectory()
        {
            // Given
            var fixture = new PaketPusherFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Process.WorkingDirectory.FullPath.Should().Be("/Working");
        }

        [Fact]
        public void Should_Throw_If_Paket_Executable_Was_Not_Found()
        {
            // Given
            var fixture = new PaketPusherFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Could not locate paket.exe.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_ExitCode()
        {
            // Given
            var fixture = new PaketPusherFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new PaketPusherFixture();
            fixture.GivenProcessCannotStart();

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_FilePath_Is_Null()
        {
            // Given
            var fixture = new PaketPusherFixture { FilePath = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("filePath");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("settings");
        }

        [Theory]
        [InlineData("/bin/tools/.paket/paket.exe", "/bin/tools/.paket/paket.exe")]
        [InlineData("./.paket/paket.exe", "/Working/.paket/paket.exe")]
        public void Should_Use_Paket_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new PaketPusherFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }
    }
}
