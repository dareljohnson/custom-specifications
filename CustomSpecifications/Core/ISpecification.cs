namespace CustomSpecifications.Core;

/// <summary>
/// Defines a contract for specifications that can be used to encapsulate business rules
/// and combine them using Boolean logic operations.
/// </summary>
/// <typeparam name="T">The type of object to be evaluated by this specification.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Determines whether the specified candidate satisfies this specification.
    /// </summary>
    /// <param name="candidate">The object to be evaluated.</param>
    /// <returns>true if the candidate satisfies the specification; otherwise, false.</returns>
    bool IsSatisfiedBy(T candidate);

    /// <summary>
    /// Creates a new specification that is satisfied when both this specification
    /// and the other specification are satisfied.
    /// </summary>
    /// <param name="other">The specification to combine with AND logic.</param>
    /// <returns>A new specification representing the AND combination.</returns>
    ISpecification<T> And(ISpecification<T> other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is satisfied
    /// and the other specification is not satisfied.
    /// </summary>
    /// <param name="other">The specification to combine with AND NOT logic.</param>
    /// <returns>A new specification representing the AND NOT combination.</returns>
    ISpecification<T> AndNot(ISpecification<T> other);

    /// <summary>
    /// Creates a new specification that is satisfied when either this specification
    /// or the other specification is satisfied.
    /// </summary>
    /// <param name="other">The specification to combine with OR logic.</param>
    /// <returns>A new specification representing the OR combination.</returns>
    ISpecification<T> Or(ISpecification<T> other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is satisfied
    /// or the other specification is not satisfied.
    /// </summary>
    /// <param name="other">The specification to combine with OR NOT logic.</param>
    /// <returns>A new specification representing the OR NOT combination.</returns>
    ISpecification<T> OrNot(ISpecification<T> other);

    /// <summary>
    /// Creates a new specification that is satisfied when this specification is not satisfied.
    /// </summary>
    /// <returns>A new specification representing the NOT operation.</returns>
    ISpecification<T> Not();
}
