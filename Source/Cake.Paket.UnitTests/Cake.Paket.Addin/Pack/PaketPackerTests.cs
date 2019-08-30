using System;
using Cake.Core;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Pack
{
    /// <summary>
    /// PaketPacker unit tests
    /// </summary>
    public sealed class PaketPackerTests
    {
        [Fact]
        public void Should_Add_IncludeReferencedProjects()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { IncludeReferencedProjects = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --include-referenced-projects");
        }

        [Fact]
        public void Should_Add_LockDependencies()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { LockDependencies = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --lock-dependencies");
        }

        [Fact]
        public void Should_Add_MinimumFromLockFile()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { MinimumFromLockFile = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --minimum-from-lock-file");
        }

        [Fact]
        public void Should_Add_PinProjectReferences()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { PinProjectReferences = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --pin-project-references");
        }

        [Fact]
        public void Should_Add_Symbols()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { Symbols = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --symbols");
        }

        [Fact]
        public void Should_Find_Paket_Executable_If_Tool_Path_Not_Provided()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be("/Working/tools/paket.exe");
        }

        [Fact]
        public void Should_Set_BuildConfig()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { BuildConfig = "config" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --build-config ""config""");
        }

        [Fact]
        public void Should_Set_BuildPlatform()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { BuildPlatform = "net45" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --build-platform ""net45""");
        }

        [Fact]
        public void Should_Set_Exclude()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { Exclude = "Cake.Foo" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --exclude ""Cake.Foo""");
        }

        [Fact]
        public void Should_Set_ProjectUrl()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { ProjectUrl = "www.google.com" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --project-url ""www.google.com""");
        }

        [Fact]
        public void Should_Set_ReleaseNotes()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { ReleaseNotes = "Initial Release" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --release-notes ""Initial Release""");
        }

        [Fact]
        public void Should_Set_SpecificVersion()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { SpecificVersion = "Cake.Foo 0.0.0" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --specific-version ""Cake.Foo 0.0.0""");
        }

        [Fact]
        public void Should_Set_TemplateFile()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { TemplateFile = "/Working/Template" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --template ""/Working/Template""");
        }

        [Fact]
        public void Should_Set_Version()
        {
            // Given
            var fixture = new PaketPackerFixture { Settings = { Version = "1.0.0" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet"" --version ""1.0.0""");
        }

        [Fact]
        public void Should_Set_WorkingDirectory()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Process.WorkingDirectory.FullPath.Should().Be("/Working");
        }

        [Fact]
        public void Should_SetOutput()
        {
            // Given
            var fixture = new PaketPackerFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"pack ""/Working/NuGet""");
        }

        [Fact]
        public void Should_Throw_If_Paket_Executable_Was_Not_Found()
        {
            // Given
            var fixture = new PaketPackerFixture();
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
            var fixture = new PaketPackerFixture();
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
            var fixture = new PaketPackerFixture();
            fixture.GivenProcessCannotStart();

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<CakeException>().WithMessage("Paket: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new PaketPackerFixture() { Settings = null };

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
            var fixture = new PaketPackerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }
    }
}
