using CustomSpecifications.Core;

namespace CustomSpecifications.Extensions;

/// <summary>
/// Extension methods for working with specifications in a fluent manner.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Filters a collection using a specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>A filtered collection containing only items that satisfy the specification.</returns>
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.Where(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Determines whether any element in the collection satisfies the specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>true if any element satisfies the specification; otherwise, false.</returns>
    public static bool Any<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.Any(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Determines whether all elements in the collection satisfy the specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>true if all elements satisfy the specification; otherwise, false.</returns>
    public static bool All<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.All(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Counts the number of elements that satisfy the specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>The count of elements that satisfy the specification.</returns>
    public static int Count<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.Count(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Returns the first element that satisfies the specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>The first element that satisfies the specification.</returns>
    /// <exception cref="InvalidOperationException">No element satisfies the specification.</exception>
    public static T First<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.First(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Returns the first element that satisfies the specification, or a default value if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>The first element that satisfies the specification, or default(T) if no such element exists.</returns>
    public static T? FirstOrDefault<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.FirstOrDefault(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Returns the only element that satisfies the specification.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>The only element that satisfies the specification.</returns>
    /// <exception cref="InvalidOperationException">
    /// More than one element satisfies the specification, or no element satisfies the specification.
    /// </exception>
    public static T Single<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.Single(specification.IsSatisfiedBy);
    }

    /// <summary>
    /// Returns the only element that satisfies the specification, or a default value if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="specification">The specification to test.</param>
    /// <returns>
    /// The only element that satisfies the specification, or default(T) if no such element exists.
    /// </returns>
    /// <exception cref="InvalidOperationException">More than one element satisfies the specification.</exception>
    public static T? SingleOrDefault<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(specification);

        return source.SingleOrDefault(specification.IsSatisfiedBy);
    }
}
