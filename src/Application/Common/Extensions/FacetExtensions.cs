using System.Linq.Expressions;
using System.Reflection;

namespace CouponPaymentSystem.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for mapping source entities or sequences
/// to Facet-generated types (synchronous and provider-agnostic only).
/// </summary>
public static class FacetExtensions
{
    /// <summary>
    /// Maps a single source instance to the specified facet type by invoking its generated constructor.
    /// </summary>
    /// <typeparam name="TSource">The source entity type.</typeparam>
    /// <typeparam name="TTarget">The facet type, which must have a public constructor accepting <c>TSource</c>.</typeparam>
    /// <param name="source">The source instance to map.</param>
    /// <returns>A new <typeparamref name="TTarget"/> instance populated from <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
    public static TTarget ToFacet<TSource, TTarget>(this TSource source) =>
        source is null
            ? throw new ArgumentNullException(nameof(source))
            : (TTarget)Activator.CreateInstance(typeof(TTarget), source)!;

    /// <summary>
    /// Maps a single source instance to the specified existing facet type.
    /// </summary>
    /// <typeparam name="TSource">The source entity type.</typeparam>
    /// <typeparam name="TTarget">The facet type, which must have a public constructor accepting <c>TSource</c>.</typeparam>
    /// <param name="source">The source instance to map.</param>
    /// <param name="target">The existing target instance to map </param>
    /// <returns>A new <typeparamref name="TTarget"/> instance populated from <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
    public static void ToFacet<TSource, TTarget>(this TSource source, TTarget target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        // Create a temporary mapped instance using the constructor
        var temp = (TTarget)Activator.CreateInstance(typeof(TTarget), source)!;
        var targetType = typeof(TTarget);
        var targetProperties = targetType.GetProperties();

        foreach (var targetProperty in targetProperties)
        {
            if (!targetProperty.CanWrite)
                continue;

            var value = targetProperty.GetValue(temp);
            targetProperty.SetValue(target, value);
        }
    }

    /// <summary>
    /// Maps an <see cref="IEnumerable{TSource}"/> to an <see cref="IEnumerable{TTarget}"/>
    /// via the generated constructor of the facet type.
    /// </summary>
    /// <typeparam name="TSource">The source entity type.</typeparam>
    /// <typeparam name="TTarget">The facet type, which must have a public constructor accepting <c>TSource</c>.</typeparam>
    /// <param name="source">The enumerable source of entities.</param>
    /// <returns>An <see cref="IEnumerable{TTarget}"/> containing mapped facet instances.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
    public static IEnumerable<TTarget> SelectFacets<TSource, TTarget>(
        this IEnumerable<TSource> source
    ) =>
        source is null
            ? throw new ArgumentNullException(nameof(source))
            : source.Select(item => item.ToFacet<TSource, TTarget>());

    /// <summary>
    /// Projects an <see cref="IQueryable{TSource}"/> to an <see cref="IQueryable{TTarget}"/>
    /// using the static <c>Expression&lt;Func&lt;TSource,TTarget&gt;&gt;</c> named <c>Projection</c> defined on <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source entity type.</typeparam>
    /// <typeparam name="TTarget">The facet type, which must define a public static <c>Expression&lt;Func&lt;TSource,TTarget&gt;&gt; Projection</c>.</typeparam>
    /// <param name="source">The queryable source of entities.</param>
    /// <returns>An <see cref="IQueryable{TTarget}"/> representing the projection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <typeparamref name="TTarget"/> does not define a static <c>Projection</c> property.
    /// </exception>
    public static IQueryable<TTarget> SelectFacet<TSource, TTarget>(this IQueryable<TSource> source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var prop = typeof(TTarget).GetProperty(
            "Projection",
            BindingFlags.Public | BindingFlags.Static
        );

        if (prop is null)
            throw new InvalidOperationException(
                $"Type {typeof(TTarget).Name} must define a public static Projection property."
            );

        var expr = (Expression<Func<TSource, TTarget>>)prop.GetValue(null)!;
        return source.Select(expr);
    }
}
