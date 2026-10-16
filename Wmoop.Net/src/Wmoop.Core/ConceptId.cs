namespace Wmoop.Core;

/// <summary>
/// WM meta-model concept: <b>Stable Global Identifier</b> (invariant #4).
/// <para>
/// Every concept and individual in the World Model carries a stable, opaque,
/// globally-unique identifier that never changes once published, even if the
/// human <c>Name</c> is corrected or translated. By convention the value is a
/// URN-like, slash-delimited path rooted in the owning domain, e.g.
/// <c>wm:nature/Person</c>, <c>wm:transport/Vehicle</c>,
/// <c>wm:transport/role/Driver</c>. The scheme is not enforced here; this type
/// only guarantees the identifier is present, non-empty and value-comparable.
/// </para>
/// </summary>
public readonly struct ConceptId : IEquatable<ConceptId>
{
    /// <summary>The raw identifier string (e.g. <c>wm:transport/Moped</c>).</summary>
    public string Value { get; }

    /// <summary>Creates a concept identifier. The value must be non-empty.</summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null or blank.</exception>
    public ConceptId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A ConceptId must be a non-empty, stable identifier.", nameof(value));
        Value = value;
    }

    /// <summary>Convenience conversion so a literal such as <c>"wm:nature/Person"</c> can be used directly.</summary>
    public static implicit operator ConceptId(string value) => new(value);

    public bool Equals(ConceptId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override bool Equals(object? obj) => obj is ConceptId other && Equals(other);
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);
    public override string ToString() => Value ?? string.Empty;

    public static bool operator ==(ConceptId left, ConceptId right) => left.Equals(right);
    public static bool operator !=(ConceptId left, ConceptId right) => !left.Equals(right);
}
