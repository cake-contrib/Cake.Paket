using FluentAssertions;
using System;
using Xunit;

namespace Cake.Paket.UnitTests.Cake.Paket.Addin
{
    public sealed class PaketRunnerTests
    {
        [Fact]
        public void ShouldThrowIfSettingsAreNull()
        {
            // Given
            var fixture = new PaketRunnerFixture { Settings = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("settings");
        }
    }
}
