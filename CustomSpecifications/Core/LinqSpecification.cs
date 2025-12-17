using System.Linq.Expressions;
using CustomSpecifications.Core.CompositeSpecifications;

namespace CustomSpecifications.Core;

/// <summary>
/// Base class for specifications that use LINQ expression trees.
/// This enables translation to database queries (e.g., Entity Framework).
/// </summary>
/// <typeparam name="T">The type of object to be evaluated by this specification.</typeparam>
public abstract class LinqSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Gets the LINQ expression tree representing this specification.
    /// </summary>
    /// <returns>An expression tree that can be used in LINQ queries.</returns>
    public abstract Expression<Func<T, bool>> AsExpression();

    /// <summary>
    /// Determines whether the specified candidate satisfies this specification
    /// by compiling the expression tree and executing it.
    /// </summary>
    public bool IsSatisfiedBy(T candidate) => AsExpression().Compile()(candidate);

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
