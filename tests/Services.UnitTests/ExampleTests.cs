using FluentAssertions;
using Xunit;

namespace Services.UnitTests;

public class ExampleTests
{
    [Fact]
    public void Test1()
    {
        var expected = true;
        var actual = true;
        actual.Should().Be(expected);
    }
}
