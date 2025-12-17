namespace CustomSpecifications.Core.CompositeSpecifications;

/// <summary>
/// A specification that is satisfied when both the left and right specifications are satisfied.
/// Implements the logical AND operation.
/// </summary>
/// <typeparam name="T">The type of object to be evaluated.</typeparam>
public sealed class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initializes a new instance of the AndSpecification class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// Determines whether the candidate satisfies both the left and right specifications.
    /// </summary>
    public override bool IsSatisfiedBy(T candidate) =>
        _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
}
