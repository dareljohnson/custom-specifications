using CustomSpecifications.Core;
using CustomSpecifications.Extensions;
using Xunit;

namespace CustomSpecifications.Tests.Extensions;

public class SpecificationExtensionsTests
{
    private sealed class AlwaysTrue<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T candidate) => true;
    }

    private sealed class AlwaysFalse<T> : Specification<T>
    {
        public override bool IsSatisfiedBy(T candidate) => false;
    }

    [Fact]
    public void And_ReturnsTrue_OnlyWhenBothTrue()
    {
        var spec = new AlwaysTrue<int>().And(new AlwaysTrue<int>());
        Assert.True(spec.IsSatisfiedBy(1));

        spec = new AlwaysTrue<int>().And(new AlwaysFalse<int>());
        Assert.False(spec.IsSatisfiedBy(1));
    }

    [Fact]
    public void Or_ReturnsTrue_WhenEitherTrue()
    {
        var spec = new AlwaysTrue<int>().Or(new AlwaysFalse<int>());
        Assert.True(spec.IsSatisfiedBy(1));

        spec = new AlwaysFalse<int>().Or(new AlwaysFalse<int>());
        Assert.False(spec.IsSatisfiedBy(1));
    }

    [Fact]
    public void Not_InvertsResult()
    {
        Assert.True(new AlwaysFalse<int>().Not().IsSatisfiedBy(1));
        Assert.False(new AlwaysTrue<int>().Not().IsSatisfiedBy(1));
    }
}