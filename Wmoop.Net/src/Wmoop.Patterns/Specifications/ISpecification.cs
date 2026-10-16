namespace Wmoop.Patterns.Specifications;

/// <summary>
/// <b>Specification</b> pattern: an encapsulated, composable boolean rule. The same
/// vocabulary is used to VALIDATE model data and to QUERY/filter the knowledge base.
/// Declared contravariant (<c>in T</c>) so a rule over a base type also applies to its
/// subtypes.
/// </summary>
public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T candidate);
}

/// <summary>Composition operators for <see cref="ISpecification{T}"/> (And / Or / Not).</summary>
public static class Spec
{
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) => new AndSpecification<T>(left, right);
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) => new OrSpecification<T>(left, right);
    public static ISpecification<T> Not<T>(this ISpecification<T> inner) => new NotSpecification<T>(inner);
}

internal sealed class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    public AndSpecification(ISpecification<T> left, ISpecification<T> right) { _left = left; _right = right; }
    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
}

internal sealed class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    public OrSpecification(ISpecification<T> left, ISpecification<T> right) { _left = left; _right = right; }
    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
}

internal sealed class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _inner;
    public NotSpecification(ISpecification<T> inner) { _inner = inner; }
    public bool IsSatisfiedBy(T candidate) => !_inner.IsSatisfiedBy(candidate);
}
