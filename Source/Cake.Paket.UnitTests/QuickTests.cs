using FluentAssertions;
using Xunit;

namespace Cake.Paket.UnitTests
{
    public class QuickTests
    {
        [Fact]
        public void Test()
        {
            var r = 1;
            r.Should().Be(1);
        }
    }
}
