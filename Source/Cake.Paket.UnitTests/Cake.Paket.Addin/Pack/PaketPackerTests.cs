using System;
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
            var fixture = new PaketPackerFixture { Settings = null };

            // When
            Action result = () => fixture.Run();

            // Then
            result.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("settings");
        }
    }
}
