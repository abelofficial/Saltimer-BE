using Xunit;
using FluentAssertions;

namespace Saltimer.Test;

public class SaltimerTests
{
    [Fact]
    public void Test1()
    {
        // arrange
        int two = 2;
        int three = 3;

        // act
        var result = two + three;

        // assert
        result.Should().Be(5);
    }
}