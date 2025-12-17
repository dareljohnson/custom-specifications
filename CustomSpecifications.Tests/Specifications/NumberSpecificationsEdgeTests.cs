using CustomSpecifications.Examples.Simple;
using CustomSpecifications.Core;
using CustomSpecifications.Extensions;
using Xunit;

namespace CustomSpecifications.Tests.Specifications;

public class NumberSpecificationsEdgeTests
{
    private readonly IsPositiveSpecification _positive = new();

    [Theory]
    [InlineData(int.MinValue, false)]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(int.MaxValue, true)]
    public void IsPositive_Boundaries(int candidate, bool expected)
    {
        Assert.Equal(expected, _positive.IsSatisfiedBy(candidate));
    }

    [Fact]
    public void InRange_NormalBoundaries()
    {
        var spec = new IsInRangeSpecification(1, 100);
        Assert.False(spec.IsSatisfiedBy(0));
        Assert.True(spec.IsSatisfiedBy(1));
        Assert.True(spec.IsSatisfiedBy(100));
        Assert.False(spec.IsSatisfiedBy(101));
    }

    [Fact]
    public void InRange_MinEqualsMax()
    {
        var spec = new IsInRangeSpecification(5, 5);
        Assert.False(spec.IsSatisfiedBy(4));
        Assert.True(spec.IsSatisfiedBy(5));
        Assert.False(spec.IsSatisfiedBy(6));
    }

    [Fact]
    public void Composition_LinqFiltering()
    {
        var numbers = new List<int> { -5, 0, 15, 25, 50, 75, 101 };
        var inRange = new IsInRangeSpecification(1, 100);
        var validNumbers = numbers.Where(_positive.And(inRange));
        Assert.Equal(new[] { 15, 25, 50, 75 }, validNumbers);
    }

    [Fact]
    public void NotOperator_FiltersOdds()
    {
        var numbers = Enumerable.Range(1, 10).ToList();
        var isEven = new IsEvenSpecification();
        var isOdd = isEven.Not();
        Assert.Equal(new[] { 1, 3, 5, 7, 9 }, numbers.Where(isOdd));
    }
}
