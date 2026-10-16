namespace Wmoop.Core;

/// <summary>
/// WM meta-model concept: <b>Unit-bearing Quantity</b> (invariant #4).
/// <para>
/// A physical/measurable property is never a bare number in the World Model: it is
/// always a magnitude paired with the <em>unit symbol</em> it is expressed in, so
/// that facts are unambiguous across languages and target implementations. This is
/// a deliberately minimal value type — a full dimensional-analysis system (SI base
/// dimensions, conversions) is a future L0 extension; here we only guarantee that a
/// value and its unit always travel together.
/// </para>
/// <para>
/// A dimensionless count (e.g. number of wheels or legs) is modelled as a plain
/// <see cref="int"/>, not a <see cref="Quantity"/>, because it carries no unit.
/// </para>
/// </summary>
public readonly struct Quantity : IEquatable<Quantity>
{
    /// <summary>The numeric magnitude.</summary>
    public double Value { get; }

    /// <summary>The unit symbol the magnitude is expressed in (e.g. <c>km/h</c>, <c>cc</c>, <c>kg</c>).</summary>
    public string Unit { get; }

    /// <summary>Creates a quantity. The unit must be supplied.</summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="unit"/> is null or blank.</exception>
    public Quantity(double value, string unit)
    {
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("A Quantity must carry a non-empty unit symbol.", nameof(unit));
        Value = value;
        Unit = unit;
    }

    public bool Equals(Quantity other) =>
        Value.Equals(other.Value) && string.Equals(Unit, other.Unit, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is Quantity other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Value, Unit ?? string.Empty);

    /// <summary>Human-readable rendering, e.g. <c>45 km/h</c>.</summary>
    public override string ToString() => $"{Value:0.###} {Unit}";

    public static bool operator ==(Quantity left, Quantity right) => left.Equals(right);
    public static bool operator !=(Quantity left, Quantity right) => !left.Equals(right);
}
