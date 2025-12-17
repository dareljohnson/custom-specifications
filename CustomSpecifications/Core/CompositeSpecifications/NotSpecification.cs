namespace CustomSpecifications.Core.CompositeSpecifications;

/// <summary>
/// A specification that is satisfied when the wrapped specification is not satisfied.
/// Implements the logical NOT operation.
/// </summary>
/// <typeparam name="T">The type of object to be evaluated.</typeparam>
public sealed class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    /// <summary>
    /// Initializes a new instance of the NotSpecification class.
    /// </summary>
    /// <param name="specification">The specification to negate.</param>
    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    /// <summary>
    /// Determines whether the candidate does not satisfy the wrapped specification.
    /// </summary>
    public override bool IsSatisfiedBy(T candidate) =>
        !_specification.IsSatisfiedBy(candidate);
}
