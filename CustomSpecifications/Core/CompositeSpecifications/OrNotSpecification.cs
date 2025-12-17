namespace CustomSpecifications.Core.CompositeSpecifications;

/// <summary>
/// A specification that is satisfied when the left specification is satisfied
/// or the right specification is not satisfied.
/// Implements the logical OR NOT operation.
/// </summary>
/// <typeparam name="T">The type of object to be evaluated.</typeparam>
public sealed class OrNotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initializes a new instance of the OrNotSpecification class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification to negate.</param>
    public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// Determines whether the candidate satisfies the left specification
    /// or does not satisfy the right specification.
    /// </summary>
    public override bool IsSatisfiedBy(T candidate) =>
        _left.IsSatisfiedBy(candidate) || !_right.IsSatisfiedBy(candidate);
}
