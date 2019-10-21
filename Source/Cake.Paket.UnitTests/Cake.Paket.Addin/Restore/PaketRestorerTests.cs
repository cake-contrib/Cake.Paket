using System;
using Cake.Core;
using Cake.Paket.Addin.Restore;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin.Restore
{
    /// <summary>
    /// PaketRestorer unit tests.
    /// </summary>
    public sealed class PaketRestorerTests
    {
        [Fact]
        public void Should_Add_Force()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { Force = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --force");
        }

        [Fact]
        public void Should_Add_OnlyReferenced()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { OnlyReferenced = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --only-referenced");
        }

        [Fact]
        public void Should_Add_TouchAffectedRefs()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { TouchAffectedRefs = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --touch-affected-refs");
        }

        [Fact]
        public void Should_Add_IgnoreChecks()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { IgnoreChecks = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --ignore-checks");
        }

        [Fact]
        public void Should_Add_FailOnChecks()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { FailOnChecks = true } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --fail-on-checks");
        }

        [Fact]
        public void Should_Set_Group()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { Group = "BuildGroup" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --group ""BuildGroup""");
        }

        [Fact]
        public void Should_Set_Project()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = { Project = "ProjectName" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args.Should().Be(@"restore --project ""ProjectName""");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new PaketRestorerFixture { Settings = null };

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
            var fixture = new PaketRestorerFixture { Settings = { ToolPath = toolPath } };
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            result.Path.FullPath.Should().Be(expected);
        }
    }
}
