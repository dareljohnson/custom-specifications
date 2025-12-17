using CustomSpecifications.Core.CompositeSpecifications;

namespace CustomSpecifications.Core;

/// <summary>
/// Abstract base class for specifications that provides default implementations
/// of Boolean composition operations.
/// </summary>
/// <typeparam name="T">The type of object to be evaluated by this specification.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Determines whether the specified candidate satisfies this specification.
    /// Must be implemented by derived classes to define the actual business rule.
    /// </summary>
    /// <param name="candidate">The object to be evaluated.</param>
    /// <returns>true if the candidate satisfies the specification; otherwise, false.</returns>
    public abstract bool IsSatisfiedBy(T candidate);

    /// <summary>
    /// Creates a new specification that is satisfied when both this specification
    /// and the other specification are satisfied.
    /// </summary>
    public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is satisfied
    /// and the other specification is not satisfied.
    /// </summary>
    public ISpecification<T> AndNot(ISpecification<T> other) => new AndNotSpecification<T>(this, other);

    /// <summary>
    /// Creates a new specification that is satisfied when either this specification
    /// or the other specification is satisfied.
    /// </summary>
    public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is satisfied
    /// or the other specification is not satisfied.
    /// </summary>
    public ISpecification<T> OrNot(ISpecification<T> other) => new OrNotSpecification<T>(this, other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is not satisfied.
    /// </summary>
    public ISpecification<T> Not() => new NotSpecification<T>(this);
}
