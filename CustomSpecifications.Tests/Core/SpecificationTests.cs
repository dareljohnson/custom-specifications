using CustomSpecifications.Core;
using CustomSpecifications.Extensions;
using Xunit;

namespace CustomSpecifications.Tests.Core;

public class SpecificationTests
{
    private sealed class EvenSpec : Specification<int>
    {
        public override bool IsSatisfiedBy(int candidate) => candidate % 2 == 0;
    }

    private sealed class PositiveSpec : Specification<int>
    {
        public override bool IsSatisfiedBy(int candidate) => candidate > 0;
    }

    [Fact]
    public void And_ComposesSpecifications()
    {
        var spec = new EvenSpec().And(new PositiveSpec());
        Assert.True(spec.IsSatisfiedBy(2));
        Assert.False(spec.IsSatisfiedBy(-2));
        Assert.False(spec.IsSatisfiedBy(3));
    }

    [Fact]
    public void Or_ComposesSpecifications()
    {
        var spec = new EvenSpec().Or(new PositiveSpec());
        Assert.True(spec.IsSatisfiedBy(2));
        Assert.True(spec.IsSatisfiedBy(3));
        Assert.False(spec.IsSatisfiedBy(-3));
    }

    [Fact]
    public void Not_NegatesSpecification()
    {
        var spec = new EvenSpec().Not();
        Assert.True(spec.IsSatisfiedBy(3));
        Assert.False(spec.IsSatisfiedBy(2));
    }

    [Fact]
    public void Linq_Where_WithSpecification_Works()
    {
        var numbers = Enumerable.Range(-3, 7).ToList(); // -3..3
        var evens = numbers.Where(new EvenSpec());
        Assert.Equal(new[] { -2, 0, 2 }, evens);
    }
}